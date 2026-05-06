using CryoTracking.Application.Interfaces;
using CryoTracking.Infrastructure.Persistence;
using CryoTracking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using FluentValidation;
using CryoTracking.Application.Validators;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Servis Kayýtlarý
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<PatientCreateValidator>();

// Veritabaný Baðlantýsý
builder.Services.AddDbContext<CryoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection (Baðýmlýlýk Enjeksiyonu)
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<ISampleRepository, SampleRepository>();
builder.Services.AddScoped<IConsentRepository, ConsentRepository>();
builder.Services.AddScoped<IQualityAssessmentRepository, QualityAssessmentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Enum'larý sayý yerine (0,1,2) isimleriyle ("Frozen", "Embryo") iþler
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Swagger arayüzünde Enum deðerlerinin isim olarak görünmesini saðlar
    c.SchemaFilter<EnumSchemaFilter>();
});
// Uygulama Build Ediliyor
var app = builder.Build();

//  Middleware (Ara Katman) Ayarlarý
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CryoTracking API V1");
        // c.RoutePrefix = string.Empty; // Eðer bunu açarsan direkt localhost:5005'te Swagger açýlýr
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Controller'larý map'le
app.MapControllers();

// Uygulamayý çalýþtýr
app.Run();