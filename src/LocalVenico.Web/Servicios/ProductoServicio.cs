using ClosedXML.Excel;
using LocalVenico.Web.Data;
using LocalVenico.Web.Data.Entidades;
using Microsoft.EntityFrameworkCore;

namespace LocalVenico.Web.Servicios;

public class ProductoServicio(AppDbContext db)
{
    // Genera código interno único: INT-00001, INT-00002, ...
    public async Task<string> GenerarCodigoInternoAsync()
    {
        var ultimo = await db.Productos
            .Where(p => p.CodigoInterno.StartsWith("INT-"))
            .OrderByDescending(p => p.CodigoInterno)
            .Select(p => p.CodigoInterno)
            .FirstOrDefaultAsync();

        int siguiente = 1;
        if (ultimo is not null && int.TryParse(ultimo[4..], out int num))
            siguiente = num + 1;

        return $"INT-{siguiente:D5}";
    }

    // Busca producto por código de barras o código interno (para pistola)
    public async Task<Producto?> BuscarPorCodigoAsync(string codigo)
    {
        codigo = codigo.Trim();
        return await db.Productos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Activo &&
                (p.CodigoBarras == codigo || p.CodigoInterno == codigo));
    }

    // Ajusta stock y registra el movimiento
    public async Task AjustarStockAsync(
        int productoId, decimal cantidad, string tipo, string? motivo,
        int usuarioId, int? ventaId = null, int? ordenCompraId = null)
    {
        var producto = await db.Productos.FindAsync(productoId)
            ?? throw new InvalidOperationException("Producto no encontrado.");

        var stockAntes = producto.StockActual;
        producto.StockActual += cantidad;
        producto.ActualizadoEn = DateTime.Now;

        db.StockMovimientos.Add(new StockMovimiento
        {
            ProductoId    = productoId,
            UsuarioId     = usuarioId,
            VentaId       = ventaId,
            OrdenCompraId = ordenCompraId,
            Tipo          = tipo,
            Cantidad      = cantidad,
            StockAntes    = stockAntes,
            StockDespues  = producto.StockActual,
            Motivo        = motivo
        });

        await db.SaveChangesAsync();
    }

    // Carga masiva desde Excel
    public async Task<(int insertados, int actualizados, List<string> errores)>
        ImportarDesdeExcelAsync(Stream stream, int usuarioId)
    {
        int insertados = 0, actualizados = 0;
        var errores = new List<string>();

        using var wb = new XLWorkbook(stream);
        var ws = wb.Worksheets.First();

        var categorias = await db.Categorias.Where(c => c.Activa).ToListAsync();

        int fila = 2; // fila 1 = encabezado
        while (!ws.Row(fila).IsEmpty())
        {
            try
            {
                var nombre        = ws.Cell(fila, 1).GetString().Trim();
                var codigoBarras  = ws.Cell(fila, 2).GetString().Trim();
                var categoriaNom  = ws.Cell(fila, 3).GetString().Trim();
                var precioCompra  = ws.Cell(fila, 4).GetValue<decimal>();
                var precioVenta   = ws.Cell(fila, 5).GetValue<decimal>();
                var stockInicial  = ws.Cell(fila, 6).GetValue<decimal>();
                var stockMinimo   = ws.Cell(fila, 7).GetValue<decimal>();
                var unidad        = ws.Cell(fila, 8).GetString().Trim();

                if (string.IsNullOrEmpty(nombre)) { fila++; continue; }

                var categoria = categorias.FirstOrDefault(c =>
                    c.Nombre.Equals(categoriaNom, StringComparison.OrdinalIgnoreCase))
                    ?? categorias.First(); // fallback a primera categoría

                // Buscar si ya existe por código de barras
                Producto? existente = null;
                if (!string.IsNullOrEmpty(codigoBarras))
                    existente = await db.Productos.FirstOrDefaultAsync(p => p.CodigoBarras == codigoBarras);

                if (existente is not null)
                {
                    existente.Nombre       = nombre;
                    existente.CategoriaId  = categoria.CategoriaId;
                    existente.PrecioCompra = precioCompra;
                    existente.PrecioVenta  = precioVenta;
                    existente.StockMinimo  = stockMinimo;
                    existente.UnidadMedida = string.IsNullOrEmpty(unidad) ? "UN" : unidad;
                    existente.ActualizadoEn = DateTime.Now;
                    actualizados++;
                }
                else
                {
                    var codigo = string.IsNullOrEmpty(codigoBarras)
                        ? await GenerarCodigoInternoAsync()
                        : codigoBarras;

                    var producto = new Producto
                    {
                        Nombre            = nombre,
                        CodigoBarras      = string.IsNullOrEmpty(codigoBarras) ? null : codigoBarras,
                        CodigoInterno     = string.IsNullOrEmpty(codigoBarras) ? codigo : $"INT-{insertados + actualizados + 1:D5}",
                        CategoriaId       = categoria.CategoriaId,
                        PrecioCompra      = precioCompra,
                        PrecioVenta       = precioVenta,
                        StockActual       = stockInicial,
                        StockMinimo       = stockMinimo,
                        UnidadMedida      = string.IsNullOrEmpty(unidad) ? "UN" : unidad,
                        TieneCodigoPropio = !string.IsNullOrEmpty(codigoBarras)
                    };
                    db.Productos.Add(producto);

                    if (stockInicial > 0)
                    {
                        await db.SaveChangesAsync(); // necesitamos el ProductoId
                        db.StockMovimientos.Add(new StockMovimiento
                        {
                            ProductoId   = producto.ProductoId,
                            UsuarioId    = usuarioId,
                            Tipo         = "ENTRADA",
                            Cantidad     = stockInicial,
                            StockAntes   = 0,
                            StockDespues = stockInicial,
                            Motivo       = "Carga inicial desde Excel"
                        });
                    }
                    insertados++;
                }

                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                errores.Add($"Fila {fila}: {ex.Message}");
            }
            fila++;
        }

        return (insertados, actualizados, errores);
    }

    // Genera plantilla Excel para carga masiva
    public byte[] GenerarPlantillaExcel()
    {
        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Productos");

        var headers = new[] { "Nombre*", "Codigo Barras", "Categoria*", "Precio Compra", "Precio Venta*", "Stock Inicial", "Stock Minimo", "Unidad (UN/KG/LT)" };
        for (int i = 0; i < headers.Length; i++)
        {
            var cell = ws.Cell(1, i + 1);
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1e3a5f");
            cell.Style.Font.FontColor = XLColor.White;
        }

        // Fila ejemplo
        ws.Cell(2, 1).Value = "Leche Entera 1L";
        ws.Cell(2, 2).Value = "7802900005551";
        ws.Cell(2, 3).Value = "Lácteos";
        ws.Cell(2, 4).Value = 800;
        ws.Cell(2, 5).Value = 1200;
        ws.Cell(2, 6).Value = 24;
        ws.Cell(2, 7).Value = 6;
        ws.Cell(2, 8).Value = "UN";

        ws.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }
}
