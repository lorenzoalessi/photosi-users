using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotosiUsers.Model;

[Table("user")]
public class User
{
    [Column("id"), Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column("first_name"), StringLength(100), Required]
    public string FirstName { get; set; }

    [Column("last_name"), StringLength(100), Required]
    public string LastName { get; set; }

    [Column("username"), StringLength(100), Required]
    public string Username { get; set; }
    
    [Column("email"), StringLength(100), Required]
    public string Email { get; set; }
    
    [Column("birth_date")]
    public DateTime? BirthDate { get; set; }
    
    public bool? IsAdult => BirthDate.HasValue ? 
        DateTime.Now.Year - BirthDate.Value.Year - (DateTime.Now.DayOfYear < BirthDate.Value.DayOfYear ? 1 : 0) >= 18 
        : null;
}