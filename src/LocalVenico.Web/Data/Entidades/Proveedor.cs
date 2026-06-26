namespace LocalVenico.Web.Data.Entidades;

public class Proveedor
{
    public int ProveedorId { get; set; }
    public string RazonSocial { get; set; } = string.Empty;
    public string Rut { get; set; } = string.Empty;
    public string? NombreContacto { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Direccion { get; set; }
    public string? Notas { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime CreadoEn { get; set; } = DateTime.Now;

    public ICollection<OrdenCompra> Ordenes { get; set; } = [];
}
