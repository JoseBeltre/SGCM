namespace SGCM.Domain.Base
{
    public abstract class BaseEntity : AuditableEntity
    {
        public int Id { get; set; }
    }
}
