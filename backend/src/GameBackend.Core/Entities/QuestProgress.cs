using GameBackend.Core.Entities.Base;
using GameBackend.Core.Interfaces;
using GameBackend.Shared.Enums;

namespace GameBackend.Core.Entities
{
    public class QuestProgress : BaseEntity, IPlayerOwned
    {
        public int PlayerCharacterId { get; set; }
        public int QuestTemplateId { get; set; }
        public QuestStatus Status { get; set; }

        // TODO: To be change to take into account different quest
        public int CurrentProgress { get; set; }
    }
}
