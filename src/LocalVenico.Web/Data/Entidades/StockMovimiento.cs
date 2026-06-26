namespace LocalVenico.Web.Data.Entidades;

public class StockMovimiento
{
    public int MovimientoId { get; set; }
    public int ProductoId { get; set; }
    public int UsuarioId { get; set; }
    public int? VentaId { get; set; }
    public int? OrdenCompraId { get; set; }
    public string Tipo { get; set; } = string.Empty; // ENTRADA, SALIDA, AJUSTE, VENTA, ANULACION, COMPRA
    public decimal Cantidad { get; set; }
    public decimal StockAntes { get; set; }
    public decimal StockDespues { get; set; }
    public string? Motivo { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.Now;

    public Producto Producto { get; set; } = null!;
    public Usuario Usuario { get; set; } = null!;
    public Venta? Venta { get; set; }
    public OrdenCompra? OrdenCompra { get; set; }
}
