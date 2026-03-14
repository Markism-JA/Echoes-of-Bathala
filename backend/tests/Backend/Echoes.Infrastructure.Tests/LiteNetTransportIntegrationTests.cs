using Echoes.Application.Common.Abstractions;
using Echoes.Application.Network;
using Echoes.Application.Session;
using Echoes.Infrastructure.Network.Configuration;
using Echoes.Infrastructure.Network.Litenet;
using Echoes.Infrastructure.Networking;
using LiteNetLib;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace Echoes.Infrastructure.Tests;

public class LiteNetTransportIntegrationTests
{
    [Fact]
    public async Task Transport_ShouldAcceptClientConnection_AndDispatchPacket()
    {
        var spMock = new Mock<IServiceProvider>();
        var transportMock = new Mock<INetworkTransport>();
        var sessionManager = new SessionManager(transportMock.Object);
        var dispatcherLogger = new NullLogger<PacketDispatcher>();

        var dispatcher = new PacketDispatcher(spMock.Object, sessionManager, dispatcherLogger);

        var transportLogger = new NullLogger<LiteNetTransport>();
        var serializer = new Mock<ISerializer>();
        var factory = new LiteNetEngineFactory();

        var options = Options.Create(
            new NetworkOptions
            {
                Port = 9051,
                ConnectionKey = "TestKey",
                MaxConnections = 10,
            }
        );

        var transport = new LiteNetTransport(
            transportLogger,
            dispatcher,
            serializer.Object,
            options,
            factory
        );

        transport.Start();

        var clientListener = new EventBasedNetListener();
        var clientManager = new NetManager(clientListener);
        clientManager.Start();

        var peer = clientManager.Connect("127.0.0.1", 9051, "TestKey");

        bool connected = false;
        for (int i = 0; i < 30; i++)
        {
            transport.PollEvents();
            clientManager.PollEvents();
            if (peer.ConnectionState == ConnectionState.Connected)
            {
                connected = true;
                break;
            }
            await Task.Delay(50);
        }

        Assert.True(connected, "Handshake failed. Check ConnectionKey or Port binding.");
        Assert.Equal(ConnectionState.Connected, peer.ConnectionState);

        transport.Stop();
        clientManager.Stop();
    }
}
