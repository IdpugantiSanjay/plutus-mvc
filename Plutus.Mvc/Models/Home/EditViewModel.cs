using MVC.Categories;
using MVC.Transactions;

namespace MVC.Models.Home;

public class EditViewModel
{
    public Transaction Transaction { get; }
    public Category[] Categories { get; }

    public EditViewModel(Transaction transaction, Category[] categories)
    {
        Transaction = transaction;
        Categories = categories;
    }
}
