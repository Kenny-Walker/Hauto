using Hauto.Contracts;

namespace Hauto.Entities
{
    public class Log : AuditableEntity
    {
        public int DeviceId { get; set; }
        public Device Device { get; set; }
        public string LogStatement { get; set; }
        public DateTime Date { get; set; }
    }
}
