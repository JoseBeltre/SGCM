namespace SGCM.Application.DTOs.Appointment
{
    public record RemoveAppointmentDto
    {
        public int Id { get; init; }
        public string? CancellationReason { get; init; }
    }
}