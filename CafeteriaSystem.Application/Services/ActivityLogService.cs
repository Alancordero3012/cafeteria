using CafeteriaSystem.Domain.Entities;
using CafeteriaSystem.Domain.Interfaces;

namespace CafeteriaSystem.Application.Services;

/// <summary>
/// Servicio centralizado para registrar actividad del sistema en la BD.
/// Nunca lanza excepciones — si falla el log, la operación principal continúa.
/// </summary>
public class ActivityLogService(IActivityLogRepository repository)
{
    public async Task LogAsync(string action, string module, string details, int? userId = null)
    {
        try
        {
            await repository.AddAsync(new ActivityLog
            {
                Action  = action,
                Module  = module,
                Details = details,
                UserId  = userId,
                Date    = DateTime.Now
            });
        }
        catch
        {
            // Silencioso: el log nunca debe interrumpir la operación principal
        }
    }

    // Helpers semánticos
    public Task LoginOk(string username, int userId) =>
        LogAsync("LOGIN_OK", "Auth", $"Usuario '{username}' inició sesión.", userId);

    public Task LoginFail(string username) =>
        LogAsync("LOGIN_FAIL", "Auth", $"Intento de login fallido para '{username}'.");

    public Task SaleCreated(string saleNumber, decimal total, int userId) =>
        LogAsync("VENTA_CREADA", "POS", $"Venta {saleNumber} por RD${total:N2}.", userId);

    public Task ProductCreated(string name, int userId) =>
        LogAsync("PRODUCTO_CREADO", "Productos", $"Producto '{name}' creado.", userId);

    public Task ProductUpdated(string name, int userId) =>
        LogAsync("PRODUCTO_EDITADO", "Productos", $"Producto '{name}' actualizado.", userId);

    public Task CategorySaved(string name, int userId) =>
        LogAsync("CATEGORIA_GUARDADA", "Categorías", $"Categoría '{name}' guardada.", userId);
}
