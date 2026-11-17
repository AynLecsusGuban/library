namespace library.Models;

// One row of the Loans page: a loan, joined with its member's name and its book's title.
public class LoanView
{
    public long LoanId { get; set; }
    public long MemberId { get; set; }
    public string MemberName { get; set; } = "";
    public string BookTitle { get; set; } = "";
    public DateOnly LoanedOn { get; set; }
    public DateOnly? ReturnedOn { get; set; }      // null while the book is out
    public int DaysAllowed { get; set; }
    public string Status { get; set; } = "";       // Out, Overdue or Returned: worked out by the SQL
}
