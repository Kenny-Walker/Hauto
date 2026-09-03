namespace Hauto.Contracts
{
    public class AuditableEntity : BaseEntity, IAuditableEntity, ISoftDelete
    {
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public int LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedOn { get; set; } = DateTime.UtcNow;
        public int? DeletedBy { get; set; }
        public bool? IsDeleted { get; set; } = false;
        DateTime? IAuditableEntity.CreatedOn { get; set; }
        DateTime? IAuditableEntity.LastModifiedOn { get; set; }
    }
}
