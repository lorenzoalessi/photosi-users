using PhotosiUsers.Model;
using PhotosiUsers.Repository.User;

namespace PhotosiUsers.xUnitTest.Repository;

public class UserRepositoryTest : TestSetup
{
    public UserRepositoryTest()
    {
        SetUp();
    }

    [Fact]
    public async Task GetAsync_ShouldReturnList_Always()
    {
        // Arrange
        var repository = GetRepository();

        var users = Enumerable.Range(0, _faker.Int(10, 30))
            .Select(_ => GenerateUserAndSave())
            .ToList();
        
        // Act
        var result = await repository.GetAsync();
        
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.Equal(result.Count, users.Count);
            Assert.Empty(result.Select(x => x.Id).Except(users.Select(x => x.Id)));
            Assert.Empty(result.Select(x => x.FirstName).Except(users.Select(x => x.FirstName)));
            Assert.Empty(result.Select(x => x.LastName).Except(users.Select(x => x.LastName)));
            Assert.Empty(result.Select(x => x.Username).Except(users.Select(x => x.Username)));
            Assert.Empty(result.Select(x => x.Email).Except(users.Select(x => x.Email)));
            Assert.Empty(result.Select(x => x.BirthDate).Except(users.Select(x => x.BirthDate)));
            
            // Campi univoci
            Assert.Equal(result.Select(x => x.Id).Distinct().Count(), result.Count);
            Assert.Equal(result.Select(x => x.Username).Distinct().Count(), result.Count);
            
            // Campi obbligatori
            Assert.All(result, x => Assert.False(string.IsNullOrEmpty(x.FirstName)));
            Assert.All(result, x => Assert.False(string.IsNullOrEmpty(x.LastName)));
            Assert.All(result, x => Assert.False(string.IsNullOrEmpty(x.Username)));
            Assert.All(result, x => Assert.False(string.IsNullOrEmpty(x.Email)));
        });
    }
    
    private IUserRepository GetRepository() => new UserRepository(_context);

    private User GenerateUserAndSave()
    {
        var user = new User()
        {
            Id = _faker.Int(1),
            FirstName = _faker.String2(1, 100),
            LastName = _faker.String2(1, 100),
            Username = _faker.String2(1, 100),
            Email = _faker.String2(1, 100),
            BirthDate = GenerateRandomDate()
        };

        _context.Add(user);
        _context.SaveChanges();

        return user;
    }

    private DateTime GenerateRandomDate()
    {
        // Genero un anno casuale
        var year = _faker.Int(1950, DateTime.Now.Year);
        // Genero un mese casuale
        var month = _faker.Int(1, 12);
        // Genero un giorno casuale
        var daysInMonth = DateTime.DaysInMonth(year, month);
        var day = _faker.Int(1, daysInMonth);

        return new DateTime(year, month, day);
    }
}