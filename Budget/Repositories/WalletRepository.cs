using Budget.Context;
using Budget.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget.Repositories;

public class WalletRepository : Repository<Wallet>, IWalletRepository
{
    public WalletRepository(BudgetDbContext budgetDbContext) : base(budgetDbContext)
    {
    }


    public IEnumerable<Wallet> GetWalletWithCategories()
    {
        var walletsWithTransactions = DbEntitySet
            .Include(w => w.Categories)!
            .ThenInclude(c => c.Transactions);
        
        return walletsWithTransactions;
    }
}