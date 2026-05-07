using CryoTracking.Application.Interfaces;
using CryoTracking.Infrastructure.Persistence;
using CryoTracking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using FluentValidation;
using CryoTracking.Application.Validators;
using System.Text.Json.Serialization;
using CryoTracking.Domain.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
// 1. JwtSettings'i konfigürasyondan oku
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// 2. Kimlik Doðrulama (Authentication) Ayarlarý
builder.Services.AddAuthentication(options =>
{
options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings!.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
    };
});

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
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Enum'larý sayý yerine (0,1,2) isimleriyle ("Frozen", "Embryo") iþler
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CryoTracking API", Version = "v1" });

    //  Swagger'a JWT kullandýðýmý tanýmla
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
    });

    // Tüm servislerin bu kilit sistemine baðlý
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    //  EnumSchemaFilter burada tut
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
app.UseAuthentication(); //kimlik sor
app.UseAuthorization(); //yetkisizse engelle
app.UseAuthorization();

// Controller'larý map'le
app.MapControllers();

// Uygulamayý çalýþtýr
app.Run();