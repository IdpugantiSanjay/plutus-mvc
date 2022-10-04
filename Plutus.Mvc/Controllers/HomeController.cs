using System.Diagnostics;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Categories;
using MVC.Database;
using MVC.Models;
using MVC.Models.Home;
using MVC.Transactions;
using System.IO;

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
            transactions =
                transactions.Where(t => EF.Functions.ILike(t.Description!, $"%{query}%"));
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

    public async Task<IActionResult> Import([FromForm] IFormFile? statement)
    {
        if (statement is not null)
        {
            List<string?> headerAndRows = new();
            using (var reader = new StreamReader(statement.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                {
                    var row = await reader.ReadLineAsync();
                    if (!string.IsNullOrWhiteSpace(row))
                    {
                        headerAndRows.Add(row);
                    }
                }
            }

            // Dictionary<string, List<string>> table = new();
            // foreach (var columnName in headerAndRows[0]!.Split(","))
            // {
            //     table.Add(columnName.Trim(), new());
            // }

            var importedTransaction = new List<ImportedTransaction>();
            // Date -> 0, Description -> 1, Debit Amount -> 3, Credit Amount -> 4
            foreach (var rows in headerAndRows.Skip(1)
                         .Select(r => r!.Split(",").Select(c => c.Trim()).ToArray()))
            {
                var date = DateTime.Parse(rows[0], new CultureInfo("en-IN"));
                var description = rows[1];
                var cell3 = decimal.Parse(rows[3]);
                var amount = cell3 > 0 ? -cell3 : decimal.Parse(rows[4]);
                importedTransaction.Add(new ImportedTransaction(date, description, amount, null));
            }

            return View(new ImportViewModel(importedTransaction, _categories));
        }

        return View(new ImportViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> SaveImported([FromForm] IFormCollection? collection)
    {
        if (collection is null)
        {
            return Ok(collection);
        }

        List<ProcessedTransaction> transactions = new(collection.Count);
        foreach (var index in Enumerable.Range(0, collection["dateTime"].Count))
        {
            if (!DateTime.TryParse(collection["dateTime"][index],
                    provider: new CultureInfo("en-IN"), DateTimeStyles.None, out var dateTime))
            {
                return BadRequest($"Invalid DateTime at row: {index + 1}");
            }

            if (!int.TryParse(collection["category"][index], out var categoryId))
            {
                return BadRequest($"Invalid Category at row: {index + 1}");
            }

            if (!decimal.TryParse(collection["amount"][index], out var amount))
            {
                return BadRequest($"Invalid Amount value at row: {index + 1}");
            }

            transactions.Add(
                new ProcessedTransaction(
                    dateTime,
                    collection["description"][index],
                    amount,
                    categoryId
                )
            );
        }

        _context.Transactions.AddRange(transactions.Select(t => new Transaction
        {
            Amount = Math.Abs(t.Amount),
            CategoryId = t.CategoryId,
            Description = t.Description,
            DateTime = t.DateTime.UtcDateTime
        }));

        await _context.SaveChangesAsync();
        return RedirectToAction(actionName: "List");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
            { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
