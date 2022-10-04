using Microsoft.EntityFrameworkCore;
using MVC.Categories;
using MVC.Transactions;

namespace MVC.Database;

public class PlutusDbContext : DbContext
{
    public DbSet<Transaction> Transactions { get; init; } = null!;

    public DbSet<Category> Categories { get; init; } = null!;

    public PlutusDbContext(DbContextOptions<PlutusDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>().HasData(new List<Category>
        {
            new () { Id = 1, Name = "Salary" },
            new () { Id = 2, Name = "Refund" },


            new () { Id = 3, Name = "Food Delivery" },
            new () { Id = 4, Name = "Online Shopping" },
            new () { Id = 5, Name = "Restaurant" },
            new () { Id = 6, Name = "Mutual Funds" },
            new () { Id = 7, Name = "Groceries" },

            new () { Id = 8, Name = "Interest" },
            new () { Id = 9, Name = "Internet" },
            new () { Id = 10, Name = "Prepaid Phone Plan" },


            new () { Id = 11, Name = "Misc Expenses" },
            new () { Id = 12, Name = "Transferred To" },
            new () { Id = 13, Name = "Transferred From" },
        });
    }
}
