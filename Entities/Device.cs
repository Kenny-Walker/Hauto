using Hauto.Contracts;

namespace Hauto.Entities
{
    public class Device : AuditableEntity
    {
        public string DeviceName { get; set; }
        public bool isActive { get; set; }
    }
}
