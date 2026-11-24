namespace library.Models;

// The member's page shows two things: the member, and their loans.
public class MemberDetails
{
    public Member Member { get; set; } = new();
    public List<LoanView> Loans { get; set; } = new();
}
