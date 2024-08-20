using System.Diagnostics.CodeAnalysis;

namespace PhotosiUsers.Dto;

[ExcludeFromCodeCoverage]
public class LoginDto
{
    public string Username { get; set; }
    
    public string Password { get; set; }
}