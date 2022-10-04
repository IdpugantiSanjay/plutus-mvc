using MVC.Categories;

namespace MVC.Models.Home;


public record ProcessedTransaction(DateTimeOffset DateTime, string Description, decimal Amount, int CategoryId);

public record ImportedTransaction(DateTimeOffset DateTime, string Description, decimal Amount, string? Category);

public record ImportViewModel(List<ImportedTransaction>? ImportedTransactions = null, Category[]? Categories = null);
