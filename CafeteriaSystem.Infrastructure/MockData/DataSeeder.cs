using CafeteriaSystem.Application.Services;
using CafeteriaSystem.Domain.Entities;

namespace CafeteriaSystem.Infrastructure.MockData;

public static class DataSeeder
{
    public static List<TaxRate> TaxRates() =>
    [
        new() { Id = 1, Name = "ITBIS 18%", Rate = 0.18m, IsDefault = true, IsActive = true },
        new() { Id = 2, Name = "Exento",    Rate = 0.00m, IsDefault = false, IsActive = true },
    ];

    public static List<Role> Roles() =>
    [
        new() { Id = 1, Name = "Administrador", Description = "Acceso total al sistema",
            CanManageProducts=true, CanManageUsers=true, CanViewReports=true,
            CanManagePurchases=true, CanOpenCloseCash=true, CanApplyDiscounts=true, CanCancelSales=true },
        new() { Id = 2, Name = "Cajero", Description = "Ventas y caja",
            CanManageProducts=false, CanManageUsers=false, CanViewReports=false,
            CanManagePurchases=false, CanOpenCloseCash=true, CanApplyDiscounts=true, CanCancelSales=false },
        new() { Id = 3, Name = "Supervisor", Description = "Reportes y gestión",
            CanManageProducts=true, CanManageUsers=false, CanViewReports=true,
            CanManagePurchases=true, CanOpenCloseCash=true, CanApplyDiscounts=true, CanCancelSales=true },
    ];

    public static List<User> Users()
    {
        var roles = Roles();
        return
        [
            new() { Id=1, Username="admin",    PasswordHash=AuthService.HashPassword("admin123"),
                    FullName="Administrador General", Email="admin@cafeteria.do",
                    RoleId=1, Role=roles[0], IsActive=true, AvatarColor="#E8A87C" },
            new() { Id=2, Username="cajero",   PasswordHash=AuthService.HashPassword("cajero123"),
                    FullName="María Pérez",  Email="maria@cafeteria.do",
                    RoleId=2, Role=roles[1], IsActive=true, AvatarColor="#7C5CBF" },
            new() { Id=3, Username="supervisor", PasswordHash=AuthService.HashPassword("super123"),
                    FullName="Carlos López", Email="carlos@cafeteria.do",
                    RoleId=3, Role=roles[2], IsActive=true, AvatarColor="#4FC3F7" },
        ];
    }

    public static List<Category> Categories() =>
    [
        new() { Id=1, Name="Bebidas Frías",      Color="#4FC3F7", Icon="Cup",      IsActive=true },
        new() { Id=2, Name="Bebidas Calientes",  Color="#E8A87C", Icon="Coffee",   IsActive=true },
        new() { Id=3, Name="Comidas Rápidas",    Color="#F06292", Icon="Hamburger",IsActive=true },
        new() { Id=4, Name="Postres",            Color="#AB47BC", Icon="Cake",     IsActive=true },
        new() { Id=5, Name="Snacks",             Color="#66BB6A", Icon="Cookie",   IsActive=true },
    ];

