namespace Echoes.Shared.Network.Common.Protocol
{
    public static class PacketIds
    {
        // Session Management (100+)
        public const int EnterGame = 100;
        public const int EnterGameResponse = 101;

        // Movement Range (200+)
        public const int Move = 200;
        public const int Teleport = 201;

        // Combat Range (300+)
        public const int SkillCast = 300;
        public const int DamageEvent = 301;
    }
}
