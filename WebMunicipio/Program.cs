using WebMunicipio.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



builder.Services.AddHttpClient<AlertaService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7061/");
});

builder.Services.AddHttpClient<CampaniaService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7061/");
});

builder.Services.AddHttpClient<ConsultaService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7061/");
});

builder.Services.AddHttpClient<FeriaService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7061/");
});

builder.Services.AddHttpClient<PermisoService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7061/");
});

builder.Services.AddHttpClient<ReporteService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7061/");
});


builder.Services.AddHttpClient<ReporteViolenciaService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7061/");
});

builder.Services.AddHttpClient<ReservaService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7061/");
});

builder.Services.AddHttpClient<SituacionCalleService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7061/");
});

builder.Services.AddHttpClient<TallerService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7061/");
});


builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
