namespace LocalVenico.Web.Data.Entidades;

public class Sesion
{
    public Guid SesionId { get; set; } = Guid.NewGuid();
    public int UsuarioId { get; set; }
    public string TokenHash { get; set; } = string.Empty;
    public string? IpOrigen { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.Now;
    public DateTime ExpiraEn { get; set; }
    public bool Activa { get; set; } = true;

    public Usuario Usuario { get; set; } = null!;
}
