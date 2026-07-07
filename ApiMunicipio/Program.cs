using ApiMunicipio.Data;
using ApiMunicipio.Models;
using ApiMunicipio.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MunicipioContext>((serviceProvider, options) =>
{
    // LocalDB cambia su canalización cuando se reinicia. Resolverla por cada
    // alcance evita que la API conserve una ruta antigua y falle después.
    var connectionString = LocalDbConnectionResolver.Resolve(builder.Configuration);
    options.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure());
});

builder.Services
    .AddIdentity<Usuario, IdentityRole>(options =>
    {
        // Reglas apropiadas para el prototipo académico y consistentes con el formulario MAUI.
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<MunicipioContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MobileApp", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseCors("MobileApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    await DatabaseSeeder.InitializeAsync(scope.ServiceProvider);
}

app.Run();
