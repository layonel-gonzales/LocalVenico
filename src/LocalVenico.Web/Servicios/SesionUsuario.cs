using LocalVenico.Web.Data;
using LocalVenico.Web.Data.Entidades;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;

namespace LocalVenico.Web.Servicios;

public class SesionUsuario
{
    private const string StorageKey = "lv_uid";

    public bool EstaAutenticado => Usuario is not null;
    public Usuario? Usuario { get; private set; }

    public bool EsAdmin     => Usuario?.Rol?.Nombre == "admin";
    public bool EsCajero    => Usuario?.Rol?.Nombre is "admin" or "cajero";
    public bool EsBodeguero => Usuario?.Rol?.Nombre is "admin" or "bodeguero";

    public event Action? OnCambio;

    public void IniciarSesion(Usuario usuario)
    {
        Usuario = usuario;
        OnCambio?.Invoke();
    }

    public void CerrarSesion()
    {
        Usuario = null;
        OnCambio?.Invoke();
    }

    // Intenta restaurar la sesión desde sessionStorage del navegador
    public async Task<bool> IntentarRestaurarAsync(
        ProtectedSessionStorage storage,
        AppDbContext db)
    {
        if (EstaAutenticado) return true;

        try
        {
            var result = await storage.GetAsync<int>(StorageKey);
            if (!result.Success || result.Value == 0) return false;

            var usuario = await db.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.UsuarioId == result.Value && u.Activo);

            if (usuario is null) return false;

            Usuario = usuario;
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task PersistirAsync(ProtectedSessionStorage storage)
    {
        if (Usuario is not null)
            await storage.SetAsync(StorageKey, Usuario.UsuarioId);
    }

    public async Task LimpiarStorageAsync(ProtectedSessionStorage storage)
    {
        await storage.DeleteAsync(StorageKey);
    }
}
