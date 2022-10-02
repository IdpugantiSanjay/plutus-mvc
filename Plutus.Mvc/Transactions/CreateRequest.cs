namespace MVC.Transactions;

public record CreateRequest(decimal Amount, DateTimeOffset DateTime, int CategoryId, string? Description);
