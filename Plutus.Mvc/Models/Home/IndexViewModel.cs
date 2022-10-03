using MVC.Categories;

namespace MVC.Models.Home;

public class IndexViewModel
{
    public IndexViewModel(Category[] categories)
    {
        Categories = categories;
    }

    public Category[] Categories { get; }
}
