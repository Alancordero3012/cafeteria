using CafeteriaSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaSystem.Infrastructure.Data;

/// <summary>
/// DbContext de EF Core que mapea las entidades de dominio al esquema
/// sistema_ventasbd en SQL Server.
/// Los nombres de tabla y columna respetan exactamente el esquema SQL provisto.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    // ── DbSets ───────────────────────────────────────────────────────────────
    public DbSet<Category> Categorias => Set<Category>();
    public DbSet<Supplier> Proveedores => Set<Supplier>();
    public DbSet<Product> Productos => Set<Product>();
    public DbSet<Customer> Clientes => Set<Customer>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permisos => Set<Permission>();
    public DbSet<User> Usuarios => Set<User>();
    public DbSet<PaymentMethodEntity> MetodosPago => Set<PaymentMethodEntity>();
    public DbSet<Sale> Ventas => Set<Sale>();
    public DbSet<SaleDetail> DetalleVentas => Set<SaleDetail>();
    public DbSet<Purchase> Compras => Set<Purchase>();
    public DbSet<PurchaseDetail> DetalleCompras => Set<PurchaseDetail>();
    public DbSet<KardexEntry> Kardex => Set<KardexEntry>();
    public DbSet<CashRegister> ArqueosCaja => Set<CashRegister>();
    public DbSet<CompanyConfig> ConfiguracionEmpresa => Set<CompanyConfig>();
    public DbSet<ActivityLog> Logs => Set<ActivityLog>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        // ── Categorias ───────────────────────────────────────────────────────
        mb.Entity<Category>(e =>
        {
            e.ToTable("Categorias");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id_categoria");
            e.Property(x => x.Name).HasColumnName("nombre").HasMaxLength(100).IsRequired();
            e.Property(x => x.Description).HasColumnName("descripcion");
            e.Property(x => x.IsActive).HasColumnName("activo").HasDefaultValue(true);
            e.Property(x => x.CreatedAt).HasColumnName("creado_en").HasDefaultValueSql("GETDATE()");
            e.Property(x => x.Color).HasColumnName("color").HasMaxLength(20).HasDefaultValue("#E8A87C");
            // Propiedades de UI sin columna en BD:
            e.Ignore(x => x.Icon);
            e.Ignore(x => x.Products);
        });

        // ── Proveedores ──────────────────────────────────────────────────────
        mb.Entity<Supplier>(e =>
        {
            e.ToTable("Proveedores");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id_proveedor");
            e.Property(x => x.Name).HasColumnName("nombre").HasMaxLength(150).IsRequired();
            e.Property(x => x.ContactName).HasColumnName("contacto").HasMaxLength(100);
            e.Property(x => x.Email).HasColumnName("email").HasMaxLength(120);
            e.Property(x => x.Phone).HasColumnName("telefono").HasMaxLength(20);
            e.Property(x => x.Address).HasColumnName("direccion");
            e.Property(x => x.RNC).HasColumnName("rnc").HasMaxLength(20);
            e.Property(x => x.IsActive).HasColumnName("activo").HasDefaultValue(true);
            e.Property(x => x.CreatedAt).HasColumnName("creado_en").HasDefaultValueSql("GETDATE()");
            e.Ignore(x => x.Purchases); // Navigation: configurada desde Purchase
        });

        // ── Productos ────────────────────────────────────────────────────────
        mb.Entity<Product>(e =>
        {
            e.ToTable("Productos");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id_producto");
            e.Property(x => x.CategoryId).HasColumnName("id_categoria");
            e.Property(x => x.SupplierId).HasColumnName("id_proveedor");
            e.Property(x => x.Barcode).HasColumnName("codigo").HasMaxLength(50);
            e.Property(x => x.Name).HasColumnName("nombre").HasMaxLength(200).IsRequired();
            e.Property(x => x.Description).HasColumnName("descripcion");
            e.Property(x => x.Unit).HasColumnName("unidad_medida").HasMaxLength(20);
            e.Property(x => x.Cost).HasColumnName("precio_compra").HasColumnType("decimal(10,2)");
            e.Property(x => x.Price).HasColumnName("precio_venta").HasColumnType("decimal(10,2)");
            e.Property(x => x.CurrentStock).HasColumnName("stock_actual").HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            e.Property(x => x.MinimumStock).HasColumnName("stock_minimo").HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            e.Property(x => x.AplicaItbis).HasColumnName("aplica_itbis").HasDefaultValue(true);
            e.Property(x => x.IsActive).HasColumnName("activo").HasDefaultValue(true);
            e.Property(x => x.CreatedAt).HasColumnName("creado_en").HasDefaultValueSql("GETDATE()");
            e.Property(x => x.UpdatedAt).HasColumnName("actualizado_en");
            // ImageUrl no existe en BD → se ignora
            e.Ignore(x => x.ImageUrl);
            e.Ignore(x => x.PriceWithTax);
            e.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId);
            e.HasOne(x => x.Supplier).WithMany().HasForeignKey(x => x.SupplierId);
        });

        // ── Clientes ─────────────────────────────────────────────────────────
        mb.Entity<Customer>(e =>
        {
            e.ToTable("Clientes");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id_cliente");
            e.Property(x => x.Cedula).HasColumnName("cedula_rnc").HasMaxLength(20);
            e.Property(x => x.Name).HasColumnName("nombre").HasMaxLength(150).IsRequired();
            e.Property(x => x.Phone).HasColumnName("telefono").HasMaxLength(20);
            e.Property(x => x.Email).HasColumnName("email").HasMaxLength(120);
            e.Property(x => x.Address).HasColumnName("direccion");
            e.Property(x => x.CreditLimit).HasColumnName("limite_credito").HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            e.Property(x => x.PendingBalance).HasColumnName("saldo_pendiente").HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            e.Property(x => x.IsActive).HasColumnName("activo").HasDefaultValue(true);
            e.Property(x => x.CreatedAt).HasColumnName("creado_en").HasDefaultValueSql("GETDATE()");
            e.Ignore(x => x.RNC);
        });

        // ── Roles ────────────────────────────────────────────────────────────
        mb.Entity<Role>(e =>
        {
            e.ToTable("Roles");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id_rol");
            e.Property(x => x.Name).HasColumnName("nombre").HasMaxLength(80).IsRequired();
            e.Property(x => x.Description).HasColumnName("descripcion");
            // Las propiedades de permisos son calculadas → ignoradas
            e.Ignore(x => x.CanManageProducts);
            e.Ignore(x => x.CanManageUsers);
            e.Ignore(x => x.CanViewReports);
            e.Ignore(x => x.CanManagePurchases);
            e.Ignore(x => x.CanOpenCloseCash);
            e.Ignore(x => x.CanApplyDiscounts);
            e.Ignore(x => x.CanCancelSales);
        });

        // ── Permisos ─────────────────────────────────────────────────────────
        mb.Entity<Permission>(e =>
        {
            e.ToTable("Permisos");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id_permiso");
            e.Property(x => x.RoleId).HasColumnName("id_rol");
            e.Property(x => x.Module).HasColumnName("modulo").HasMaxLength(80);
            e.Property(x => x.CanView).HasColumnName("puede_ver").HasDefaultValue(false);
            e.Property(x => x.CanCreate).HasColumnName("puede_crear").HasDefaultValue(false);
            e.Property(x => x.CanEdit).HasColumnName("puede_editar").HasDefaultValue(false);
            e.Property(x => x.CanDelete).HasColumnName("puede_eliminar").HasDefaultValue(false);
            e.HasOne(x => x.Role).WithMany(r => r.Permissions).HasForeignKey(x => x.RoleId);
        });

        // ── Usuarios ─────────────────────────────────────────────────────────
        mb.Entity<User>(e =>
        {
            e.ToTable("Usuarios");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id_usuario");
            e.Property(x => x.RoleId).HasColumnName("id_rol");
            e.Property(x => x.FullName).HasColumnName("nombre").HasMaxLength(150).IsRequired();
            e.Property(x => x.Username).HasColumnName("usuario").HasMaxLength(60).IsRequired();
            e.Property(x => x.PasswordHash).HasColumnName("password_hash").HasMaxLength(255).IsRequired();
            e.Property(x => x.Email).HasColumnName("email").HasMaxLength(120);
            e.Property(x => x.IsActive).HasColumnName("activo").HasDefaultValue(true);
            e.Property(x => x.LastLogin).HasColumnName("ultimo_acceso");
            e.Property(x => x.CreatedAt).HasColumnName("creado_en").HasDefaultValueSql("GETDATE()");
            // Propiedades solo de UI → ignoradas en BD
            e.Ignore(x => x.AvatarColor);
            e.Ignore(x => x.Phone);
            e.Ignore(x => x.Initials);
            e.HasOne(x => x.Role).WithMany(r => r.Users).HasForeignKey(x => x.RoleId);
        });

        // ── Metodos_Pago ─────────────────────────────────────────────────────
        mb.Entity<PaymentMethodEntity>(e =>
        {
            e.ToTable("Metodos_Pago");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id_metodo");
            e.Property(x => x.Name).HasColumnName("nombre").HasMaxLength(80).IsRequired();
            e.Property(x => x.IsActive).HasColumnName("activo").HasDefaultValue(true);
        });

        // ── Ventas ───────────────────────────────────────────────────────────
        mb.Entity<Sale>(e =>
        {
            e.ToTable("Ventas");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id_venta");
            e.Property(x => x.UserId).HasColumnName("id_usuario");
            e.Property(x => x.CustomerId).HasColumnName("id_cliente");
            e.Property(x => x.PaymentMethodId).HasColumnName("id_metodo");
            e.Property(x => x.SaleNumber).HasColumnName("numero_factura").HasMaxLength(30);
            e.Property(x => x.SaleDate).HasColumnName("fecha").HasDefaultValueSql("GETDATE()");
            e.Property(x => x.Subtotal).HasColumnName("subtotal").HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            e.Property(x => x.DiscountAmount).HasColumnName("descuento").HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            e.Property(x => x.TaxAmount).HasColumnName("itbis").HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            e.Property(x => x.Total).HasColumnName("total").HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            e.Property(x => x.AmountPaid).HasColumnName("monto_pagado").HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            e.Property(x => x.Change).HasColumnName("cambio").HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            e.Property(x => x.Notes).HasColumnName("observaciones");
            e.Property(x => x.Status).HasColumnName("estado").HasConversion<string>().HasMaxLength(20);
            e.Ignore(x => x.PaymentMethod); // propiedad calculada
            e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(x => x.Customer).WithMany(c => c.Sales).HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(x => x.PaymentMethodEntity).WithMany(p => p.Sales).HasForeignKey(x => x.PaymentMethodId).OnDelete(DeleteBehavior.NoAction);
            e.HasMany(x => x.Details).WithOne(d => d.Sale).HasForeignKey(d => d.SaleId).OnDelete(DeleteBehavior.NoAction);
        });

        // ── Detalle_Ventas ───────────────────────────────────────────────────
        mb.Entity<SaleDetail>(e =>
        {
            e.ToTable("Detalle_Ventas");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id_detalle");
            e.Property(x => x.SaleId).HasColumnName("id_venta");
            e.Property(x => x.ProductId).HasColumnName("id_producto");
            e.Property(x => x.Quantity).HasColumnName("cantidad").HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            e.Property(x => x.UnitPrice).HasColumnName("precio_unitario").HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            e.Property(x => x.Discount).HasColumnName("descuento").HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            e.Property(x => x.TaxAmount).HasColumnName("itbis").HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            e.Property(x => x.Subtotal).HasColumnName("subtotal").HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            // Propiedades de UI, no existen en la BD:
            e.Ignore(x => x.ProductName);
            e.Ignore(x => x.Total);
            e.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.NoAction);
            // Relación con Sale configurada desde el lado de Sale (HasMany→WithOne→HasForeignKey)
        });

        // ── Compras ──────────────────────────────────────────────────────────
        mb.Entity<Purchase>(e =>
        {
            e.ToTable("Compras");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id_compra");
            e.Property(x => x.SupplierId).HasColumnName("id_proveedor");
            e.Property(x => x.UserId).HasColumnName("id_usuario");
            e.Property(x => x.PurchaseNumber).HasColumnName("numero_doc").HasMaxLength(50);
            e.Property(x => x.Date).HasColumnName("fecha").HasDefaultValueSql("GETDATE()");
            e.Property(x => x.Subtotal).HasColumnName("subtotal").HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            e.Property(x => x.TaxAmount).HasColumnName("itbis").HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            e.Property(x => x.Total).HasColumnName("total").HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            e.Property(x => x.Status).HasColumnName("estado").HasConversion<string>().HasMaxLength(20);
            e.Ignore(x => x.Notes); // no existe en BD
            e.HasOne(x => x.Supplier).WithMany(s => s.Purchases).HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.NoAction);
            e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
            e.HasMany(x => x.Details).WithOne(d => d.Purchase).HasForeignKey(d => d.PurchaseId).OnDelete(DeleteBehavior.NoAction);
        });

        // ── Detalle_Compras ──────────────────────────────────────────────────
        mb.Entity<PurchaseDetail>(e =>
        {
            e.ToTable("Detalle_Compras");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id_detalle");
            e.Property(x => x.PurchaseId).HasColumnName("id_compra");
            e.Property(x => x.ProductId).HasColumnName("id_producto");
            e.Property(x => x.Quantity).HasColumnName("cantidad").HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            e.Property(x => x.UnitCost).HasColumnName("precio_unitario").HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            e.Property(x => x.TaxAmount).HasColumnName("itbis").HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            e.Property(x => x.Subtotal).HasColumnName("subtotal").HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            // Propiedades de UI, no existen en la BD:
            e.Ignore(x => x.ProductName);
            e.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.NoAction);
            // Relación con Purchase configurada desde el lado de Purchase (HasMany→WithOne→HasForeignKey)
        });

        // ── Kardex ───────────────────────────────────────────────────────────
        mb.Entity<KardexEntry>(e =>
        {
            e.ToTable("Kardex");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id_movimiento");
            e.Property(x => x.ProductId).HasColumnName("id_producto");
            e.Property(x => x.UserId).HasColumnName("id_usuario");
            e.Property(x => x.MovementType).HasColumnName("tipo_movimiento").HasMaxLength(50);
            e.Property(x => x.Quantity).HasColumnName("cantidad").HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            e.Property(x => x.StockBefore).HasColumnName("stock_anterior").HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            e.Property(x => x.StockAfter).HasColumnName("stock_nuevo").HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            e.Property(x => x.Date).HasColumnName("fecha").HasDefaultValueSql("GETDATE()");
            // Campos de UI no en BD
            e.Ignore(x => x.Type);
            e.Ignore(x => x.Reason);
            e.Ignore(x => x.ReferenceNumber);
            e.Ignore(x => x.UnitCost);
            e.HasOne(x => x.Product).WithMany().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.NoAction);
        });

        // ── Arqueos_Caja ─────────────────────────────────────────────────────
        mb.Entity<CashRegister>(e =>
        {
            e.ToTable("Arqueos_Caja");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id_arqueo");
            e.Property(x => x.UserId).HasColumnName("id_usuario");
            e.Property(x => x.OpenDate).HasColumnName("fecha_apertura").HasDefaultValueSql("GETDATE()");
            e.Property(x => x.CloseDate).HasColumnName("fecha_cierre");
            e.Property(x => x.InitialAmount).HasColumnName("monto_apertura").HasColumnType("decimal(10,2)").HasDefaultValue(0m);
            e.Property(x => x.ActualAmount).HasColumnName("monto_real").HasColumnType("decimal(10,2)");
            e.Property(x => x.Status).HasColumnName("estado").HasConversion<string>().HasMaxLength(20);
            // Campos solo de UI
            e.Ignore(x => x.ExpectedAmount);
            e.Ignore(x => x.Notes);
            e.Ignore(x => x.Difference);
            e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
        });

        // ── Configuracion_Empresa ────────────────────────────────────────────
        mb.Entity<CompanyConfig>(e =>
        {
            e.ToTable("Configuracion_Empresa");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id_config");
            e.Property(x => x.Key).HasColumnName("clave").HasMaxLength(80).IsRequired();
            e.Property(x => x.Value).HasColumnName("valor");
        });

        // ── Logs_Actividad ───────────────────────────────────────────────────
        mb.Entity<ActivityLog>(e =>
        {
            e.ToTable("Logs_Actividad");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id_log");
            e.Property(x => x.UserId).HasColumnName("id_usuario");
            e.Property(x => x.Action).HasColumnName("accion").HasMaxLength(100).IsRequired();
            e.Property(x => x.Module).HasColumnName("modulo").HasMaxLength(50);
            e.Property(x => x.Details).HasColumnName("detalles");
            e.Property(x => x.Date).HasColumnName("fecha").HasDefaultValueSql("GETDATE()");
            e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
        });

        // ── Conversor global: NULL → "" para todas las columnas string ──────────
        // Con C# Nullable enable, los 'string' se marcan como IsNullable=false
        // en EF aunque la columna BD sí permita NULL (ej: email, descripcion).
        // Forzamos IsNullable=true + converter para que EF use un lector null-safe.
        foreach (var entityType in mb.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties()
                .Where(p => p.ClrType == typeof(string) && !p.IsKey()))
            {
                property.IsNullable = true;
                property.SetValueConverter(
                    new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<string, string?>(
                        v => string.IsNullOrEmpty(v) ? null : v,   // C# → DB
                        v => v ?? string.Empty));                   // DB → C#
            }
        }
    }
}
