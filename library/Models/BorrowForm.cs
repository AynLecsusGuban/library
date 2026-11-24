using System.ComponentModel.DataAnnotations;

namespace library.Models;

// What the Borrow form sends. It is not a table row, so it is its own small class.
public class BorrowForm
{
    [Required(ErrorMessage = "Choose a member.")]
    public long? MemberId { get; set; }

    [Required(ErrorMessage = "Choose a book.")]
    public long? BookId { get; set; }

    [Range(1, 30, ErrorMessage = "A loan lasts from 1 to 30 days.")]
    public int DaysAllowed { get; set; } = 7;
}
