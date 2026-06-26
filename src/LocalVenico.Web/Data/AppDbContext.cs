using LocalVenico.Web.Data.Entidades;
using Microsoft.EntityFrameworkCore;

namespace LocalVenico.Web.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Sesion> Sesiones => Set<Sesion>();
    public DbSet<Empresa> Empresa => Set<Empresa>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Proveedor> Proveedores => Set<Proveedor>();
    public DbSet<MedioPago> MediosPago => Set<MedioPago>();
    public DbSet<Venta> Ventas => Set<Venta>();
    public DbSet<DetalleVenta> DetallesVenta => Set<DetalleVenta>();
    public DbSet<StockMovimiento> StockMovimientos => Set<StockMovimiento>();
    public DbSet<OrdenCompra> OrdenesCompra => Set<OrdenCompra>();
    public DbSet<DetalleOrdenCompra> DetallesOrdenCompra => Set<DetalleOrdenCompra>();
    public DbSet<ConfigImpresora> ConfigImpresora => Set<ConfigImpresora>();

    protected override void OnModelCreating(ModelBuilder model)
    {
        // Roles
        model.Entity<Rol>(e =>
        {
            e.ToTable("roles");
            e.HasKey(x => x.RolId);
            e.Property(x => x.RolId).HasColumnName("rol_id");
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(50).IsRequired();
            e.Property(x => x.Descripcion).HasColumnName("descripcion").HasMaxLength(200);
            e.Property(x => x.Activo).HasColumnName("activo").HasDefaultValue(true);
            e.Property(x => x.CreadoEn).HasColumnName("creado_en").HasDefaultValueSql("GETDATE()");
            e.HasIndex(x => x.Nombre).IsUnique();
        });

        // Usuarios
        model.Entity<Usuario>(e =>
        {
            e.ToTable("usuarios");
            e.HasKey(x => x.UsuarioId);
            e.Property(x => x.UsuarioId).HasColumnName("usuario_id");
            e.Property(x => x.RolId).HasColumnName("rol_id");
            e.Property(x => x.NombreCompleto).HasColumnName("nombre_completo").HasMaxLength(100).IsRequired();
            e.Property(x => x.NombreUsuario).HasColumnName("nombre_usuario").HasMaxLength(50).IsRequired();
            e.Property(x => x.PasswordHash).HasColumnName("password_hash").HasMaxLength(256).IsRequired();
            e.Property(x => x.Activo).HasColumnName("activo").HasDefaultValue(true);
            e.Property(x => x.CreadoEn).HasColumnName("creado_en").HasDefaultValueSql("GETDATE()");
            e.Property(x => x.UltimoAcceso).HasColumnName("ultimo_acceso");
            e.HasIndex(x => x.NombreUsuario).IsUnique();
            e.HasOne(x => x.Rol).WithMany(r => r.Usuarios).HasForeignKey(x => x.RolId);
        });

        // Sesiones
        model.Entity<Sesion>(e =>
        {
            e.ToTable("sesiones");
            e.HasKey(x => x.SesionId);
            e.Property(x => x.SesionId).HasColumnName("sesion_id").HasDefaultValueSql("NEWID()");
            e.Property(x => x.UsuarioId).HasColumnName("usuario_id");
            e.Property(x => x.TokenHash).HasColumnName("token_hash").HasMaxLength(256).IsRequired();
            e.Property(x => x.IpOrigen).HasColumnName("ip_origen").HasMaxLength(45);
            e.Property(x => x.CreadoEn).HasColumnName("creado_en").HasDefaultValueSql("GETDATE()");
            e.Property(x => x.ExpiraEn).HasColumnName("expira_en");
            e.Property(x => x.Activa).HasColumnName("activa").HasDefaultValue(true);
            e.HasOne(x => x.Usuario).WithMany(u => u.Sesiones).HasForeignKey(x => x.UsuarioId);
        });

        // Empresa
        model.Entity<Empresa>(e =>
        {
            e.ToTable("empresa");
            e.HasKey(x => x.EmpresaId);
            e.Property(x => x.EmpresaId).HasColumnName("empresa_id");
            e.Property(x => x.RazonSocial).HasColumnName("razon_social").HasMaxLength(150).IsRequired();
            e.Property(x => x.Rut).HasColumnName("rut").HasMaxLength(12).IsRequired();
            e.Property(x => x.Direccion).HasColumnName("direccion").HasMaxLength(200).IsRequired();
            e.Property(x => x.Comuna).HasColumnName("comuna").HasMaxLength(100).IsRequired();
            e.Property(x => x.Region).HasColumnName("region").HasMaxLength(100).HasDefaultValue("Metropolitana");
            e.Property(x => x.Telefono).HasColumnName("telefono").HasMaxLength(20);
            e.Property(x => x.Email).HasColumnName("email").HasMaxLength(150);
            e.Property(x => x.Giro).HasColumnName("giro").HasMaxLength(150).HasDefaultValue("Almacén de Abarrotes");
            e.Property(x => x.LogoRuta).HasColumnName("logo_ruta").HasMaxLength(500);
            e.Property(x => x.PorcentajeIva).HasColumnName("porcentaje_iva").HasPrecision(5, 2).HasDefaultValue(19m);
            e.Property(x => x.PreciosConIva).HasColumnName("precios_con_iva").HasDefaultValue(true);
            e.Property(x => x.Moneda).HasColumnName("moneda").HasMaxLength(5).HasDefaultValue("CLP");
            e.Property(x => x.NumeroBoleta).HasColumnName("numero_boleta_inicio").HasDefaultValue(1);
            e.Property(x => x.ActualizadoEn).HasColumnName("actualizado_en").HasDefaultValueSql("GETDATE()");
        });

        // Categorias
        model.Entity<Categoria>(e =>
        {
            e.ToTable("categorias");
            e.HasKey(x => x.CategoriaId);
            e.Property(x => x.CategoriaId).HasColumnName("categoria_id");
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
            e.Property(x => x.Descripcion).HasColumnName("descripcion").HasMaxLength(300);
            e.Property(x => x.Activa).HasColumnName("activa").HasDefaultValue(true);
            e.Property(x => x.CreadoEn).HasColumnName("creado_en").HasDefaultValueSql("GETDATE()");
            e.HasIndex(x => x.Nombre).IsUnique();
        });

        // Productos
        model.Entity<Producto>(e =>
        {
            e.ToTable("productos");
            e.HasKey(x => x.ProductoId);
            e.Property(x => x.ProductoId).HasColumnName("producto_id");
            e.Property(x => x.CategoriaId).HasColumnName("categoria_id");
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(150).IsRequired();
            e.Property(x => x.CodigoBarras).HasColumnName("codigo_barras").HasMaxLength(30);
            e.Property(x => x.CodigoInterno).HasColumnName("codigo_interno").HasMaxLength(20).IsRequired();
            e.Property(x => x.Descripcion).HasColumnName("descripcion").HasMaxLength(500);
            e.Property(x => x.UnidadMedida).HasColumnName("unidad_medida").HasMaxLength(20).HasDefaultValue("UN");
            e.Property(x => x.PrecioCompra).HasColumnName("precio_compra").HasPrecision(12, 2).HasDefaultValue(0m);
            e.Property(x => x.PrecioVenta).HasColumnName("precio_venta").HasPrecision(12, 2).IsRequired();
            e.Property(x => x.StockActual).HasColumnName("stock_actual").HasPrecision(12, 3).HasDefaultValue(0m);
            e.Property(x => x.StockMinimo).HasColumnName("stock_minimo").HasPrecision(12, 3).HasDefaultValue(0m);
            e.Property(x => x.TieneCodigoPropio).HasColumnName("tiene_codigo_propio").HasDefaultValue(false);
            e.Property(x => x.ImagenUrl).HasColumnName("imagen_url").HasMaxLength(500);
            e.Property(x => x.Activo).HasColumnName("activo").HasDefaultValue(true);
            e.Property(x => x.CreadoEn).HasColumnName("creado_en").HasDefaultValueSql("GETDATE()");
            e.Property(x => x.ActualizadoEn).HasColumnName("actualizado_en").HasDefaultValueSql("GETDATE()");
            e.Ignore(x => x.StockBajo);
            e.HasIndex(x => x.CodigoBarras).IsUnique().HasFilter("[codigo_barras] IS NOT NULL");
            e.HasIndex(x => x.CodigoInterno).IsUnique();
            e.HasIndex(x => x.Nombre);
            e.HasOne(x => x.Categoria).WithMany(c => c.Productos).HasForeignKey(x => x.CategoriaId);
        });

        // Proveedores
        model.Entity<Proveedor>(e =>
        {
            e.ToTable("proveedores");
            e.HasKey(x => x.ProveedorId);
            e.Property(x => x.ProveedorId).HasColumnName("proveedor_id");
            e.Property(x => x.RazonSocial).HasColumnName("razon_social").HasMaxLength(150).IsRequired();
            e.Property(x => x.Rut).HasColumnName("rut").HasMaxLength(12).IsRequired();
            e.Property(x => x.NombreContacto).HasColumnName("nombre_contacto").HasMaxLength(100);
            e.Property(x => x.Telefono).HasColumnName("telefono").HasMaxLength(20);
            e.Property(x => x.Email).HasColumnName("email").HasMaxLength(150);
            e.Property(x => x.Direccion).HasColumnName("direccion").HasMaxLength(200);
            e.Property(x => x.Notas).HasColumnName("notas").HasMaxLength(500);
            e.Property(x => x.Activo).HasColumnName("activo").HasDefaultValue(true);
            e.Property(x => x.CreadoEn).HasColumnName("creado_en").HasDefaultValueSql("GETDATE()");
        });

        // MediosPago
        model.Entity<MedioPago>(e =>
        {
            e.ToTable("medios_pago");
            e.HasKey(x => x.MedioPagoId);
            e.Property(x => x.MedioPagoId).HasColumnName("medio_pago_id");
            e.Property(x => x.Nombre).HasColumnName("nombre").HasMaxLength(50).IsRequired();
            e.Property(x => x.RequiereVuelto).HasColumnName("requiere_vuelto").HasDefaultValue(false);
            e.Property(x => x.Activo).HasColumnName("activo").HasDefaultValue(true);
            e.HasIndex(x => x.Nombre).IsUnique();
        });

        // Ventas
        model.Entity<Venta>(e =>
        {
            e.ToTable("ventas");
            e.HasKey(x => x.VentaId);
            e.Property(x => x.VentaId).HasColumnName("venta_id");
            e.Property(x => x.UsuarioId).HasColumnName("usuario_id");
            e.Property(x => x.MedioPagoId).HasColumnName("medio_pago_id");
            e.Property(x => x.NumeroBoleta).HasColumnName("numero_boleta");
            e.Property(x => x.SubtotalNeto).HasColumnName("subtotal_neto").HasPrecision(12, 2);
            e.Property(x => x.MontoIva).HasColumnName("monto_iva").HasPrecision(12, 2);
            e.Property(x => x.DescuentoTotal).HasColumnName("descuento_total").HasPrecision(12, 2).HasDefaultValue(0m);
            e.Property(x => x.Total).HasColumnName("total").HasPrecision(12, 2);
            e.Property(x => x.MontoPagado).HasColumnName("monto_pagado").HasPrecision(12, 2);
            e.Property(x => x.Vuelto).HasColumnName("vuelto").HasPrecision(12, 2).HasDefaultValue(0m);
            e.Property(x => x.Estado).HasColumnName("estado").HasMaxLength(20).HasDefaultValue("COMPLETADA");
            e.Property(x => x.MotivoAnulacion).HasColumnName("motivo_anulacion").HasMaxLength(300);
            e.Property(x => x.AnuladaPor).HasColumnName("anulada_por");
            e.Property(x => x.FechaAnulacion).HasColumnName("fecha_anulacion");
            e.Property(x => x.FechaVenta).HasColumnName("fecha_venta").HasDefaultValueSql("GETDATE()");
            e.HasOne(x => x.Usuario).WithMany(u => u.Ventas).HasForeignKey(x => x.UsuarioId);
            e.HasOne(x => x.MedioPago).WithMany(m => m.Ventas).HasForeignKey(x => x.MedioPagoId);
            e.HasOne(x => x.UsuarioAnulacion).WithMany().HasForeignKey(x => x.AnuladaPor);
            e.HasIndex(x => x.FechaVenta);
        });

        // DetalleVenta
        model.Entity<DetalleVenta>(e =>
        {
            e.ToTable("detalle_venta");
            e.HasKey(x => x.DetalleId);
            e.Property(x => x.DetalleId).HasColumnName("detalle_id");
            e.Property(x => x.VentaId).HasColumnName("venta_id");
            e.Property(x => x.ProductoId).HasColumnName("producto_id");
            e.Property(x => x.NombreProducto).HasColumnName("nombre_producto").HasMaxLength(150).IsRequired();
            e.Property(x => x.CodigoBarras).HasColumnName("codigo_barras").HasMaxLength(30);
            e.Property(x => x.Cantidad).HasColumnName("cantidad").HasPrecision(12, 3);
            e.Property(x => x.PrecioUnitario).HasColumnName("precio_unitario").HasPrecision(12, 2);
            e.Property(x => x.DescuentoItem).HasColumnName("descuento_item").HasPrecision(12, 2).HasDefaultValue(0m);
            e.Property(x => x.Subtotal).HasColumnName("subtotal").HasPrecision(12, 2);
            e.HasOne(x => x.Venta).WithMany(v => v.Detalles).HasForeignKey(x => x.VentaId);
            e.HasOne(x => x.Producto).WithMany(p => p.DetallesVenta).HasForeignKey(x => x.ProductoId);
        });

        // StockMovimientos
        model.Entity<StockMovimiento>(e =>
        {
            e.ToTable("stock_movimientos");
            e.HasKey(x => x.MovimientoId);
            e.Property(x => x.MovimientoId).HasColumnName("movimiento_id");
            e.Property(x => x.ProductoId).HasColumnName("producto_id");
            e.Property(x => x.UsuarioId).HasColumnName("usuario_id");
            e.Property(x => x.VentaId).HasColumnName("venta_id");
            e.Property(x => x.OrdenCompraId).HasColumnName("orden_compra_id");
            e.Property(x => x.Tipo).HasColumnName("tipo").HasMaxLength(20).IsRequired();
            e.Property(x => x.Cantidad).HasColumnName("cantidad").HasPrecision(12, 3);
            e.Property(x => x.StockAntes).HasColumnName("stock_antes").HasPrecision(12, 3);
            e.Property(x => x.StockDespues).HasColumnName("stock_despues").HasPrecision(12, 3);
            e.Property(x => x.Motivo).HasColumnName("motivo").HasMaxLength(300);
            e.Property(x => x.CreadoEn).HasColumnName("creado_en").HasDefaultValueSql("GETDATE()");
            e.HasOne(x => x.Producto).WithMany(p => p.Movimientos).HasForeignKey(x => x.ProductoId);
            e.HasOne(x => x.Usuario).WithMany(u => u.Movimientos).HasForeignKey(x => x.UsuarioId);
            e.HasOne(x => x.Venta).WithMany(v => v.Movimientos).HasForeignKey(x => x.VentaId);
            e.HasOne(x => x.OrdenCompra).WithMany(o => o.Movimientos).HasForeignKey(x => x.OrdenCompraId);
        });

        // OrdenesCompra
        model.Entity<OrdenCompra>(e =>
        {
            e.ToTable("ordenes_compra");
            e.HasKey(x => x.OrdenId);
            e.Property(x => x.OrdenId).HasColumnName("orden_id");
            e.Property(x => x.ProveedorId).HasColumnName("proveedor_id");
            e.Property(x => x.CreadoPor).HasColumnName("creado_por");
            e.Property(x => x.RecibidoPor).HasColumnName("recibido_por");
            e.Property(x => x.NumeroOrden).HasColumnName("numero_orden");
            e.Property(x => x.Estado).HasColumnName("estado").HasMaxLength(20).HasDefaultValue("BORRADOR");
            e.Property(x => x.TotalEstimado).HasColumnName("total_estimado").HasPrecision(12, 2).HasDefaultValue(0m);
            e.Property(x => x.Notas).HasColumnName("notas").HasMaxLength(500);
            e.Property(x => x.FechaCreacion).HasColumnName("fecha_creacion").HasDefaultValueSql("GETDATE()");
            e.Property(x => x.FechaRecepcion).HasColumnName("fecha_recepcion");
            e.HasOne(x => x.Proveedor).WithMany(p => p.Ordenes).HasForeignKey(x => x.ProveedorId);
            e.HasOne(x => x.UsuarioCreador).WithMany(u => u.OrdenesCreadas).HasForeignKey(x => x.CreadoPor);
            e.HasOne(x => x.UsuarioReceptor).WithMany(u => u.OrdenesRecibidas).HasForeignKey(x => x.RecibidoPor);
        });

        // DetallesOrdenCompra
        model.Entity<DetalleOrdenCompra>(e =>
        {
            e.ToTable("detalle_orden_compra");
            e.HasKey(x => x.DetalleOrdenId);
            e.Property(x => x.DetalleOrdenId).HasColumnName("detalle_orden_id");
            e.Property(x => x.OrdenId).HasColumnName("orden_id");
            e.Property(x => x.ProductoId).HasColumnName("producto_id");
            e.Property(x => x.CantidadPedida).HasColumnName("cantidad_pedida").HasPrecision(12, 3);
            e.Property(x => x.CantidadRecibida).HasColumnName("cantidad_recibida").HasPrecision(12, 3).HasDefaultValue(0m);
            e.Property(x => x.PrecioUnitarioCompra).HasColumnName("precio_unitario_compra").HasPrecision(12, 2);
            e.Property(x => x.Subtotal).HasColumnName("subtotal").HasPrecision(12, 2);
            e.HasOne(x => x.OrdenCompra).WithMany(o => o.Detalles).HasForeignKey(x => x.OrdenId);
            e.HasOne(x => x.Producto).WithMany(p => p.DetallesOrden).HasForeignKey(x => x.ProductoId);
        });

        // ConfigImpresora
        model.Entity<ConfigImpresora>(e =>
        {
            e.ToTable("config_impresora");
            e.HasKey(x => x.ConfigId);
            e.Property(x => x.ConfigId).HasColumnName("config_id");
            e.Property(x => x.NombreImpresora).HasColumnName("nombre_impresora").HasMaxLength(200).IsRequired();
            e.Property(x => x.Tipo).HasColumnName("tipo").HasMaxLength(20).HasDefaultValue("TERMICA_80MM");
            e.Property(x => x.LineasEncabezado).HasColumnName("lineas_encabezado").HasDefaultValue(3);
            e.Property(x => x.LineasPie).HasColumnName("lineas_pie").HasDefaultValue(3);
            e.Property(x => x.MostrarLogo).HasColumnName("mostrar_logo").HasDefaultValue(false);
            e.Property(x => x.Activa).HasColumnName("activa").HasDefaultValue(true);
            e.Property(x => x.ActualizadoEn).HasColumnName("actualizado_en").HasDefaultValueSql("GETDATE()");
        });

        // Seed data
        model.Entity<Rol>().HasData(
            new Rol { RolId = 1, Nombre = "admin",      Descripcion = "Acceso total al sistema",                     CreadoEn = new DateTime(2026, 1, 1) },
            new Rol { RolId = 2, Nombre = "cajero",     Descripcion = "Acceso al punto de venta e inventario básico", CreadoEn = new DateTime(2026, 1, 1) },
            new Rol { RolId = 3, Nombre = "bodeguero",  Descripcion = "Acceso a inventario, stock y recepciones",     CreadoEn = new DateTime(2026, 1, 1) }
        );

        model.Entity<MedioPago>().HasData(
            new MedioPago { MedioPagoId = 1, Nombre = "Efectivo",       RequiereVuelto = true  },
            new MedioPago { MedioPagoId = 2, Nombre = "Débito",         RequiereVuelto = false },
            new MedioPago { MedioPagoId = 3, Nombre = "Crédito",        RequiereVuelto = false },
            new MedioPago { MedioPagoId = 4, Nombre = "Transferencia",  RequiereVuelto = false }
        );

        model.Entity<Categoria>().HasData(
            new Categoria { CategoriaId = 1,  Nombre = "Abarrotes",               CreadoEn = new DateTime(2026, 1, 1) },
            new Categoria { CategoriaId = 2,  Nombre = "Bebestibles",             CreadoEn = new DateTime(2026, 1, 1) },
            new Categoria { CategoriaId = 3,  Nombre = "Lácteos",                 CreadoEn = new DateTime(2026, 1, 1) },
            new Categoria { CategoriaId = 4,  Nombre = "Panadería y Pastelería",  CreadoEn = new DateTime(2026, 1, 1) },
            new Categoria { CategoriaId = 5,  Nombre = "Frutas y Verduras",       CreadoEn = new DateTime(2026, 1, 1) },
            new Categoria { CategoriaId = 6,  Nombre = "Carnes y Embutidos",      CreadoEn = new DateTime(2026, 1, 1) },
            new Categoria { CategoriaId = 7,  Nombre = "Limpieza y Hogar",        CreadoEn = new DateTime(2026, 1, 1) },
            new Categoria { CategoriaId = 8,  Nombre = "Higiene Personal",        CreadoEn = new DateTime(2026, 1, 1) },
            new Categoria { CategoriaId = 9,  Nombre = "Snacks y Confites",       CreadoEn = new DateTime(2026, 1, 1) },
            new Categoria { CategoriaId = 10, Nombre = "Congelados",              CreadoEn = new DateTime(2026, 1, 1) },
            new Categoria { CategoriaId = 11, Nombre = "Otros",                   CreadoEn = new DateTime(2026, 1, 1) }
        );

        // Usuario admin inicial — password: Admin1234
        model.Entity<Usuario>().HasData(new Usuario
        {
            UsuarioId     = 1,
            RolId         = 1,
            NombreCompleto = "Administrador",
            NombreUsuario  = "admin",
            PasswordHash   = "$2b$12$w7U5s1ltCJlrCTawN.BLg.VDFKd3Ag8cXD03RrmdSVYlTTC5/bk5O",
            Activo         = true,
            CreadoEn       = new DateTime(2026, 1, 1)
        });
    }
}
