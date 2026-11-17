using Hauto.Contracts;

namespace Hauto.Entities
{
    public class Hubs : AuditableEntity
    {
        public int UserId { get; set; }
        public string HubName { get; set; }
        public List<Device> Devices { get; set; }
    }
}
