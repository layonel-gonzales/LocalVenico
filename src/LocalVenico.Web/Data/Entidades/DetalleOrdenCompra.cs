namespace LocalVenico.Web.Data.Entidades;

public class DetalleOrdenCompra
{
    public int DetalleOrdenId { get; set; }
    public int OrdenId { get; set; }
    public int ProductoId { get; set; }
    public decimal CantidadPedida { get; set; }
    public decimal CantidadRecibida { get; set; }
    public decimal PrecioUnitarioCompra { get; set; }
    public decimal Subtotal { get; set; }

    public OrdenCompra OrdenCompra { get; set; } = null!;
    public Producto Producto { get; set; } = null!;
}
