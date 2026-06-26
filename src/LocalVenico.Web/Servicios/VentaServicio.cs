using LocalVenico.Web.Data;
using LocalVenico.Web.Data.Entidades;
using Microsoft.EntityFrameworkCore;

namespace LocalVenico.Web.Servicios;

public class VentaServicio(AppDbContext db)
{
    public async Task<Venta> CerrarVentaAsync(
        List<ItemCarrito> items,
        int medioPagoId,
        decimal descuentoGlobal,
        decimal montoPagado,
        int usuarioId,
        decimal porcentajeIva)
    {
        if (!items.Any()) throw new InvalidOperationException("El carrito está vacío.");

        // Calcular totales
        decimal totalConDescuento = items.Sum(i => i.Subtotal) - descuentoGlobal;
        if (totalConDescuento < 0) totalConDescuento = 0;

        decimal subtotalNeto = Math.Round(totalConDescuento / (1 + porcentajeIva / 100), 2);
        decimal montoIva     = totalConDescuento - subtotalNeto;
        decimal vuelto       = montoPagado - totalConDescuento;
        if (vuelto < 0) vuelto = 0;

        // Número de boleta correlativo
        var ultimoBoleta = await db.Ventas.MaxAsync(v => (int?)v.NumeroBoleta) ?? 0;

        var venta = new Venta
        {
            UsuarioId      = usuarioId,
            MedioPagoId    = medioPagoId,
            NumeroBoleta   = ultimoBoleta + 1,
            SubtotalNeto   = subtotalNeto,
            MontoIva       = montoIva,
            DescuentoTotal = descuentoGlobal,
            Total          = totalConDescuento,
            MontoPagado    = montoPagado,
            Vuelto         = vuelto,
            Estado         = "COMPLETADA",
            FechaVenta     = DateTime.Now
        };

        db.Ventas.Add(venta);
        await db.SaveChangesAsync(); // obtener VentaId

        foreach (var item in items)
        {
            var producto = await db.Productos.FindAsync(item.ProductoId)
                ?? throw new InvalidOperationException($"Producto {item.NombreProducto} no encontrado.");

            if (producto.StockActual < item.Cantidad)
                throw new InvalidOperationException($"Stock insuficiente para '{producto.Nombre}'. Stock: {producto.StockActual}.");

            db.DetallesVenta.Add(new DetalleVenta
            {
                VentaId        = venta.VentaId,
                ProductoId     = item.ProductoId,
                NombreProducto = item.NombreProducto,
                CodigoBarras   = item.CodigoBarras,
                Cantidad       = item.Cantidad,
                PrecioUnitario = item.PrecioUnitario,
                DescuentoItem  = item.DescuentoItem,
                Subtotal       = item.Subtotal
            });

            var stockAntes = producto.StockActual;
            producto.StockActual -= item.Cantidad;
            producto.ActualizadoEn = DateTime.Now;

            db.StockMovimientos.Add(new StockMovimiento
            {
                ProductoId   = item.ProductoId,
                UsuarioId    = usuarioId,
                VentaId      = venta.VentaId,
                Tipo         = "VENTA",
                Cantidad     = -item.Cantidad,
                StockAntes   = stockAntes,
                StockDespues = producto.StockActual,
                Motivo       = $"Venta boleta #{venta.NumeroBoleta}"
            });
        }

        await db.SaveChangesAsync();
        return venta;
    }

    public async Task AnularVentaAsync(int ventaId, string motivo, int usuarioId)
    {
        var venta = await db.Ventas
            .Include(v => v.Detalles)
            .FirstOrDefaultAsync(v => v.VentaId == ventaId)
            ?? throw new InvalidOperationException("Venta no encontrada.");

        if (venta.Estado == "ANULADA")
            throw new InvalidOperationException("La venta ya está anulada.");

        venta.Estado          = "ANULADA";
        venta.MotivoAnulacion = motivo;
        venta.AnuladaPor      = usuarioId;
        venta.FechaAnulacion  = DateTime.Now;

        // Reponer stock
        foreach (var detalle in venta.Detalles)
        {
            var producto = await db.Productos.FindAsync(detalle.ProductoId);
            if (producto is null) continue;

            var stockAntes = producto.StockActual;
            producto.StockActual += detalle.Cantidad;
            producto.ActualizadoEn = DateTime.Now;

            db.StockMovimientos.Add(new StockMovimiento
            {
                ProductoId   = detalle.ProductoId,
                UsuarioId    = usuarioId,
                VentaId      = ventaId,
                Tipo         = "ANULACION",
                Cantidad     = detalle.Cantidad,
                StockAntes   = stockAntes,
                StockDespues = producto.StockActual,
                Motivo       = $"Anulación boleta #{venta.NumeroBoleta}: {motivo}"
            });
        }

        await db.SaveChangesAsync();
    }
}

public class ItemCarrito
{
    public int     ProductoId     { get; set; }
    public string  NombreProducto { get; set; } = string.Empty;
    public string? CodigoBarras   { get; set; }
    public decimal Cantidad       { get; set; } = 1;
    public decimal PrecioUnitario { get; set; }
    public decimal DescuentoItem  { get; set; }
    public decimal Subtotal       => Math.Round((PrecioUnitario * Cantidad) - DescuentoItem, 0);
}
