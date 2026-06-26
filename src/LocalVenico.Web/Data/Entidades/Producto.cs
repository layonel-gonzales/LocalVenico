namespace LocalVenico.Web.Data.Entidades;

public class Producto
{
    public int ProductoId { get; set; }
    public int CategoriaId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? CodigoBarras { get; set; }
    public string CodigoInterno { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string UnidadMedida { get; set; } = "UN";
    public decimal PrecioCompra { get; set; }
    public decimal PrecioVenta { get; set; }
    public decimal StockActual { get; set; }
    public decimal StockMinimo { get; set; }
    public bool TieneCodigoPropio { get; set; }
    public string? ImagenUrl { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime CreadoEn { get; set; } = DateTime.Now;
    public DateTime ActualizadoEn { get; set; } = DateTime.Now;

    public Categoria Categoria { get; set; } = null!;
    public ICollection<DetalleVenta> DetallesVenta { get; set; } = [];
    public ICollection<StockMovimiento> Movimientos { get; set; } = [];
    public ICollection<DetalleOrdenCompra> DetallesOrden { get; set; } = [];

    public bool StockBajo => StockActual < StockMinimo;
}
