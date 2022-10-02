namespace MVC.Transactions;

public record UpdateRequest(int TransactionId, decimal Amount, DateTimeOffset DateTime, int CategoryId, string? Description) 
    : CreateRequest(Amount, DateTime, CategoryId, Description);