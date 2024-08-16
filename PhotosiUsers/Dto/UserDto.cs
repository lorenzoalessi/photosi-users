﻿namespace PhotosiUsers.Dto;

public class UserDto
{
    public int Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Username { get; set; }
    
    public string Password { get; set; }
    
    public string Email { get; set; }
    
    public DateTime? BirthDate { get; set; }
}