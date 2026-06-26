using LocalVenico.Web.Data.Entidades;

namespace LocalVenico.Web.Servicios;

public interface IAutenticacionServicio
{
    Task<Usuario?> LoginAsync(string nombreUsuario, string password);
    Task<bool> CambiarPasswordAsync(int usuarioId, string passwordActual, string passwordNuevo);
    string HashPassword(string password);
    bool VerificarPassword(string password, string hash);
}
