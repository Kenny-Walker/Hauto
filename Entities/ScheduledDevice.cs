using Hauto.Contracts;

namespace Hauto.Entities
{
    public class ScheduledDevice : AuditableEntity
    {
        public int DeviceId { get; set; }
        public Device Device { get; set; }
        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }
        public bool isActive { get; set; }

    }
}
