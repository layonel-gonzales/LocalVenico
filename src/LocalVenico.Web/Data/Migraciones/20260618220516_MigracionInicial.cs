using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LocalVenico.Web.Data.Migraciones
{
    /// <inheritdoc />
    public partial class MigracionInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categorias",
                columns: table => new
                {
                    categoria_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    activa = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    creado_en = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categorias", x => x.categoria_id);
                });

            migrationBuilder.CreateTable(
                name: "config_impresora",
                columns: table => new
                {
                    config_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre_impresora = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "TERMICA_80MM"),
                    lineas_encabezado = table.Column<int>(type: "int", nullable: false, defaultValue: 3),
                    lineas_pie = table.Column<int>(type: "int", nullable: false, defaultValue: 3),
                    mostrar_logo = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    activa = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    actualizado_en = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_config_impresora", x => x.config_id);
                });

            migrationBuilder.CreateTable(
                name: "empresa",
                columns: table => new
                {
                    empresa_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    razon_social = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    rut = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    direccion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    comuna = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    region = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "Metropolitana"),
                    telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    giro = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, defaultValue: "Almacén de Abarrotes"),
                    logo_ruta = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    porcentaje_iva = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false, defaultValue: 19m),
                    precios_con_iva = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    moneda = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, defaultValue: "CLP"),
                    numero_boleta_inicio = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    actualizado_en = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_empresa", x => x.empresa_id);
                });

            migrationBuilder.CreateTable(
                name: "medios_pago",
                columns: table => new
                {
                    medio_pago_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    requiere_vuelto = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_medios_pago", x => x.medio_pago_id);
                });

            migrationBuilder.CreateTable(
                name: "proveedores",
                columns: table => new
                {
                    proveedor_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    razon_social = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    rut = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    nombre_contacto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    direccion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    notas = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    creado_en = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_proveedores", x => x.proveedor_id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    rol_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    creado_en = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.rol_id);
                });

            migrationBuilder.CreateTable(
                name: "productos",
                columns: table => new
                {
                    producto_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    categoria_id = table.Column<int>(type: "int", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    codigo_barras = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    codigo_interno = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    unidad_medida = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "UN"),
                    precio_compra = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false, defaultValue: 0m),
                    precio_venta = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    stock_actual = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: false, defaultValue: 0m),
                    stock_minimo = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: false, defaultValue: 0m),
                    tiene_codigo_propio = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    imagen_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    creado_en = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    actualizado_en = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productos", x => x.producto_id);
                    table.ForeignKey(
                        name: "FK_productos_categorias_categoria_id",
                        column: x => x.categoria_id,
                        principalTable: "categorias",
                        principalColumn: "categoria_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    usuario_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rol_id = table.Column<int>(type: "int", nullable: false),
                    nombre_completo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    nombre_usuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    creado_en = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ultimo_acceso = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.usuario_id);
                    table.ForeignKey(
                        name: "FK_usuarios_roles_rol_id",
                        column: x => x.rol_id,
                        principalTable: "roles",
                        principalColumn: "rol_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ordenes_compra",
                columns: table => new
                {
                    orden_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    proveedor_id = table.Column<int>(type: "int", nullable: false),
                    creado_por = table.Column<int>(type: "int", nullable: false),
                    recibido_por = table.Column<int>(type: "int", nullable: true),
                    numero_orden = table.Column<int>(type: "int", nullable: false),
                    estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "BORRADOR"),
                    total_estimado = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false, defaultValue: 0m),
                    notas = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    fecha_creacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    fecha_recepcion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ordenes_compra", x => x.orden_id);
                    table.ForeignKey(
                        name: "FK_ordenes_compra_proveedores_proveedor_id",
                        column: x => x.proveedor_id,
                        principalTable: "proveedores",
                        principalColumn: "proveedor_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ordenes_compra_usuarios_creado_por",
                        column: x => x.creado_por,
                        principalTable: "usuarios",
                        principalColumn: "usuario_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ordenes_compra_usuarios_recibido_por",
                        column: x => x.recibido_por,
                        principalTable: "usuarios",
                        principalColumn: "usuario_id");
                });

            migrationBuilder.CreateTable(
                name: "sesiones",
                columns: table => new
                {
                    sesion_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    token_hash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ip_origen = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    creado_en = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    expira_en = table.Column<DateTime>(type: "datetime2", nullable: false),
                    activa = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sesiones", x => x.sesion_id);
                    table.ForeignKey(
                        name: "FK_sesiones_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "usuario_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ventas",
                columns: table => new
                {
                    venta_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    medio_pago_id = table.Column<int>(type: "int", nullable: false),
                    numero_boleta = table.Column<int>(type: "int", nullable: false),
                    subtotal_neto = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    monto_iva = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    descuento_total = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false, defaultValue: 0m),
                    total = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    monto_pagado = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    vuelto = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false, defaultValue: 0m),
                    estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "COMPLETADA"),
                    motivo_anulacion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    anulada_por = table.Column<int>(type: "int", nullable: true),
                    fecha_anulacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    fecha_venta = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ventas", x => x.venta_id);
                    table.ForeignKey(
                        name: "FK_ventas_medios_pago_medio_pago_id",
                        column: x => x.medio_pago_id,
                        principalTable: "medios_pago",
                        principalColumn: "medio_pago_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ventas_usuarios_anulada_por",
                        column: x => x.anulada_por,
                        principalTable: "usuarios",
                        principalColumn: "usuario_id");
                    table.ForeignKey(
                        name: "FK_ventas_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "usuario_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detalle_orden_compra",
                columns: table => new
                {
                    detalle_orden_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    orden_id = table.Column<int>(type: "int", nullable: false),
                    producto_id = table.Column<int>(type: "int", nullable: false),
                    cantidad_pedida = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: false),
                    cantidad_recibida = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: false, defaultValue: 0m),
                    precio_unitario_compra = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    subtotal = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detalle_orden_compra", x => x.detalle_orden_id);
                    table.ForeignKey(
                        name: "FK_detalle_orden_compra_ordenes_compra_orden_id",
                        column: x => x.orden_id,
                        principalTable: "ordenes_compra",
                        principalColumn: "orden_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_detalle_orden_compra_productos_producto_id",
                        column: x => x.producto_id,
                        principalTable: "productos",
                        principalColumn: "producto_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detalle_venta",
                columns: table => new
                {
                    detalle_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    venta_id = table.Column<int>(type: "int", nullable: false),
                    producto_id = table.Column<int>(type: "int", nullable: false),
                    nombre_producto = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    codigo_barras = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    cantidad = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: false),
                    precio_unitario = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    descuento_item = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false, defaultValue: 0m),
                    subtotal = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detalle_venta", x => x.detalle_id);
                    table.ForeignKey(
                        name: "FK_detalle_venta_productos_producto_id",
                        column: x => x.producto_id,
                        principalTable: "productos",
                        principalColumn: "producto_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_detalle_venta_ventas_venta_id",
                        column: x => x.venta_id,
                        principalTable: "ventas",
                        principalColumn: "venta_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stock_movimientos",
                columns: table => new
                {
                    movimiento_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    producto_id = table.Column<int>(type: "int", nullable: false),
                    usuario_id = table.Column<int>(type: "int", nullable: false),
                    venta_id = table.Column<int>(type: "int", nullable: true),
                    orden_compra_id = table.Column<int>(type: "int", nullable: true),
                    tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    cantidad = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: false),
                    stock_antes = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: false),
                    stock_despues = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: false),
                    motivo = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    creado_en = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stock_movimientos", x => x.movimiento_id);
                    table.ForeignKey(
                        name: "FK_stock_movimientos_ordenes_compra_orden_compra_id",
                        column: x => x.orden_compra_id,
                        principalTable: "ordenes_compra",
                        principalColumn: "orden_id");
                    table.ForeignKey(
                        name: "FK_stock_movimientos_productos_producto_id",
                        column: x => x.producto_id,
                        principalTable: "productos",
                        principalColumn: "producto_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_stock_movimientos_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "usuario_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_stock_movimientos_ventas_venta_id",
                        column: x => x.venta_id,
                        principalTable: "ventas",
                        principalColumn: "venta_id");
                });

            migrationBuilder.InsertData(
                table: "categorias",
                columns: new[] { "categoria_id", "activa", "creado_en", "descripcion", "nombre" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Abarrotes" },
                    { 2, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bebestibles" },
                    { 3, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Lácteos" },
                    { 4, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Panadería y Pastelería" },
                    { 5, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Frutas y Verduras" },
                    { 6, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Carnes y Embutidos" },
                    { 7, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Limpieza y Hogar" },
                    { 8, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Higiene Personal" },
                    { 9, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Snacks y Confites" },
                    { 10, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Congelados" },
                    { 11, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Otros" }
                });

            migrationBuilder.InsertData(
                table: "medios_pago",
                columns: new[] { "medio_pago_id", "activo", "nombre", "requiere_vuelto" },
                values: new object[] { 1, true, "Efectivo", true });

            migrationBuilder.InsertData(
                table: "medios_pago",
                columns: new[] { "medio_pago_id", "activo", "nombre" },
                values: new object[,]
                {
                    { 2, true, "Débito" },
                    { 3, true, "Crédito" },
                    { 4, true, "Transferencia" }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "rol_id", "activo", "creado_en", "descripcion", "nombre" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Acceso total al sistema", "admin" },
                    { 2, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Acceso al punto de venta e inventario básico", "cajero" },
                    { 3, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Acceso a inventario, stock y recepciones", "bodeguero" }
                });

            migrationBuilder.InsertData(
                table: "usuarios",
                columns: new[] { "usuario_id", "activo", "creado_en", "nombre_completo", "nombre_usuario", "password_hash", "rol_id", "ultimo_acceso" },
                values: new object[] { 1, true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Administrador", "admin", "$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewdBPj7oFJDKHbPm", 1, null });

            migrationBuilder.CreateIndex(
                name: "IX_categorias_nombre",
                table: "categorias",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_detalle_orden_compra_orden_id",
                table: "detalle_orden_compra",
                column: "orden_id");

            migrationBuilder.CreateIndex(
                name: "IX_detalle_orden_compra_producto_id",
                table: "detalle_orden_compra",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "IX_detalle_venta_producto_id",
                table: "detalle_venta",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "IX_detalle_venta_venta_id",
                table: "detalle_venta",
                column: "venta_id");

            migrationBuilder.CreateIndex(
                name: "IX_medios_pago_nombre",
                table: "medios_pago",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ordenes_compra_creado_por",
                table: "ordenes_compra",
                column: "creado_por");

            migrationBuilder.CreateIndex(
                name: "IX_ordenes_compra_proveedor_id",
                table: "ordenes_compra",
                column: "proveedor_id");

            migrationBuilder.CreateIndex(
                name: "IX_ordenes_compra_recibido_por",
                table: "ordenes_compra",
                column: "recibido_por");

            migrationBuilder.CreateIndex(
                name: "IX_productos_categoria_id",
                table: "productos",
                column: "categoria_id");

            migrationBuilder.CreateIndex(
                name: "IX_productos_codigo_barras",
                table: "productos",
                column: "codigo_barras",
                unique: true,
                filter: "[codigo_barras] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_productos_codigo_interno",
                table: "productos",
                column: "codigo_interno",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_productos_nombre",
                table: "productos",
                column: "nombre");

            migrationBuilder.CreateIndex(
                name: "IX_roles_nombre",
                table: "roles",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sesiones_usuario_id",
                table: "sesiones",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_movimientos_orden_compra_id",
                table: "stock_movimientos",
                column: "orden_compra_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_movimientos_producto_id",
                table: "stock_movimientos",
                column: "producto_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_movimientos_usuario_id",
                table: "stock_movimientos",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_stock_movimientos_venta_id",
                table: "stock_movimientos",
                column: "venta_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_nombre_usuario",
                table: "usuarios",
                column: "nombre_usuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_rol_id",
                table: "usuarios",
                column: "rol_id");

            migrationBuilder.CreateIndex(
                name: "IX_ventas_anulada_por",
                table: "ventas",
                column: "anulada_por");

            migrationBuilder.CreateIndex(
                name: "IX_ventas_fecha_venta",
                table: "ventas",
                column: "fecha_venta");

            migrationBuilder.CreateIndex(
                name: "IX_ventas_medio_pago_id",
                table: "ventas",
                column: "medio_pago_id");

            migrationBuilder.CreateIndex(
                name: "IX_ventas_usuario_id",
                table: "ventas",
                column: "usuario_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "config_impresora");

            migrationBuilder.DropTable(
                name: "detalle_orden_compra");

            migrationBuilder.DropTable(
                name: "detalle_venta");

            migrationBuilder.DropTable(
                name: "empresa");

            migrationBuilder.DropTable(
                name: "sesiones");

            migrationBuilder.DropTable(
                name: "stock_movimientos");

            migrationBuilder.DropTable(
                name: "ordenes_compra");

            migrationBuilder.DropTable(
                name: "productos");

            migrationBuilder.DropTable(
                name: "ventas");

            migrationBuilder.DropTable(
                name: "proveedores");

            migrationBuilder.DropTable(
                name: "categorias");

            migrationBuilder.DropTable(
                name: "medios_pago");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
