using Echoes.Shared.Network.Common.Abstractions;
using Echoes.Shared.Network.Features.Gameplay.Combat;
using Echoes.Shared.Network.Features.Gameplay.Movement;
using MemoryPack;
using EnterGamePacket = Echoes.Shared.Network.Features.ServerSession.EnterGamePacket;

namespace Echoes.Shared.Network.Common.Protocol;

/// <summary>
/// Defines the root union type for all network packets, enabling polymorphic serialization via <see cref="MemoryPack"/>.
/// </summary>
/// <remarks>
/// This interface acts as the central registry for the packet system. By inheriting from <see cref="IPacket"/>,
/// all implementing packets are guaranteed to have routing metadata.
/// <para>
/// To add a new packet type:
/// <list type="number">
/// <item><description>Define the new record/class implementing <see cref="IPacketUnion"/>.</description></item>
/// <item><description>Assign a unique identifier in <see cref="PacketIds"/>.</description></item>
/// <item><description>Register the mapping here using the <see cref="MemoryPackUnionAttribute"/>.</description></item>
/// </list>
/// </para>
/// </remarks>
[MemoryPackUnion(PacketIds.Move, typeof(MovePacket))]
[MemoryPackUnion(PacketIds.SkillCast, typeof(SkillPacket))]
[MemoryPackUnion(PacketIds.EnterGame, typeof(EnterGamePacket))]
public interface IPacketUnion : IPacket { }
