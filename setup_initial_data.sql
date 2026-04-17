-- ============================================================
-- SCRIPT DE CONFIGURACIÓN INICIAL - sistema_ventasbd
-- Ejecutar este script DESPUÉS de crear la BD con el SQL original
-- ============================================================

-- Corrección: el INSERT original usa password '1234' en texto plano.
-- La aplicación valida con SHA256. 
-- SHA256('1234') = 03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4

USE sistema_ventasbd;
GO

-- Actualizar el hash del admin al valor SHA256 de '1234'
UPDATE Usuarios
SET password_hash = '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4'
WHERE usuario = 'admin';
GO

-- Agregar permisos para el rol Administrador (id_rol = 1)
-- El Administrador tiene acceso total a todos los módulos
INSERT INTO Permisos (id_rol, modulo, puede_ver, puede_crear, puede_editar, puede_eliminar) VALUES
(1, 'Productos',   1, 1, 1, 1),
(1, 'Categorias',  1, 1, 1, 1),
(1, 'Usuarios',    1, 1, 1, 1),
(1, 'Ventas',      1, 1, 1, 1),
(1, 'Compras',     1, 1, 1, 1),
(1, 'Clientes',    1, 1, 1, 1),
(1, 'Proveedores', 1, 1, 1, 1),
(1, 'Reportes',    1, 1, 1, 1),
(1, 'Caja',        1, 1, 1, 1),
(1, 'Descuentos',  1, 1, 1, 1);
GO

-- Permisos para Cajero (id_rol = 2): solo puede ver, crear ventas y ver reportes básicos
INSERT INTO Permisos (id_rol, modulo, puede_ver, puede_crear, puede_editar, puede_eliminar) VALUES
(2, 'Productos',   1, 0, 0, 0),
(2, 'Ventas',      1, 1, 0, 0),
(2, 'Clientes',    1, 1, 0, 0),
(2, 'Caja',        1, 1, 1, 0),
(2, 'Reportes',    1, 0, 0, 0);
GO

-- Permisos para Almacenero (id_rol = 3)
INSERT INTO Permisos (id_rol, modulo, puede_ver, puede_crear, puede_editar, puede_eliminar) VALUES
(3, 'Productos',   1, 1, 1, 0),
(3, 'Categorias',  1, 0, 0, 0),
(3, 'Compras',     1, 1, 1, 0),
(3, 'Proveedores', 1, 1, 1, 0);
GO

-- Permisos para Supervisor (id_rol = 4)
INSERT INTO Permisos (id_rol, modulo, puede_ver, puede_crear, puede_editar, puede_eliminar) VALUES
(4, 'Productos',   1, 0, 1, 0),
(4, 'Ventas',      1, 1, 1, 0),
(4, 'Clientes',    1, 1, 1, 0),
(4, 'Reportes',    1, 0, 0, 0),
(4, 'Caja',        1, 1, 1, 0),
(4, 'Descuentos',  1, 1, 1, 0);
GO

-- Verificar 
SELECT u.nombre, r.nombre AS rol, p.modulo, p.puede_ver, p.puede_crear, p.puede_editar, p.puede_eliminar
FROM Usuarios u
JOIN Roles r ON u.id_rol = r.id_rol
LEFT JOIN Permisos p ON p.id_rol = r.id_rol
ORDER BY u.nombre, p.modulo;
GO
