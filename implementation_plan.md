# Sistema de Ventas para Cafetería — Plan de Implementación

## Descripción General

Se construirá un sistema de punto de venta (POS) completo para cafetería/tienda usando **C# WPF + MVVM + Arquitectura Limpia**. El sistema operará 100% en memoria (Mock Services) con una arquitectura que permita sustituir los mocks por repositorios SQL reales sin tocar la UI ni la lógica de negocio.

---

## Estructura de la Solución (5 Proyectos)

```
CafeteriaSystem.sln
├── CafeteriaSystem.Domain           ← Entidades + Contratos (Interfaces)
├── CafeteriaSystem.Application      ← Casos de uso, DTOs, Lógica de Negocio
├── CafeteriaSystem.Infrastructure   ← Mock Services (en memoria)
├── CafeteriaSystem.WPF              ← UI WPF + ViewModels + Commands
└── CafeteriaSystem.Tests            ← (Estructura base, sin tests exhaustivos aún)
```

---

## Propuestas de Cambio por Capa

---

### 1. CafeteriaSystem.Domain

Define las 15 entidades del negocio y las interfaces de los repositorios. **No contiene lógica, solo contratos y modelos.**

#### Entidades (15 Tablas)

| # | Entidad | Descripción |
|---|---------|-------------|
| 1 | `Product` | Producto con precio, impuesto y stock |
| 2 | `Category` | Categoría de productos |
| 3 | `Sale` | Cabecera de venta (fecha, cliente, cajero, estado) |
| 4 | `SaleDetail` | Línea de detalle de venta (producto, cant, precio, descuento) |
| 5 | `Inventory` | Stock actual por producto + almacén |
| 6 | `KardexEntry` | Movimiento de inventario (kardex: entrada/salida/ajuste) |
| 7 | `User` | Usuario del sistema (cajero, admin, supervisor) |
| 8 | `Role` | Rol con permisos granulares |
| 9 | `Customer` | Cliente con historial de compras |
| 10 | `Purchase` | Orden de compra a proveedor |
| 11 | `PurchaseDetail` | Línea de detalle de compra |
| 12 | `Supplier` | Proveedor |
| 13 | `CashRegister` | Sesión de caja (apertura, cierre, diferencia) |
| 14 | `Discount` | Reglas de descuento (%) aplicables a productos/categorías |
| 15 | `TaxRate` | Configuración de tasas de impuesto por producto |

#### Interfaces de Repositorio (principales)

```csharp
IProductRepository    ISaleRepository      IInventoryRepository
ICategoryRepository   ICustomerRepository  IKardexRepository
IUserRepository       IPurchaseRepository  ICashRegisterRepository
ISupplierRepository
```

---

### 2. CafeteriaSystem.Application

Contiene los **Casos de Uso** (servicios de aplicación) y DTOs. Depende solo de `Domain`.

**Servicios principales:**
- `SaleService` — Crear venta, aplicar descuentos, calcular impuestos, validar stock
- `InventoryService` — Consultar stock, registrar movimientos de kardex
- `CashRegisterService` — Apertura/cierre de caja, arqueo
- `ReportService` — Generación de datos para gráficas (ventas por día, top productos)
- `AuthService` — Autenticación de usuario (validación en memoria)

**Lógica de Negocio implementada:**
- Cálculo de impuesto por línea y total
- Aplicación de descuentos (por producto o categoría)
- Validación de stock antes de confirmar venta
- Generación de kardex automático al vender/comprar

---

### 3. CafeteriaSystem.Infrastructure (Mock Services)

Implementa todas las interfaces con **datos en memoria** (listas precargadas con datos de muestra realistas).

```
Infrastructure/
├── MockData/         ← Seeders con datos de ejemplo
├── Repositories/     ← MockProductRepository, MockSaleRepository, etc.
└── DependencyInjection.cs ← Registro de todos los servicios Mock
```

Los mocks tendrán **50+ productos, 5 categorías, 10 clientes, y ventas históricas** para que los reportes y gráficas se vean reales desde el día uno.

---

### 4. CafeteriaSystem.WPF (UI + MVVM)

#### Vistas Principales

