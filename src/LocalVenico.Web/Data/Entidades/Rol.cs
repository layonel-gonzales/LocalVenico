namespace LocalVenico.Web.Data.Entidades;

public class Rol
{
    public int RolId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime CreadoEn { get; set; } = DateTime.Now;

    public ICollection<Usuario> Usuarios { get; set; } = [];
}
