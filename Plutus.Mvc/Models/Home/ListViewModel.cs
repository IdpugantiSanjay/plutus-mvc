using MVC.Transactions;

namespace MVC.Models.Home;

public record ListViewModel(SortedDictionary<(int SequenceNumber, string MonthYear), Transaction[]> TransactionsGrouped,  string[] SortOptions, string? Query, string? SortBy);
