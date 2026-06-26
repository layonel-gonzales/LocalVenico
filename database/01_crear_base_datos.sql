-- ============================================================
-- LocalVenico - Sistema de Gestión para Almacén de Abarrotes
-- Base de datos principal
-- SQL Server 2019 / SQL Server Express
-- Autor: LocalVenico
-- ============================================================

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'LocalVenico')
BEGIN
    CREATE DATABASE LocalVenico
    COLLATE Modern_Spanish_CI_AI;
END
GO

USE LocalVenico;
GO

-- ============================================================
-- SEGURIDAD Y ACCESO
-- ============================================================

CREATE TABLE roles (
    rol_id          INT IDENTITY(1,1) PRIMARY KEY,
    nombre          VARCHAR(50)     NOT NULL UNIQUE,
    descripcion     VARCHAR(200)    NULL,
    activo          BIT             NOT NULL DEFAULT 1,
    creado_en       DATETIME2       NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE usuarios (
    usuario_id      INT IDENTITY(1,1) PRIMARY KEY,
    rol_id          INT             NOT NULL,
    nombre_completo VARCHAR(100)    NOT NULL,
    nombre_usuario  VARCHAR(50)     NOT NULL UNIQUE,
    password_hash   VARCHAR(256)    NOT NULL,   -- BCrypt hash
    activo          BIT             NOT NULL DEFAULT 1,
    creado_en       DATETIME2       NOT NULL DEFAULT GETDATE(),
    ultimo_acceso   DATETIME2       NULL,
    CONSTRAINT FK_usuarios_rol FOREIGN KEY (rol_id) REFERENCES roles(rol_id)
);
GO

CREATE TABLE sesiones (
    sesion_id       UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    usuario_id      INT             NOT NULL,
    token_hash      VARCHAR(256)    NOT NULL,
    ip_origen       VARCHAR(45)     NULL,
    creado_en       DATETIME2       NOT NULL DEFAULT GETDATE(),
    expira_en       DATETIME2       NOT NULL,
    activa          BIT             NOT NULL DEFAULT 1,
    CONSTRAINT FK_sesiones_usuario FOREIGN KEY (usuario_id) REFERENCES usuarios(usuario_id)
);
GO

-- ============================================================
-- CONFIGURACIÓN DE LA EMPRESA
-- ============================================================

CREATE TABLE empresa (
    empresa_id          INT IDENTITY(1,1) PRIMARY KEY,
    razon_social        VARCHAR(150)    NOT NULL,
    rut                 VARCHAR(12)     NOT NULL,           -- Ej: 12.345.678-9
    direccion           VARCHAR(200)    NOT NULL,
    comuna              VARCHAR(100)    NOT NULL,
    region              VARCHAR(100)    NOT NULL DEFAULT 'Metropolitana',
    telefono            VARCHAR(20)     NULL,
    email               VARCHAR(150)    NULL,
    giro                VARCHAR(150)    NOT NULL DEFAULT 'Almacén de Abarrotes',
    logo_ruta           VARCHAR(500)    NULL,               -- Ruta al archivo de logo
    porcentaje_iva      DECIMAL(5,2)    NOT NULL DEFAULT 19.00,
    precios_con_iva     BIT             NOT NULL DEFAULT 1, -- TRUE = precio venta ya incluye IVA
    moneda              VARCHAR(5)      NOT NULL DEFAULT 'CLP',
    numero_boleta_inicio INT            NOT NULL DEFAULT 1, -- Correlativo interno
    actualizado_en      DATETIME2       NOT NULL DEFAULT GETDATE()
);
GO

-- Solo puede existir un registro de empresa
CREATE UNIQUE INDEX UX_empresa_unica ON empresa ((1));
GO

-- ============================================================
-- INVENTARIO
-- ============================================================

CREATE TABLE categorias (
    categoria_id    INT IDENTITY(1,1) PRIMARY KEY,
    nombre          VARCHAR(100)    NOT NULL UNIQUE,
    descripcion     VARCHAR(300)    NULL,
    activa          BIT             NOT NULL DEFAULT 1,
    creado_en       DATETIME2       NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE productos (
    producto_id         INT IDENTITY(1,1) PRIMARY KEY,
    categoria_id        INT             NOT NULL,
    nombre              VARCHAR(150)    NOT NULL,
    codigo_barras       VARCHAR(30)     NULL UNIQUE,       -- EAN-13, UPC-A, etc.
    codigo_interno      VARCHAR(20)     NOT NULL UNIQUE,   -- Generado internamente si no tiene barras
    descripcion         VARCHAR(500)    NULL,
    unidad_medida       VARCHAR(20)     NOT NULL DEFAULT 'UN', -- UN, KG, LT, MT, GR
    precio_compra       DECIMAL(12,2)   NOT NULL DEFAULT 0,
    precio_venta        DECIMAL(12,2)   NOT NULL,          -- Con IVA incluido (para almacén retail)
    stock_actual        DECIMAL(12,3)   NOT NULL DEFAULT 0,
    stock_minimo        DECIMAL(12,3)   NOT NULL DEFAULT 0,
    tiene_codigo_propio BIT             NOT NULL DEFAULT 0, -- FALSE = generamos etiqueta interna
    imagen_url          VARCHAR(500)    NULL,
    activo              BIT             NOT NULL DEFAULT 1,
    creado_en           DATETIME2       NOT NULL DEFAULT GETDATE(),
    actualizado_en      DATETIME2       NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_productos_categoria FOREIGN KEY (categoria_id) REFERENCES categorias(categoria_id)
);
GO

CREATE INDEX IX_productos_codigo_barras  ON productos(codigo_barras)  WHERE codigo_barras IS NOT NULL;
CREATE INDEX IX_productos_codigo_interno ON productos(codigo_interno);
CREATE INDEX IX_productos_nombre         ON productos(nombre);
CREATE INDEX IX_productos_categoria      ON productos(categoria_id);
GO

-- ============================================================
-- PROVEEDORES
-- ============================================================

CREATE TABLE proveedores (
    proveedor_id    INT IDENTITY(1,1) PRIMARY KEY,
    razon_social    VARCHAR(150)    NOT NULL,
    rut             VARCHAR(12)     NOT NULL,
    nombre_contacto VARCHAR(100)    NULL,
    telefono        VARCHAR(20)     NULL,
    email           VARCHAR(150)    NULL,
    direccion       VARCHAR(200)    NULL,
    notas           VARCHAR(500)    NULL,
    activo          BIT             NOT NULL DEFAULT 1,
    creado_en       DATETIME2       NOT NULL DEFAULT GETDATE()
);
GO

-- ============================================================
-- PUNTO DE VENTA
-- ============================================================

CREATE TABLE medios_pago (
    medio_pago_id   INT IDENTITY(1,1) PRIMARY KEY,
    nombre          VARCHAR(50)     NOT NULL UNIQUE,  -- Efectivo, Débito, Crédito, Transferencia
    requiere_vuelto BIT             NOT NULL DEFAULT 0,
    activo          BIT             NOT NULL DEFAULT 1
);
GO

CREATE TABLE ventas (
    venta_id            INT IDENTITY(1,1) PRIMARY KEY,
    usuario_id          INT             NOT NULL,       -- Cajero que realizó la venta
    medio_pago_id       INT             NOT NULL,
    numero_boleta       INT             NOT NULL,       -- Correlativo interno de boleta
    subtotal_neto       DECIMAL(12,2)   NOT NULL,       -- Sin IVA
    monto_iva           DECIMAL(12,2)   NOT NULL,       -- Monto IVA
    descuento_total     DECIMAL(12,2)   NOT NULL DEFAULT 0,
    total               DECIMAL(12,2)   NOT NULL,       -- Total con IVA, con descuento aplicado
    monto_pagado        DECIMAL(12,2)   NOT NULL,
    vuelto              DECIMAL(12,2)   NOT NULL DEFAULT 0,
    estado              VARCHAR(20)     NOT NULL DEFAULT 'COMPLETADA', -- COMPLETADA, ANULADA
    motivo_anulacion    VARCHAR(300)    NULL,
    anulada_por         INT             NULL,           -- FK a usuarios
    fecha_anulacion     DATETIME2       NULL,
    fecha_venta         DATETIME2       NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_ventas_usuario        FOREIGN KEY (usuario_id)    REFERENCES usuarios(usuario_id),
    CONSTRAINT FK_ventas_medio_pago     FOREIGN KEY (medio_pago_id) REFERENCES medios_pago(medio_pago_id),
    CONSTRAINT FK_ventas_anulada_por    FOREIGN KEY (anulada_por)   REFERENCES usuarios(usuario_id),
    CONSTRAINT CK_ventas_estado         CHECK (estado IN ('COMPLETADA','ANULADA'))
);
GO

CREATE INDEX IX_ventas_fecha       ON ventas(fecha_venta);
CREATE INDEX IX_ventas_usuario     ON ventas(usuario_id);
CREATE INDEX IX_ventas_estado      ON ventas(estado);
CREATE INDEX IX_ventas_numero      ON ventas(numero_boleta);
GO

CREATE TABLE detalle_venta (
    detalle_id      INT IDENTITY(1,1) PRIMARY KEY,
    venta_id        INT             NOT NULL,
    producto_id     INT             NOT NULL,
    nombre_producto VARCHAR(150)    NOT NULL,   -- Snapshot: nombre al momento de vender
    codigo_barras   VARCHAR(30)     NULL,        -- Snapshot
    cantidad        DECIMAL(12,3)   NOT NULL,
    precio_unitario DECIMAL(12,2)   NOT NULL,    -- Con IVA al momento de vender
    descuento_item  DECIMAL(12,2)   NOT NULL DEFAULT 0,
    subtotal        DECIMAL(12,2)   NOT NULL,    -- (precio_unitario * cantidad) - descuento_item
    CONSTRAINT FK_detalle_venta_venta    FOREIGN KEY (venta_id)   REFERENCES ventas(venta_id),
    CONSTRAINT FK_detalle_venta_producto FOREIGN KEY (producto_id) REFERENCES productos(producto_id)
);
GO

CREATE INDEX IX_detalle_venta_venta    ON detalle_venta(venta_id);
CREATE INDEX IX_detalle_venta_producto ON detalle_venta(producto_id);
GO

-- ============================================================
-- MOVIMIENTOS DE STOCK
-- ============================================================

CREATE TABLE stock_movimientos (
    movimiento_id       INT IDENTITY(1,1) PRIMARY KEY,
    producto_id         INT             NOT NULL,
    usuario_id          INT             NOT NULL,
    venta_id            INT             NULL,           -- Asociado si el movimiento es por venta
    orden_compra_id     INT             NULL,           -- Asociado si es por recepción de compra
    tipo                VARCHAR(20)     NOT NULL,       -- ENTRADA, SALIDA, AJUSTE, VENTA, ANULACION
    cantidad            DECIMAL(12,3)   NOT NULL,       -- Positivo = entrada, negativo = salida
    stock_antes         DECIMAL(12,3)   NOT NULL,
    stock_despues       DECIMAL(12,3)   NOT NULL,
    motivo              VARCHAR(300)    NULL,           -- Descripción manual para ajustes
    creado_en           DATETIME2       NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_stock_mov_producto    FOREIGN KEY (producto_id)    REFERENCES productos(producto_id),
    CONSTRAINT FK_stock_mov_usuario     FOREIGN KEY (usuario_id)     REFERENCES usuarios(usuario_id),
    CONSTRAINT FK_stock_mov_venta       FOREIGN KEY (venta_id)       REFERENCES ventas(venta_id),
    CONSTRAINT CK_stock_mov_tipo        CHECK (tipo IN ('ENTRADA','SALIDA','AJUSTE','VENTA','ANULACION','COMPRA'))
);
GO

CREATE INDEX IX_stock_mov_producto ON stock_movimientos(producto_id);
CREATE INDEX IX_stock_mov_fecha    ON stock_movimientos(creado_en);
GO

-- ============================================================
-- ÓRDENES DE COMPRA
-- ============================================================

CREATE TABLE ordenes_compra (
    orden_id        INT IDENTITY(1,1) PRIMARY KEY,
    proveedor_id    INT             NOT NULL,
    creado_por      INT             NOT NULL,           -- FK a usuarios
    recibido_por    INT             NULL,               -- FK a usuarios, cuando se recibe
    numero_orden    INT             NOT NULL,           -- Correlativo interno
    estado          VARCHAR(20)     NOT NULL DEFAULT 'BORRADOR', -- BORRADOR, ENVIADA, RECIBIDA, CANCELADA
    total_estimado  DECIMAL(12,2)   NOT NULL DEFAULT 0,
    notas           VARCHAR(500)    NULL,
    fecha_creacion  DATETIME2       NOT NULL DEFAULT GETDATE(),
    fecha_recepcion DATETIME2       NULL,
    CONSTRAINT FK_oc_proveedor   FOREIGN KEY (proveedor_id) REFERENCES proveedores(proveedor_id),
    CONSTRAINT FK_oc_creado_por  FOREIGN KEY (creado_por)   REFERENCES usuarios(usuario_id),
    CONSTRAINT FK_oc_recibido_por FOREIGN KEY (recibido_por) REFERENCES usuarios(usuario_id),
    CONSTRAINT CK_oc_estado      CHECK (estado IN ('BORRADOR','ENVIADA','RECIBIDA','CANCELADA'))
);
GO

CREATE TABLE detalle_orden_compra (
    detalle_orden_id        INT IDENTITY(1,1) PRIMARY KEY,
    orden_id                INT             NOT NULL,
    producto_id             INT             NOT NULL,
    cantidad_pedida         DECIMAL(12,3)   NOT NULL,
    cantidad_recibida       DECIMAL(12,3)   NOT NULL DEFAULT 0,
    precio_unitario_compra  DECIMAL(12,2)   NOT NULL,
    subtotal                DECIMAL(12,2)   NOT NULL,
    CONSTRAINT FK_doc_orden    FOREIGN KEY (orden_id)    REFERENCES ordenes_compra(orden_id),
    CONSTRAINT FK_doc_producto FOREIGN KEY (producto_id) REFERENCES productos(producto_id)
);
GO

-- FK tardía: stock_movimientos → ordenes_compra
ALTER TABLE stock_movimientos
    ADD CONSTRAINT FK_stock_mov_orden
    FOREIGN KEY (orden_compra_id) REFERENCES ordenes_compra(orden_id);
GO

-- ============================================================
-- CONFIGURACIÓN DE IMPRESORA
-- ============================================================

CREATE TABLE config_impresora (
    config_id           INT IDENTITY(1,1) PRIMARY KEY,
    nombre_impresora    VARCHAR(200)    NOT NULL,       -- Nombre en Windows
    tipo                VARCHAR(20)     NOT NULL DEFAULT 'TERMICA_80MM',
    lineas_encabezado   INT             NOT NULL DEFAULT 3,
    lineas_pie          INT             NOT NULL DEFAULT 3,
    mostrar_logo        BIT             NOT NULL DEFAULT 0,
    activa              BIT             NOT NULL DEFAULT 1,
    actualizado_en      DATETIME2       NOT NULL DEFAULT GETDATE()
);
GO

-- ============================================================
-- DATOS INICIALES (SEED)
-- ============================================================

-- Roles base
INSERT INTO roles (nombre, descripcion) VALUES
    ('admin',      'Acceso total al sistema'),
    ('cajero',     'Acceso al punto de venta e inventario básico'),
    ('bodeguero',  'Acceso a inventario, stock y recepciones');
GO

-- Usuario administrador por defecto
-- Password: Admin1234  (BCrypt hash de ejemplo — cambiar al iniciar)
INSERT INTO usuarios (rol_id, nombre_completo, nombre_usuario, password_hash)
VALUES (
    (SELECT rol_id FROM roles WHERE nombre = 'admin'),
    'Administrador',
    'admin',
    '$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewdBPj7oFJDKHbPm'
);
GO

-- Medios de pago estándar Chile
INSERT INTO medios_pago (nombre, requiere_vuelto) VALUES
    ('Efectivo',        1),
    ('Débito',          0),
    ('Crédito',         0),
    ('Transferencia',   0);
GO

-- Categorías base para almacén de abarrotes
INSERT INTO categorias (nombre) VALUES
    ('Abarrotes'),
    ('Bebestibles'),
    ('Lácteos'),
    ('Panadería y Pastelería'),
    ('Frutas y Verduras'),
    ('Carnes y Embutidos'),
    ('Limpieza y Hogar'),
    ('Higiene Personal'),
    ('Snacks y Confites'),
    ('Congelados'),
    ('Otros');
GO

-- ============================================================
-- VISTAS ÚTILES
-- ============================================================

-- Productos con stock bajo mínimo
CREATE VIEW vw_productos_stock_bajo AS
SELECT
    p.producto_id,
    p.codigo_interno,
    p.codigo_barras,
    p.nombre,
    c.nombre        AS categoria,
    p.stock_actual,
    p.stock_minimo,
    (p.stock_minimo - p.stock_actual) AS deficit
FROM productos p
JOIN categorias c ON p.categoria_id = c.categoria_id
WHERE p.activo = 1
  AND p.stock_actual < p.stock_minimo;
GO

-- Resumen de ventas del día actual
CREATE VIEW vw_ventas_hoy AS
SELECT
    COUNT(*)            AS total_transacciones,
    SUM(total)          AS total_ventas,
    SUM(descuento_total) AS total_descuentos,
    SUM(monto_iva)      AS total_iva,
    SUM(subtotal_neto)  AS total_neto
FROM ventas
WHERE estado = 'COMPLETADA'
  AND CAST(fecha_venta AS DATE) = CAST(GETDATE() AS DATE);
GO

-- Detalle de ventas con información del cajero
CREATE VIEW vw_ventas_detalle AS
SELECT
    v.venta_id,
    v.numero_boleta,
    v.fecha_venta,
    u.nombre_completo   AS cajero,
    mp.nombre           AS medio_pago,
    v.subtotal_neto,
    v.monto_iva,
    v.descuento_total,
    v.total,
    v.monto_pagado,
    v.vuelto,
    v.estado
FROM ventas v
JOIN usuarios u     ON v.usuario_id    = u.usuario_id
JOIN medios_pago mp ON v.medio_pago_id = mp.medio_pago_id;
GO

PRINT 'Base de datos LocalVenico creada exitosamente.';
GO
