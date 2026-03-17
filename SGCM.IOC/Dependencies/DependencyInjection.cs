using Microsoft.Extensions.DependencyInjection;
using SGCM.IOC.Dependencies.Appointment;
using SGCM.IOC.Dependencies.AuditLog;
using SGCM.IOC.Dependencies.Availability;
using SGCM.IOC.Dependencies.Doctor;
using SGCM.IOC.Dependencies.Notification;
using SGCM.IOC.Dependencies.Patient;
using SGCM.IOC.Dependencies.SystemSetting;
using SGCM.IOC.Dependencies.User;

namespace SGCM.IOC.Dependencies
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSGCM(IServiceCollection services)
        {
            services = NotificationDependency.Register(services);
            services = AvailabilityExceptionDependency.Register(services);
            services = AvailabilityDependency.Register(services);
            services = AuditLogDependency.Register(services);
            services = AppointmentDependency.Register(services);
            services = UserDependency.Register(services);
            services = SystemSettingDependency.Register(services);
            services = PatientDependency.Register(services);
            services = DoctorDependency.Register(services);

            return services;
        }
    }
}
