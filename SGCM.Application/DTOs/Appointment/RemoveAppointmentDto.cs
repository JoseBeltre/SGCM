namespace SGCM.Application.DTOs.Appointment
{
    public record RemoveAppointmentDto
    {
        public required int Id { get; init; }
        public required string CancellationReason { get; init; }
    }
}