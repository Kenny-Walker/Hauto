using Hauto.Contracts;

namespace Hauto.Entities
{
    public class Device : AuditableEntity
    {
        public int UserId { get; set; }
        public int HubId { get; set; }
        public Hubs Hub { get; set; }
        public string DeviceName { get; set; }
        public bool isActive { get; set; }
    }
}
