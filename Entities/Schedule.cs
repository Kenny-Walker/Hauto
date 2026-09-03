using Hauto.Contracts;

namespace Hauto.Entities
{
    public class Schedule : AuditableEntity
    {
        public string ScheduleName { get; set; }    
        public Device Device { get; set; }
        public int DeviceId { get; set; }
        public DateTime Time { get; set; }
        public bool OnRepeat { get; set; }
    }
}
