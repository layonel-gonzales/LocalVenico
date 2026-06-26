namespace LocalVenico.Web.Data.Entidades;

public class Venta
{
    public int VentaId { get; set; }
    public int UsuarioId { get; set; }
    public int MedioPagoId { get; set; }
    public int NumeroBoleta { get; set; }
    public decimal SubtotalNeto { get; set; }
    public decimal MontoIva { get; set; }
    public decimal DescuentoTotal { get; set; }
    public decimal Total { get; set; }
    public decimal MontoPagado { get; set; }
    public decimal Vuelto { get; set; }
    public string Estado { get; set; } = "COMPLETADA";
    public string? MotivoAnulacion { get; set; }
    public int? AnuladaPor { get; set; }
    public DateTime? FechaAnulacion { get; set; }
    public DateTime FechaVenta { get; set; } = DateTime.Now;

    public Usuario Usuario { get; set; } = null!;
    public MedioPago MedioPago { get; set; } = null!;
    public Usuario? UsuarioAnulacion { get; set; }
    public ICollection<DetalleVenta> Detalles { get; set; } = [];
    public ICollection<StockMovimiento> Movimientos { get; set; } = [];
}
