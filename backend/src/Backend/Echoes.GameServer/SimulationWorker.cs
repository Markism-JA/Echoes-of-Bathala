using System.Diagnostics;
using Echoes.Application.Simulation.Management;

namespace Echoes.GameServer;

/// <summary>
/// The core background service responsible for driving the multi-instance game world simulation.
/// </summary>
/// <remarks>
/// This worker implements a Fixed Timestep loop to ensure deterministic logic.
/// It utilizes Parallel.ForEach to scale across multiple CPU cores for game instances.
/// </remarks>
public class SimulationWorker(ILogger<SimulationWorker> logger, InstanceManager instanceManager)
    : BackgroundService
{
    /// <summary>
    /// Executes the high-precision simulation heartbeat.
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Simulation Server starting at 60 TPS...");

        var stopwatch = Stopwatch.StartNew();
        double accumulator = 0;
        const double fixedDeltaTime = 1.0 / 60.0;
        const double maxAccumulator = 0.25;
        const double sleepThreshold = 0.005;

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                double deltaTime = stopwatch.Elapsed.TotalSeconds;
                stopwatch.Restart();
                accumulator += deltaTime;

                /* If the server stalls (e.g., GC spike), we cap the accumulator. 
                   This prevents the server from trying to "catch up" by running 
                   hundreds of ticks in a single frame. */
                if (accumulator > maxAccumulator)
                {
                    logger.LogWarning("Server Lag Detected! Dropping ticks. Accumulator: {Acc:F4}s", accumulator);
                    accumulator = maxAccumulator;
                }

                while (accumulator >= fixedDeltaTime)
                {
                    var activeInstances = instanceManager.GetActiveInstances().ToList();

                    /* MULTI-INSTANCE TICKING
                       We process instances in parallel. Fault isolation ensures that an 
                       exception in one instance only shuts down that specific world. */
                    Parallel.ForEach(activeInstances, instance =>
                    {
                        try
                        {
                            instance.Manager.Tick((float)fixedDeltaTime);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "Fault isolated in Instance {Id}. Terminating.", instance.Id);
                            instanceManager.Shutdown(instance.Id);
                        }
                    });

                    accumulator -= fixedDeltaTime;
                }

                /* SMART YIELDING
                   We use Task.Delay for long waits to be eco-friendly with CPU usage, 
                   but switch to Thread.Yield when close to the next tick to avoid 
                   the imprecise 15ms Windows timer resolution. */
                double remainingTime = fixedDeltaTime - accumulator;

                if (remainingTime > sleepThreshold)
                {
                    await Task.Delay(1, stoppingToken);
                }
                else if (remainingTime > 0)
                {
                    Thread.Yield();
                }
            }
            catch (OperationCanceledException) { break; }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Critical error in Master Simulation Loop!");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
