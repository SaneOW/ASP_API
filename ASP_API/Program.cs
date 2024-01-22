using ASP_API;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Dienste hinzufügen
builder.Services.AddDbContext<MesseDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// CORS-Konfiguration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policyBuilder => policyBuilder.AllowAnyOrigin()
                                       .AllowAnyMethod()
                                       .AllowAnyHeader());
});

// Weitere Dienste und Konfigurationen...

var app = builder.Build();

// Middleware für CORS vor der Autorisierung verwenden
app.UseCors("AllowAll");


// Middleware-Konfigurationen
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Meine API V1");
        options.RoutePrefix = string.Empty; // Setzt Swagger auf die Root-Seite
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
