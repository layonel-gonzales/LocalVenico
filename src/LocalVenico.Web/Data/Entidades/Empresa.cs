namespace LocalVenico.Web.Data.Entidades;

public class Empresa
{
    public int EmpresaId { get; set; }
    public string RazonSocial { get; set; } = string.Empty;
    public string Rut { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Comuna { get; set; } = string.Empty;
    public string Region { get; set; } = "Metropolitana";
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string Giro { get; set; } = "Almacén de Abarrotes";
    public string? LogoRuta { get; set; }
    public decimal PorcentajeIva { get; set; } = 19m;
    public bool PreciosConIva { get; set; } = true;
    public string Moneda { get; set; } = "CLP";
    public int NumeroBoleta { get; set; } = 1;
    public DateTime ActualizadoEn { get; set; } = DateTime.Now;
}
