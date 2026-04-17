-- ============================================================
-- db_changes.sql
-- Ejecutar este script en SSMS sobre la BD sistema_ventasbd
-- ANTES de correr la aplicación con los nuevos cambios.
-- ============================================================

USE sistema_ventasbd;
GO

-- ── 1. Agregar columna color a Categorias ──────────────────
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'Categorias' AND COLUMN_NAME = 'color'
)
BEGIN
    ALTER TABLE Categorias ADD color VARCHAR(20) DEFAULT '#E8A87C';
    PRINT 'Columna color agregada a Categorias.';
END
ELSE
    PRINT 'Columna color ya existe en Categorias.';
GO

-- ── 2. Insertar 5 categorías típicas de cafetería ─────────
-- Solo se insertan si la tabla está vacía.
IF NOT EXISTS (SELECT 1 FROM Categorias)
BEGIN
    INSERT INTO Categorias (nombre, descripcion, color, activo) VALUES
    ('Bebidas Calientes', 'Cafés, tés, chocolates calientes', '#E8A87C', 1),
    ('Bebidas Frías',     'Jugos, refrescos, batidos, smoothies', '#4FC3F7', 1),
    ('Comidas',           'Platos del día, sánduches, snacks salados', '#4CAF50', 1),
    ('Postres',           'Dulces, pasteles, galletas, helados', '#F48FB1', 1),
    ('Panadería',         'Pan, croissants, bizcochos, repostería', '#FFB74D', 1);
    PRINT '5 categorías insertadas correctamente.';
END
ELSE
    PRINT 'La tabla Categorias ya tiene datos. No se insertaron categorías nuevas.';
GO

-- ── 3. Crear tabla Logs_Actividad ─────────────────────────
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.TABLES
    WHERE TABLE_NAME = 'Logs_Actividad'
)
BEGIN
    CREATE TABLE Logs_Actividad (
        id_log      INT IDENTITY(1,1) PRIMARY KEY,
        id_usuario  INT,
        accion      VARCHAR(100) NOT NULL,
        modulo      VARCHAR(50),
        detalles    VARCHAR(MAX),
        fecha       DATETIME DEFAULT GETDATE(),
        CONSTRAINT FK_Logs_Usuario
            FOREIGN KEY (id_usuario) REFERENCES Usuarios(id_usuario)
            ON DELETE SET NULL
    );
    PRINT 'Tabla Logs_Actividad creada correctamente.';
END
ELSE
    PRINT 'Tabla Logs_Actividad ya existe.';
GO

PRINT '=== Script completado. ===';
