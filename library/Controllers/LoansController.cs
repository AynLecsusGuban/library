using library.Data;
using library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace library.Controllers;

// Answers every address that starts with /Loans.
public class LoansController : Controller
{
    private readonly LoanRepository _loans;
    private readonly MemberRepository _members;
    private readonly BookRepository _books;

    public LoansController(LoanRepository loans, MemberRepository members, BookRepository books)
    {
        _loans = loans;
        _members = members;
        _books = books;
    }

    // GET /Loans : every loan, newest first.
    public async Task<IActionResult> Index()
    {
        var loans = await _loans.GetAllAsync();
        return View(loans);
    }
}
