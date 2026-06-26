namespace LocalVenico.Web.Data.Entidades;

public class Usuario
{
    public int UsuarioId { get; set; }
    public int RolId { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string NombreUsuario { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    public DateTime CreadoEn { get; set; } = DateTime.Now;
    public DateTime? UltimoAcceso { get; set; }

    public Rol Rol { get; set; } = null!;
    public ICollection<Sesion> Sesiones { get; set; } = [];
    public ICollection<Venta> Ventas { get; set; } = [];
    public ICollection<StockMovimiento> Movimientos { get; set; } = [];
    public ICollection<OrdenCompra> OrdenesCreadas { get; set; } = [];
    public ICollection<OrdenCompra> OrdenesRecibidas { get; set; } = [];
}
