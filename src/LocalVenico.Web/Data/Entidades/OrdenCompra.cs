namespace LocalVenico.Web.Data.Entidades;

public class OrdenCompra
{
    public int OrdenId { get; set; }
    public int ProveedorId { get; set; }
    public int CreadoPor { get; set; }
    public int? RecibidoPor { get; set; }
    public int NumeroOrden { get; set; }
    public string Estado { get; set; } = "BORRADOR"; // BORRADOR, ENVIADA, RECIBIDA, CANCELADA
    public decimal TotalEstimado { get; set; }
    public string? Notas { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    public DateTime? FechaRecepcion { get; set; }

    public Proveedor Proveedor { get; set; } = null!;
    public Usuario UsuarioCreador { get; set; } = null!;
    public Usuario? UsuarioReceptor { get; set; }
    public ICollection<DetalleOrdenCompra> Detalles { get; set; } = [];
    public ICollection<StockMovimiento> Movimientos { get; set; } = [];
}
