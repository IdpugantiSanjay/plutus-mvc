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

    public IActionResult List()
    {
        return View(new ListViewModel(
            Transactions: _context.Transactions.Include(t => t.Category).ToArray()
        ));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
