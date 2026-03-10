using Echoes.Domain.Common;
using Echoes.Domain.Inventories;

namespace Echoes.Domain.Characters.Persistence
{
    /// <summary>
    /// Represents the primary database aggregate for a character.
    /// Acts as an Aggregate Root for inventory and progression data.
    /// </summary>
    public class CharacterEntity : BaseEntity, ISoftDelete, IAggregateRoot
    {
        /// <summary>The display name of the character.</summary>
        public string Name { get; set; } = null!;

        /// <summary>Defines the archetype and specialization.</summary>
        public AnitoClass ClassType { get; set; }

        /// <summary>Current progression level of the character.</summary>
        public short Level { get; set; }

        /// <summary>Total lifetime experience points earned by the character.</summary>
        public long Experience { get; set; }

        /// <summary>Current health pool; fluctuates during gameplay.</summary>
        public float CurrentHealth { get; set; }

        /// <summary>Maximum health capacity.</summary>
        public float MaxHealth { get; set; }

        /// <summary>Current mana pool; fluctuates during gameplay.</summary>
        public float CurrentMana { get; set; }

        /// <summary>Maximum mana capacity.</summary>
        public float MaxMana { get; set; }

        /// <summary>Base movement speed modifier.</summary>
        public float MovementSpeed { get; set; }

        /// <summary>Flag for soft-deletion logic.</summary>
        public bool IsDeleted { get; set; }

        /// <summary>Timestamp of when the entity was soft-deleted.</summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>The raw list of all items associated with this character.</summary>
        public List<ItemInstanceRecord> Items { get; set; } = [];

        /// <summary>Filters items that are currently in the inventory (not equipped).</summary>
        public IEnumerable<ItemInstanceRecord> Inventory =>
            Items.Where(i => i.EquippedSlot == EquipmentSlot.None);

        /// <summary>Filters items that are currently equipped in a slot.</summary>
        public IEnumerable<ItemInstanceRecord> EquippedGear =>
            Items.Where(i => i.EquippedSlot != EquipmentSlot.None);
    }
}
