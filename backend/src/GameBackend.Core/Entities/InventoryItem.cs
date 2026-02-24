using GameBackend.Core.Entities.Base;
using GameBackend.Core.Interfaces;
using GameBackend.Shared.Enums;

namespace GameBackend.Core.Entities
{
    public class InventoryItem : BaseEntity, ISoftDelete, IPlayerOwned
    {
        public int PlayerCharacterId { get; set; }
        public int ItemTemplateId { get; set; }

        public int Quantity { get; set; }
        public int Durability { get; set; }
        public EquipmentSlot Slot { get; set; } = EquipmentSlot.None;

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
