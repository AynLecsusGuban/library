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

    // Fills the Borrow form's two dropdowns from the database.
    private async Task FillDropdownsAsync()
    {
        ViewBag.Members = new SelectList(await _members.GetAllAsync(), "MemberId", "FullName");
        ViewBag.Books = new SelectList(await _books.GetAllAsync(), "BookId", "Title");
    }

    // GET /Loans/Borrow : the empty Borrow form.
    public async Task<IActionResult> Borrow()
    {
        await FillDropdownsAsync();
        return View(new BorrowForm());
    }

    // POST /Loans/Borrow : the form was submitted.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Borrow(BorrowForm form)
    {
        if (ModelState.IsValid)
        {
            bool saved = await _loans.BorrowAsync(form.MemberId!.Value, form.BookId!.Value, form.DaysAllowed);
            if (saved)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", $"This member already has {LoanRepository.MaxOpenLoans} books out. Nothing was saved.");
        }

        await FillDropdownsAsync();
        return View(form);
    }



}
