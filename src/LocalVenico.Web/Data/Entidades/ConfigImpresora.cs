namespace LocalVenico.Web.Data.Entidades;

public class ConfigImpresora
{
    public int ConfigId { get; set; }
    public string NombreImpresora { get; set; } = string.Empty;
    public string Tipo { get; set; } = "TERMICA_80MM";
    public int AnchoPapelMm { get; set; } = 80;
    public int CaracteresPorLinea { get; set; } = 42;
    public int CopiasImpresion { get; set; } = 1;
    public bool CorteAutomatico { get; set; } = true;
    public string? MensajePieBoleta { get; set; }
    public string? LogoTexto { get; set; }
    public int LineasEncabezado { get; set; } = 3;
    public int LineasPie { get; set; } = 3;
    public bool MostrarLogo { get; set; }
    public bool Activa { get; set; } = true;
    public DateTime ActualizadoEn { get; set; } = DateTime.Now;
}
