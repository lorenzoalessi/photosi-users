using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PhotosiUsers.Model;

[ExcludeFromCodeCoverage]
[Table("user")]
public class User
{
    [Column("id"), Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("first_name"), Required]
    public string FirstName { get; set; }

    [Column("last_name"), Required]
    public string LastName { get; set; }

    [Column("username"), Required]
    public string Username { get; set; }
    
    [Column("password"), Required]
    public string Password { get; set; }
    
    [Column("email"), Required]
    public string Email { get; set; }
    
    [Column("birth_date")]
    public DateTime? BirthDate { get; set; }
    
    public bool? IsAdult => BirthDate.HasValue ? 
        DateTime.Now.Year - BirthDate.Value.Year - (DateTime.Now.DayOfYear < BirthDate.Value.DayOfYear ? 1 : 0) >= 18 
        : null;
}