    public static List<Product> Products()
    {
        var cats = Categories();
        var tax18 = TaxRates()[0];
        var taxFree = TaxRates()[1];
        var list = new List<Product>
        {
            // Bebidas Frías (CatId=1)
            new(){Id=1, Name="Jugo de Naranja",    CategoryId=1,Category=cats[0],TaxRateId=1,TaxRate=tax18,Price=120,Cost=45,CurrentStock=50,MinimumStock=10,Unit="Und",IsActive=true},
            new(){Id=2, Name="Jugo de Chinola",    CategoryId=1,Category=cats[0],TaxRateId=1,TaxRate=tax18,Price=110,Cost=40,CurrentStock=40,MinimumStock=10,Unit="Und",IsActive=true},
            new(){Id=3, Name="Limonada Natural",   CategoryId=1,Category=cats[0],TaxRateId=1,TaxRate=tax18,Price=130,Cost=50,CurrentStock=35,MinimumStock=8,Unit="Und",IsActive=true},
            new(){Id=4, Name="Refresco Pepsi",     CategoryId=1,Category=cats[0],TaxRateId=1,TaxRate=tax18,Price=80, Cost=35,CurrentStock=100,MinimumStock=20,Unit="Und",IsActive=true},
            new(){Id=5, Name="Agua Botella 500ml", CategoryId=1,Category=cats[0],TaxRateId=2,TaxRate=taxFree,Price=50,Cost=20,CurrentStock=200,MinimumStock=30,Unit="Und",IsActive=true},
            new(){Id=6, Name="Batido de Fresa",   CategoryId=1,Category=cats[0],TaxRateId=1,TaxRate=tax18,Price=180,Cost=70,CurrentStock=25,MinimumStock=5,Unit="Und",IsActive=true},
            new(){Id=7, Name="Batido de Mango",   CategoryId=1,Category=cats[0],TaxRateId=1,TaxRate=tax18,Price=180,Cost=70,CurrentStock=25,MinimumStock=5,Unit="Und",IsActive=true},
            new(){Id=8, Name="Nestea Limón",      CategoryId=1,Category=cats[0],TaxRateId=1,TaxRate=tax18,Price=90, Cost=40,CurrentStock=60,MinimumStock=10,Unit="Und",IsActive=true},
            new(){Id=9, Name="Gatorade Azul",     CategoryId=1,Category=cats[0],TaxRateId=1,TaxRate=tax18,Price=100,Cost=55,CurrentStock=45,MinimumStock=10,Unit="Und",IsActive=true},
            new(){Id=10,Name="Agua de Coco",      CategoryId=1,Category=cats[0],TaxRateId=2,TaxRate=taxFree,Price=150,Cost=60,CurrentStock=20,MinimumStock=5,Unit="Und",IsActive=true},
            // Bebidas Calientes (CatId=2)
            new(){Id=11,Name="Café Americano",    CategoryId=2,Category=cats[1],TaxRateId=1,TaxRate=tax18,Price=100,Cost=35,CurrentStock=999,MinimumStock=5,Unit="Taza",IsActive=true},
            new(){Id=12,Name="Café con Leche",    CategoryId=2,Category=cats[1],TaxRateId=1,TaxRate=tax18,Price=130,Cost=45,CurrentStock=999,MinimumStock=5,Unit="Taza",IsActive=true},
            new(){Id=13,Name="Capuchino",         CategoryId=2,Category=cats[1],TaxRateId=1,TaxRate=tax18,Price=180,Cost=60,CurrentStock=999,MinimumStock=5,Unit="Taza",IsActive=true},
            new(){Id=14,Name="Expreso Doble",     CategoryId=2,Category=cats[1],TaxRateId=1,TaxRate=tax18,Price=120,Cost=40,CurrentStock=999,MinimumStock=5,Unit="Taza",IsActive=true},
            new(){Id=15,Name="Chocolate Caliente",CategoryId=2,Category=cats[1],TaxRateId=1,TaxRate=tax18,Price=150,Cost=55,CurrentStock=999,MinimumStock=5,Unit="Taza",IsActive=true},
            new(){Id=16,Name="Té Verde",          CategoryId=2,Category=cats[1],TaxRateId=1,TaxRate=tax18,Price=90, Cost=30,CurrentStock=999,MinimumStock=5,Unit="Taza",IsActive=true},
            new(){Id=17,Name="Té de Manzanilla",  CategoryId=2,Category=cats[1],TaxRateId=1,TaxRate=tax18,Price=80, Cost=25,CurrentStock=999,MinimumStock=5,Unit="Taza",IsActive=true},
            new(){Id=18,Name="Matcha Latte",      CategoryId=2,Category=cats[1],TaxRateId=1,TaxRate=tax18,Price=220,Cost=80,CurrentStock=999,MinimumStock=5,Unit="Taza",IsActive=true},
            new(){Id=19,Name="Café Frío",         CategoryId=2,Category=cats[1],TaxRateId=1,TaxRate=tax18,Price=160,Cost=55,CurrentStock=30,MinimumStock=5,Unit="Und",IsActive=true},
            new(){Id=20,Name="Hot Chocolate XL",  CategoryId=2,Category=cats[1],TaxRateId=1,TaxRate=tax18,Price=200,Cost=70,CurrentStock=999,MinimumStock=5,Unit="Taza",IsActive=true},
            // Comidas Rápidas (CatId=3)
            new(){Id=21,Name="Sandwich de Pollo", CategoryId=3,Category=cats[2],TaxRateId=1,TaxRate=tax18,Price=350,Cost=140,CurrentStock=30,MinimumStock=5,Unit="Und",IsActive=true},
            new(){Id=22,Name="Sandwich de Jamón", CategoryId=3,Category=cats[2],TaxRateId=1,TaxRate=tax18,Price=280,Cost=110,CurrentStock=25,MinimumStock=5,Unit="Und",IsActive=true},
            new(){Id=23,Name="Burger Clásica",    CategoryId=3,Category=cats[2],TaxRateId=1,TaxRate=tax18,Price=420,Cost=165,CurrentStock=20,MinimumStock=5,Unit="Und",IsActive=true},
            new(){Id=24,Name="Hot Dog",           CategoryId=3,Category=cats[2],TaxRateId=1,TaxRate=tax18,Price=250,Cost=95,CurrentStock=30,MinimumStock=5,Unit="Und",IsActive=true},
            new(){Id=25,Name="Empanada de Pollo", CategoryId=3,Category=cats[2],TaxRateId=1,TaxRate=tax18,Price=150,Cost=55,CurrentStock=40,MinimumStock=8,Unit="Und",IsActive=true},
            new(){Id=26,Name="Empanada de Res",   CategoryId=3,Category=cats[2],TaxRateId=1,TaxRate=tax18,Price=160,Cost=60,CurrentStock=35,MinimumStock=8,Unit="Und",IsActive=true},
            new(){Id=27,Name="Wrap de Vegetales", CategoryId=3,Category=cats[2],TaxRateId=1,TaxRate=tax18,Price=320,Cost=125,CurrentStock=15,MinimumStock=3,Unit="Und",IsActive=true},
            new(){Id=28,Name="Pizza Personal",    CategoryId=3,Category=cats[2],TaxRateId=1,TaxRate=tax18,Price=450,Cost=175,CurrentStock=15,MinimumStock=3,Unit="Und",IsActive=true},
            new(){Id=29,Name="Tostado Mixto",     CategoryId=3,Category=cats[2],TaxRateId=1,TaxRate=tax18,Price=220,Cost=85,CurrentStock=3,MinimumStock=5,Unit="Und",IsActive=true},
            new(){Id=30,Name="Shawarma Pollo",    CategoryId=3,Category=cats[2],TaxRateId=1,TaxRate=tax18,Price=380,Cost=148,CurrentStock=12,MinimumStock=4,Unit="Und",IsActive=true},
            // Postres (CatId=4)
            new(){Id=31,Name="Brownie Chocolate", CategoryId=4,Category=cats[3],TaxRateId=1,TaxRate=tax18,Price=180,Cost=65,CurrentStock=20,MinimumStock=5,Unit="Und",IsActive=true},
            new(){Id=32,Name="Cheesecake Fresa",  CategoryId=4,Category=cats[3],TaxRateId=1,TaxRate=tax18,Price=220,Cost=80,CurrentStock=15,MinimumStock=3,Unit="Porción",IsActive=true},
            new(){Id=33,Name="Flan de Coco",      CategoryId=4,Category=cats[3],TaxRateId=1,TaxRate=tax18,Price=160,Cost=58,CurrentStock=2,MinimumStock=5,Unit="Und",IsActive=true},
            new(){Id=34,Name="Tres Leches",       CategoryId=4,Category=cats[3],TaxRateId=1,TaxRate=tax18,Price=200,Cost=72,CurrentStock=12,MinimumStock=3,Unit="Porción",IsActive=true},
            new(){Id=35,Name="Galletas Choco",    CategoryId=4,Category=cats[3],TaxRateId=1,TaxRate=tax18,Price=90, Cost=30,CurrentStock=50,MinimumStock=10,Unit="Und",IsActive=true},
            new(){Id=36,Name="Dona Glaseada",     CategoryId=4,Category=cats[3],TaxRateId=1,TaxRate=tax18,Price=120,Cost=40,CurrentStock=18,MinimumStock=5,Unit="Und",IsActive=true},
            new(){Id=37,Name="Croissant",         CategoryId=4,Category=cats[3],TaxRateId=1,TaxRate=tax18,Price=140,Cost=50,CurrentStock=4,MinimumStock=5,Unit="Und",IsActive=true},
            new(){Id=38,Name="Muffin Arándano",   CategoryId=4,Category=cats[3],TaxRateId=1,TaxRate=tax18,Price=130,Cost=45,CurrentStock=16,MinimumStock=5,Unit="Und",IsActive=true},
            new(){Id=39,Name="Churros con Choco", CategoryId=4,Category=cats[3],TaxRateId=1,TaxRate=tax18,Price=150,Cost=52,CurrentStock=20,MinimumStock=5,Unit="Porción",IsActive=true},
            new(){Id=40,Name="Pan de Dulce",      CategoryId=4,Category=cats[3],TaxRateId=1,TaxRate=tax18,Price=80, Cost=25,CurrentStock=30,MinimumStock=8,Unit="Und",IsActive=true},
            // Snacks (CatId=5)
            new(){Id=41,Name="Papas Fritas",      CategoryId=5,Category=cats[4],TaxRateId=1,TaxRate=tax18,Price=150,Cost=55,CurrentStock=40,MinimumStock=8,Unit="Porción",IsActive=true},
            new(){Id=42,Name="Nachos con Queso",  CategoryId=5,Category=cats[4],TaxRateId=1,TaxRate=tax18,Price=180,Cost=65,CurrentStock=30,MinimumStock=5,Unit="Porción",IsActive=true},
            new(){Id=43,Name="Almendras Saladas", CategoryId=5,Category=cats[4],TaxRateId=1,TaxRate=tax18,Price=120,Cost=55,CurrentStock=25,MinimumStock=5,Unit="Und",IsActive=true},
            new(){Id=44,Name="Granola Bar",       CategoryId=5,Category=cats[4],TaxRateId=1,TaxRate=tax18,Price=90, Cost=40,CurrentStock=35,MinimumStock=8,Unit="Und",IsActive=true},
            new(){Id=45,Name="Frutas Mixtas",     CategoryId=5,Category=cats[4],TaxRateId=2,TaxRate=taxFree,Price=160,Cost=65,CurrentStock=15,MinimumStock=3,Unit="Porción",IsActive=true},
            new(){Id=46,Name="Maní Tostado",      CategoryId=5,Category=cats[4],TaxRateId=1,TaxRate=tax18,Price=70, Cost=28,CurrentStock=50,MinimumStock=10,Unit="Und",IsActive=true},
            new(){Id=47,Name="Chips Plátano",     CategoryId=5,Category=cats[4],TaxRateId=1,TaxRate=tax18,Price=80, Cost=32,CurrentStock=40,MinimumStock=10,Unit="Und",IsActive=true},
            new(){Id=48,Name="Palomitas Caramelo",CategoryId=5,Category=cats[4],TaxRateId=1,TaxRate=tax18,Price=100,Cost=38,CurrentStock=0,MinimumStock=5,Unit="Und",IsActive=true},
            new(){Id=49,Name="Mix Semillas",      CategoryId=5,Category=cats[4],TaxRateId=1,TaxRate=tax18,Price=110,Cost=48,CurrentStock=22,MinimumStock=5,Unit="Und",IsActive=true},
            new(){Id=50,Name="Yogurt con Granola",CategoryId=5,Category=cats[4],TaxRateId=1,TaxRate=tax18,Price=150,Cost=58,CurrentStock=10,MinimumStock=3,Unit="Und",IsActive=true},
        };
        return list;
    }

