namespace SGCM.Domain.Base
{
    public abstract class UpdatableEntity : BaseEntity
    {
        public abstract DateTime? UpdatedAt { get; set; }
        public abstract DateTime CreatedAt { get; set; }
    }
}
