using Microsoft.EntityFrameworkCore;
using SGCM.IOC.Dependencies;
using SGCM.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSGCM();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "SGCM Desktop API",
        Version = "v1"
    });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Seed missing User ID 1 for database trigger "tr_Appointments_TrackChanges"
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SGCM.Persistence.Context.AppDbContext>();
    var user1Exists = await db.Users.AnyAsync(u => u.Id == 1);
    if (!user1Exists)
    {
        await db.Database.ExecuteSqlRawAsync(
            "SET IDENTITY_INSERT Users ON; " +
            "INSERT INTO dbo.Users (UserId, FullName, Email, PasswordHash, UserType, IsActive, CreatedAt) " +
            "VALUES (1, 'System Auto', 'system@auto.local', 'none', 'Administrador', 1, GETDATE()); " +
            "SET IDENTITY_INSERT Users OFF;");
    }
}

app.Run();