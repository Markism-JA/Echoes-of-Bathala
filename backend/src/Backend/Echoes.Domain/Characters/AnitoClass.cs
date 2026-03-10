namespace Echoes.Domain.Characters
{
    /// <summary>
    /// Defines the playable class archetypes available in the simulation.
    /// </summary>
    public enum AnitoClass
    {
        /// <summary> Ranged physical damage dealer; focuses on critical hits and trapping. </summary>
        Mangangaso,

        /// <summary> Frontline tank; focuses on damage mitigation and threat generation. </summary>
        Guwardiya,

        /// <summary> Melee burst specialist; focuses on mobility, stealth, and high single-target damage. </summary>
        Lihim,

        /// <summary> Pet-based support; focuses on battlefield control through summoned entities. </summary>
        Kaanib,

        /// <summary> Primary support; focuses on health restoration and defensive buffs. </summary>
        Manggagamot,

        /// <summary> Magical glass cannon; focuses on area-of-effect damage and elemental debuffs. </summary>
        Mangkukulam,
    }
}
