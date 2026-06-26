namespace LocalVenico.Web.Data.Entidades;

public class DetalleVenta
{
    public int DetalleId { get; set; }
    public int VentaId { get; set; }
    public int ProductoId { get; set; }
    public string NombreProducto { get; set; } = string.Empty;
    public string? CodigoBarras { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal DescuentoItem { get; set; }
    public decimal Subtotal { get; set; }

    public Venta Venta { get; set; } = null!;
    public Producto Producto { get; set; } = null!;
}