    public static List<Customer> Customers() =>
    [
        new(){Id=1,Name="Cliente General",  Email="",                  Phone="",           IsActive=true,TotalPurchases=0},
        new(){Id=2,Name="Ana Rodríguez",    Email="ana@gmail.com",     Phone="809-555-0101",IsActive=true,TotalPurchases=4800},
        new(){Id=3,Name="Juan Méndez",      Email="juan@gmail.com",    Phone="809-555-0102",IsActive=true,TotalPurchases=12500},
        new(){Id=4,Name="Laura Sánchez",    Email="laura@gmail.com",   Phone="809-555-0103",IsActive=true,TotalPurchases=8900},
        new(){Id=5,Name="Pedro García",     Email="pedro@gmail.com",   Phone="809-555-0104",IsActive=true,TotalPurchases=3200},
        new(){Id=6,Name="Empresa ABC S.R.L.",Email="compras@abc.do",   Phone="809-555-0200",RNC="1-31-00001-1",IsActive=true,TotalPurchases=55000},
        new(){Id=7,Name="Colegio San Pablo",Email="admin@sanpablo.do", Phone="809-555-0201",RNC="1-31-00002-2",IsActive=true,TotalPurchases=32000},
    ];

    public static List<Supplier> Suppliers() =>
    [
        new(){Id=1,Name="Distribuidora El Cafeto",ContactName="Roberto Díaz",  Email="rdíaz@cafeto.do",  Phone="809-555-1001",IsActive=true},
        new(){Id=2,Name="Bebidas & Más S.A.",     ContactName="Patricia Vega",  Email="pvega@bebidas.do", Phone="809-555-1002",IsActive=true},
        new(){Id=3,Name="Panadería La Espiga",    ContactName="Miguel Torres",  Email="mtorres@espiga.do",Phone="809-555-1003",IsActive=true},
        new(){Id=4,Name="Friasa Lácteos",         ContactName="Carmen Núñez",   Email="cnunez@friasa.do", Phone="809-555-1004",IsActive=true},
    ];