| Vista | Descripción |
|-------|-------------|
| `MainWindow` | Shell con sidebar de navegación y área de contenido |
| `LoginView` | Pantalla de inicio de sesión elegante |
| `DashboardView` | KPIs, gráficas de ventas, alertas de stock bajo |
| `POSView` | Punto de Venta: catálogo + carrito + totales en tiempo real |
| `InventoryView` | Lista de productos con stock, filtros, kardex |
| `SalesHistoryView` | Historial de ventas con filtro por fecha/cliente |
| `ReportsView` | Gráficas de ventas diarias/semanales, top productos |
| `ProductsView` | CRUD de productos y categorías |
| `SuppliersView` | Gestión de proveedores y compras |
| `CashRegisterView` | Apertura/cierre de caja, arqueo |
| `UsersView` | Gestión de usuarios (solo Admin) |
| `SettingsView` | Configuración de impuestos, descuentos |

#### Design System / Paleta

- **Tema:** Oscuro profesional (Dark Mode) con acentos en ámbar/naranja (**cafetería**)
- **Tipografía:** Inter / Segoe UI Variable
- **Colores:** `#1A1A2E` (fondo), `#16213E` (sidebar), `#F4A261` (acento primario), `#2EC4B6` (acento secundario)
- **Iconos:** Material Design Icons (paquete NuGet `MaterialDesignThemes`)
- **Gráficas:** `LiveCharts2` para WPF (barras, líneas, pastel)

#### ViewModels Clave

```
ViewModels/
├── MainViewModel.cs        ← Navegación y estado global
├── LoginViewModel.cs       ← Autenticación
├── DashboardViewModel.cs   ← KPIs y resumen
├── POSViewModel.cs         ← Lógica del carrito de compras
├── InventoryViewModel.cs   ← Gestión de stock
├── SalesHistoryViewModel.cs
├── ReportsViewModel.cs     ← Datos para gráficas
└── ProductsViewModel.cs    ← CRUD de productos
```

---

## Paquetes NuGet Requeridos

| Paquete | Uso |
|---------|-----|
| `Microsoft.Extensions.DependencyInjection` | Inyección de dependencias |
| `CommunityToolkit.Mvvm` | Base para ViewModels (ObservableObject, RelayCommand) |
| `MaterialDesignThemes` | Controles y estilos Material Design para WPF |
| `LiveChartsCore.SkiaSharpView.WPF` | Gráficas para reportes/dashboard |

---

## Flujo de Inyección de Dependencias

```
App.xaml.cs
└── ServiceCollection
    ├── AddSingleton<IProductRepository, MockProductRepository>()
    ├── AddSingleton<ISaleRepository, MockSaleRepository>()
    ├── AddSingleton<IInventoryRepository, MockInventoryRepository>()
    ├── ... (todos los repositorios mock)
    ├── AddTransient<SaleService>()
    ├── AddTransient<InventoryService>()
    └── AddTransient<POSViewModel>()   ← ViewModels registrados también
```

> **Nota:** En el futuro, solo se cambia `MockProductRepository` → `SqlProductRepository` en este único archivo y el sistema funciona completo con base de datos real.

---

## Preguntas Abiertas

> [!IMPORTANT]
> **¿Qué versión de .NET usar?** Se recomienda **.NET 8** (LTS) con WPF. ¿Tienes alguna restricción?

> [!IMPORTANT]
> **¿El módulo de reportes necesita exportación a PDF/Excel?** Esto requeriría paquetes adicionales. ¿Lo incluimos en esta fase?

> [!NOTE]
> **Idioma de la UI:** ¿La interfaz debe estar en **Español** o **Inglés**? (Los modelos y código estarán en inglés, la UI puede ser en español).

> [!NOTE]
> **Módulo de impuesto:** ¿Se necesita IVA específico (ej. 18% Perú, 16% México, 19% Colombia)? ¿O un valor configurable?

---

## Plan de Verificación

1. Compilar la solución completa sin errores
2. Verificar que la aplicación inicia y muestra el `LoginView`
3. Navegar por todas las vistas del sidebar
4. Realizar una venta completa en el módulo POS (agregar productos, aplicar descuento, confirmar)
5. Verificar que el stock se descuenta en inventario
6. Verificar que los reportes muestran gráficas con datos mock
7. Verificar que el navegador de ventanas (MainViewModel) funciona correctamente
