using MVC.Categories;

namespace MVC.Transactions;

public class Transaction
{
    public long Id { get; init; }

    public decimal Amount { get; init; }

    public int CategoryId { get; set; }

    public DateTime DateTime { get; set; }
    
    public string? Description { get; init; }

    public Category Category { get; init; } = null!;
}