    public static List<Discount> Discounts() =>
    [
        new(){Id=1,Name="Descuento Empleado",Code="EMP15",Type=DiscountType.Porcentaje,Value=15,Scope=DiscountScope.Todos,IsActive=true},
        new(){Id=2,Name="Happy Hour Bebidas",Code="HAPPY",Type=DiscountType.Porcentaje,Value=10,Scope=DiscountScope.Categoria,CategoryId=1,IsActive=true},
        new(){Id=3,Name="Promo Café",        Code="CAFE20",Type=DiscountType.Porcentaje,Value=20,Scope=DiscountScope.Categoria,CategoryId=2,IsActive=true},
        new(){Id=4,Name="Descuento Manual",  Code="MANUAL",Type=DiscountType.Porcentaje,Value=5, Scope=DiscountScope.Todos,IsActive=true},
    ];

    public static List<Sale> HistoricalSales()
    {
        var sales = new List<Sale>();
        var rnd = new Random(42);
        var products = Products();
        var customers = Customers();
        int id = 1;

        for (int daysBack = 30; daysBack >= 0; daysBack--)
        {
            var date = DateTime.Today.AddDays(-daysBack);
            int ordersPerDay = daysBack == 0 ? 3 : rnd.Next(5, 15);
            for (int o = 0; o < ordersPerDay; o++)
            {
                var details = new List<SaleDetail>();
                int itemCount = rnd.Next(1, 5);
                for (int i = 0; i < itemCount; i++)
                {
                    var p = products[rnd.Next(products.Count)];
                    int qty = rnd.Next(1, 4);
                    details.Add(new SaleDetail
                    {
                        Id = id * 10 + i,
                        ProductId = p.Id,
                        ProductName = p.Name,
                        UnitPrice = p.Price,
                        Quantity = qty,
                        TaxPercent = (p.TaxRate?.Rate ?? 0.18m) * 100,
                    });
                }
                decimal sub = details.Sum(d => d.Subtotal);
                decimal tax = details.Sum(d => d.TaxAmount);
                decimal total = sub + tax;
                sales.Add(new Sale
                {
                    Id = id++,
                    SaleNumber = $"V-{id:D6}",
                    SaleDate = date.AddHours(rnd.Next(7, 20)).AddMinutes(rnd.Next(60)),
                    UserId = 2,
                    CustomerId = rnd.Next(1, 3) == 1 ? rnd.Next(1, 7) : 1,
                    PaymentMethod = (PaymentMethod)rnd.Next(0, 3),
                    Subtotal = sub,
                    TaxAmount = tax,
                    Total = total,
                    AmountPaid = total,
                    Status = SaleStatus.Completada,
                    Details = details,
                });
            }
        }
        return sales;
    }
}
