using Hauto.Contracts;

namespace Hauto.Entities
{
    public class Schedule : AuditableEntity
    {
        public int UserId { get; set; }
        public string ScheduleName { get; set; }    
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bool OnRepeat { get; set; }
        public bool isActive { get; set; }
        public List<ScheduledDevice> ScheduledDevice { get; set; }
    }
}
