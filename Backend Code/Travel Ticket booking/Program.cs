using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Travel_Ticket_booking.Interface;
using Travel_Ticket_booking.Model;
using Travel_Ticket_booking.Repository;
using Travel_Ticket_booking.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TravelTicketingDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Payoda")));
builder.Services.AddScoped<IAdminDashboardMetrics, AdminDashboardMetricsRepository>();
builder.Services.AddScoped<AdminDashboardMetricsService>();
builder.Services.AddScoped<IAdmin,AdminRepository>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<IAuditLog,AuditLogRepository>();
builder.Services.AddScoped<AuditLogService>();
builder.Services.AddScoped<IBooking,BookingRepository>();
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<IEmailService, SmtpEmailService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {

                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Key"]!)),
                       ValidateIssuer = false,
                       ValidateAudience = false
                   };
               });

builder.Services.AddSwaggerGen(c => {
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
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
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient",
        builder => builder
         .AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader());
});

var app = builder.Build();
app.UseCors("AllowAngularClient");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
