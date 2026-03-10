using System.Collections.Concurrent;
using System.Linq.Expressions;
using Echoes.Application.Connection;
using Echoes.Shared.Network.Common.Protocol;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Echoes.Application.Network;

/// <summary>
/// A high-performance router that dispatches incoming polymorphic packets to their
/// corresponding <see cref="IPacketHandler{T}"/> implementations.
/// </summary>
/// <remarks>
/// This dispatcher utilizes an expression-tree compilation cache to eliminate the
/// overhead of runtime reflection. Handlers are resolved via Dependency Injection
/// and cached after the first invocation for near-native execution speed.
/// </remarks>
public class PacketDispatcher(
    IServiceProvider serviceProvider,
    ConnectionManager connectionManager,
    ILogger<PacketDispatcher> logger
)
{
    /// <summary>
    /// Stores compiled delegates for packet handling to avoid repeated reflection lookups.
    /// </summary>
    private readonly ConcurrentDictionary<Type, Action<Guid, IPacketUnion>> _cachedHandlers = new();

    /// <summary>
    /// Dispatches a packet to its registered handler.
    /// </summary>
    /// <param name="senderId">The <see cref="Guid"/> of the client that sent the packet.</param>
    /// <param name="packet">The polymorphic packet to process.</param>
    /// <remarks>
    /// If the packet type has not been dispatched before, this method compiles an expression
    /// tree to handle the invocation, which is then cached for all subsequent calls.
    /// </remarks>
    public void Dispatch(Guid senderId, IPacketUnion packet)
    {
        if (!connectionManager.IsConnected(senderId))
        {
            logger.LogWarning(
                "Unauthorized packet attempts from {SenderId}. Dropping packet.",
                senderId
            );
            return;
        }
        var type = packet.GetType();

        var handler = _cachedHandlers.GetOrAdd(type, CreateHandlerDelegate);
        handler(senderId, packet);
    }

    /// <summary>
    /// Creates a compiled <see cref="Action"/> delegate that invokes the generic handler.
    /// </summary>
    /// <param name="packetType">The specific type of the incoming packet.</param>
    /// <returns>A compiled delegate ready for execution.</returns>
    private Action<Guid, IPacketUnion> CreateHandlerDelegate(Type packetType)
    {
        var handlerType = typeof(IPacketHandler<>).MakeGenericType(packetType);
        var handler = serviceProvider.GetRequiredService(handlerType);

        if (handler == null)
        {
            // Return a "no-op" delegate so the server doesn't crash on unregistered packets
            return (sender, packet) =>
                logger.LogWarning("No handler registered for packet type {Type}", packetType);
        }
        // Build the expression: (senderId, packet) => handler.Handle(senderId, (PacketType)packet)
        var senderParam = Expression.Parameter(typeof(Guid), "senderId");
        var packetParam = Expression.Parameter(typeof(IPacketUnion), "packet");

        var method = handlerType.GetMethod("Handle")!;

        var body = Expression.Call(
            Expression.Constant(handler),
            method,
            senderParam,
            Expression.Convert(packetParam, packetType)
        );

        return Expression
            .Lambda<Action<Guid, IPacketUnion>>(body, senderParam, packetParam)
            .Compile();
    }
}
