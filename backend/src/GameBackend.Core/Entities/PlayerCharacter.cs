using GameBackend.Core.Entities.Base;
using GameBackend.Core.Interfaces;
using GameBackend.Shared.Enums;

namespace GameBackend.Core.Entities
{
    public class PlayerCharacter : BaseEntity, ISoftDelete
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public string Name { get; set; }
        public CharacterClass Class { get; set; }

        public int Level { get; set; }
        public long Experience { get; set; }

        public int Health { get; set; }
        public int Mana { get; set; }

        public int ZoneId { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public float RotationY { get; set; }

        public long Perlas { get; set; }
        public int CharacterModelId { get; set; }

        public ICollection<InventoryItem> Inventory { get; set; }
        public ICollection<QuestProgress> Quests { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
