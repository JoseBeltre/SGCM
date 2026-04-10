namespace SGCM.Desktop.Models
{
    public class ReportDto
    {
        public string Title { get; set; } = string.Empty;
        public int Value { get; set; }
        public string DateRange { get; set; } = string.Empty;
    }

    public class AppointmentStatsDto
    {
        public int TotalAppointments { get; set; }
        public int ConfirmedAppointments { get; set; }
        public int CancelledAppointments { get; set; }
        public int CompletedAppointments { get; set; }
    }
}
