using SGCM.Domain.Base;
using SGCM.Domain.Enums;

namespace SGCM.Domain.Entities
{
    public class AvailabilityException : BaseEntity
    {
        public override int Id { get; set; }
        public int DoctorId { get; set; }
        public DateTime StartDate{ get; set; }
        public DateTime EndDate { get; set; }
        public string? Reason { get; set; }
        public ExceptionType ExceptionType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
