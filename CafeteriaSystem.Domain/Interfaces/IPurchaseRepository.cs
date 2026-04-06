using CafeteriaSystem.Domain.Entities;

namespace CafeteriaSystem.Domain.Interfaces;

public interface IPurchaseRepository
{
    Task<IEnumerable<Purchase>> GetAllAsync();
    Task<Purchase?> GetByIdAsync(int id);
    Task<Purchase> AddAsync(Purchase purchase);
    Task<Purchase> UpdateAsync(Purchase purchase);
    Task<int> GetNextPurchaseNumberAsync();
}

public interface ICashRegisterRepository
{
    Task<CashRegister?> GetOpenRegisterAsync();
    Task<IEnumerable<CashRegister>> GetAllAsync();
    Task<CashRegister?> GetByIdAsync(int id);
    Task<CashRegister> AddAsync(CashRegister cashRegister);
    Task<CashRegister> UpdateAsync(CashRegister cashRegister);
}

public interface IDiscountRepository
{
    Task<IEnumerable<Discount>> GetAllActiveAsync();
    Task<Discount?> GetByCodeAsync(string code);
    Task<Discount?> GetByIdAsync(int id);
    Task<Discount> AddAsync(Discount discount);
    Task<Discount> UpdateAsync(Discount discount);
}

public interface ITaxRateRepository
{
    Task<IEnumerable<TaxRate>> GetAllAsync();
    Task<TaxRate?> GetDefaultAsync();
    Task<TaxRate> UpdateAsync(TaxRate taxRate);
}
