using PhotosiUsers.Model;
using PhotosiUsers.Repository.User;
using PhotosiUsers.Utility;

namespace PhotosiUsers.xUnitTest.Repository;

public class UserRepositoryTest : TestSetup
{
    [Fact]
    public async Task GetByUsernamePasswordAsync_ShouldReturnNull_IfNoUserFound()
    {
        // Arrange
        var repository = GetRepository();

        // Act
        var result = await repository.GetByUsernamePasswordAsync(_faker.String2(15, 30), _faker.String2(15, 30));

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task GetByUsernamePasswordAsync_ShouldReturnObject_IfUserFound()
    {
        // Arrange
        var repository = GetRepository();
        var password = _faker.String2(15, 30);
        var user = GenerateUserAndSave(password);
        
        // Act
        var result = await repository.GetByUsernamePasswordAsync(user.Username, password);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result.Id, user.Id);
        Assert.Equal(result.FirstName, user.FirstName);
        Assert.Equal(result.Email, user.Email);
        Assert.Equal(result.Username, user.Username);
        Assert.Equal(result.LastName, user.LastName);
        Assert.Equal(result.Password, user.Password);
        Assert.Equal(result.BirthDate, user.BirthDate);
    }
    
    private IUserRepository GetRepository() => new UserRepository(_context);
    
    private User GenerateUserAndSave(string? password = null)
    {
        var user = new User()
        {
            Id = _faker.Int(1),
            Password = password?.ConvertToSha512() ?? _faker.String2(1, 100).ConvertToSha512(),
            FirstName = _faker.String2(1, 100),
            LastName = _faker.String2(1, 100),
            Username = _faker.String2(1, 100),
            Email = _faker.String2(1, 100)
        };

        _context.Add(user);
        _context.SaveChanges();

        return user;
    }
}