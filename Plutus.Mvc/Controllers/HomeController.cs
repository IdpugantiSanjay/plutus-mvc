using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Categories;
using MVC.Database;
using MVC.Models;
using MVC.Models.Home;
using MVC.Transactions;

namespace MVC.Controllers;

public class HomeController : Controller
{
    private readonly PlutusDbContext _context;
    private readonly Category[] _categories;

    public HomeController(PlutusDbContext context, Category[] categories)
    {
        _context = context;
        _categories = categories;
    }

    public IActionResult Index()
    {
        return View(new IndexViewModel(_categories));
    }

    public async Task<IActionResult> Save([FromForm] CreateRequest request, [FromRoute] int? id)
    {
        if (id.HasValue)
        {
            _context.Transactions.Update(new Transaction
            {
                Id = id.Value,
                Amount = request.Amount,
                CategoryId = request.CategoryId,
                DateTime = request.DateTime.UtcDateTime,
                Description = request.Description
            });
        }

        else
        {
            _context.Transactions.Add(new Transaction
            {
                Amount = request.Amount,
                CategoryId = request.CategoryId,
                DateTime = request.DateTime.UtcDateTime,
                Description = request.Description
            });
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(actionName: "List");
    }

    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid TransactionId");
        }

        _context.Transactions.Remove(new Transaction
        {
            Id = id
        });

        await _context.SaveChangesAsync();
        return RedirectToAction(actionName: "List");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var transaction = await _context.Transactions.FirstAsync(t => t.Id == id);
        return View(new EditViewModel(
            categories: _categories, transaction: transaction));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult List(string? query, string? sortBy)
    {
        var transactions = _context.Transactions.AsQueryable();
        switch (sortBy?.ToLower())
        {
            case "date asc":
                transactions = transactions.OrderBy(t => t.DateTime);
                break;
            case "date desc":
                transactions = transactions.OrderByDescending(t => t.DateTime);
                break;
        }


        if (!string.IsNullOrWhiteSpace(query))
        {
            transactions = transactions.Where(t => EF.Functions.ILike(t.Description!, $"%{query}%"));
        }

        var transactionsGroupedByMonthYear = transactions
                .Include(t => t.Category)
                .AsEnumerable()
                .GroupBy(g => g.DateTime.ToString("Y"))
                .ToDictionary(x => x.Key, g => g.ToList())
            ;

        var sortedMonthYear = transactionsGroupedByMonthYear.Keys.OrderByDescending(DateTime.Parse);

        SortedDictionary<(int SequenceNumber, string MonthYear), Transaction[]>
            transactionsGroupedByMonth =
                new();

        var sequenceNumber = 0;
        foreach (var monthYear in sortedMonthYear)
        {
            transactionsGroupedByMonth[(sequenceNumber++, monthYear)] =
                transactionsGroupedByMonthYear[monthYear].ToArray();
        }

        return View(new ListViewModel(
            TransactionsGrouped: transactionsGroupedByMonth,
            new[] { "Date Asc", "Date Desc" },
            query,
            sortBy
        ));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
            { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
