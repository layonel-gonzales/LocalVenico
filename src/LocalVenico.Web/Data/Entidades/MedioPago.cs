namespace LocalVenico.Web.Data.Entidades;

public class MedioPago
{
    public int MedioPagoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool RequiereVuelto { get; set; }
    public bool Activo { get; set; } = true;

    public ICollection<Venta> Ventas { get; set; } = [];
}
