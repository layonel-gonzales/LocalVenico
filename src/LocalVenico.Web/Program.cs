using LocalVenico.Web.Components;
using LocalVenico.Web.Data;
using LocalVenico.Web.Servicios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Escuchar en toda la red local (para celulares y otras PCs del almacén)
builder.WebHost.UseUrls("http://0.0.0.0:5000", "https://0.0.0.0:5001");

// ── Base de datos
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("LocalVenico")));

// ── Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ── Servicios propios
builder.Services.AddScoped<IAutenticacionServicio, AutenticacionServicio>();
builder.Services.AddScoped<SesionUsuario>();
builder.Services.AddScoped<ProductoServicio>();
builder.Services.AddScoped<VentaServicio>();

var app = builder.Build();

// ── Aplicar migraciones automáticamente al iniciar
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
    app.UseHttpsRedirection();
}
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
