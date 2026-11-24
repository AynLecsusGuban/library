using System.ComponentModel.DataAnnotations;

namespace library.Models;

// One Member object holds one row of lending.member.
public class Member
{
    public long MemberId { get; set; }               // member_id    BIGINT, the primary key

    [StringLength(80, ErrorMessage = "The name can be at most 80 characters.")]
    [RegularExpression(@"^[A-Za-zñÑ .'-]+, [A-Za-zñÑ .'-]+$", ErrorMessage = "Write the name as Lastname, Firstname.")]
    public string FullName { get; set; } = "";       // full_name    VARCHAR(80) NOT NULL

    [StringLength(120, ErrorMessage = "The email can be at most 120 characters.")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[A-Za-z]{2,}$", ErrorMessage = "Enter an email such as ana@ubaguio.edu.")]
    public string? Email { get; set; }               // email        VARCHAR(120), may be NULL, UNIQUE

    public string MemberType { get; set; } = "";     // member_type  VARCHAR(10) NOT NULL

    public long OpenLoans { get; set; }              // filled only by the member list; never saved
}
