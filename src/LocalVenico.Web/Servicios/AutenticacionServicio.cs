using LocalVenico.Web.Data;
using LocalVenico.Web.Data.Entidades;
using Microsoft.EntityFrameworkCore;

namespace LocalVenico.Web.Servicios;

public class AutenticacionServicio(AppDbContext db) : IAutenticacionServicio
{
    public async Task<Usuario?> LoginAsync(string nombreUsuario, string password)
    {
        var usuario = await db.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario && u.Activo);

        if (usuario is null || !VerificarPassword(password, usuario.PasswordHash))
            return null;

        usuario.UltimoAcceso = DateTime.Now;
        await db.SaveChangesAsync();
        return usuario;
    }

    public async Task<bool> CambiarPasswordAsync(int usuarioId, string passwordActual, string passwordNuevo)
    {
        var usuario = await db.Usuarios.FindAsync(usuarioId);
        if (usuario is null || !VerificarPassword(passwordActual, usuario.PasswordHash))
            return false;

        usuario.PasswordHash = HashPassword(passwordNuevo);
        await db.SaveChangesAsync();
        return true;
    }

    public string HashPassword(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);

    public bool VerificarPassword(string password, string hash) =>
        BCrypt.Net.BCrypt.Verify(password, hash);
}
