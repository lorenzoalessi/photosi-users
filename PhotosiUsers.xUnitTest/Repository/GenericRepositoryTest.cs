using PhotosiUsers.Model;
using PhotosiUsers.Repository;
using PhotosiUsers.Repository.User;
using PhotosiUsers.Utility;

namespace PhotosiUsers.xUnitTest.Repository;

public class GenericRepositoryTest : TestSetup
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task GetByIdAsync_ShouldReturnEntity_IfFounded(bool founded)
    {
        // Arrange
        var repository = GetRepository();
        var id = founded ? GenerateUserAndSave().Id : 0;

        // Act
        var result = await repository.GetByIdAsync(id);

        // Assert
        if (founded) Assert.Equal(result?.Id, id);
        else Assert.Null(result);
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

     [Fact]
    public async Task AddAsync_Should_AddObject_Always()
    {
        // Arrange
        var repository = GetRepository();
        var input = new User()
        {
            Id = _faker.Int(1),
            Password = _faker.String2(1, 100).ConvertToSha512(),
            FirstName = _faker.String2(1, 100),
            LastName = _faker.String2(1, 100),
            Username = _faker.String2(1, 100),
            Email = _faker.String2(1, 100),
            BirthDate = GenerateRandomDate()
        };
        
        // Act
        var result = await repository.AddAsync(input);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal(input.BirthDate, result.BirthDate);
        Assert.Equal(input.Password, result.Password);
        Assert.Equal(input.FirstName, result.FirstName);
        Assert.Equal(input.Email, result.Email);
        Assert.Equal(input.Username, result.Username);
        Assert.Equal(input.LastName, result.LastName);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_IfObjectNotFound()
    {
        // Arrange
        var repository = GetRepository();
        
        // Act
        var result = await repository.DeleteAsync(_faker.Int(1));
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_IfObjectFound()
    {
        // Arrange
        var repository = GetRepository();
        var addressBook = GenerateUserAndSave();
        
        // Act
        var result = await repository.DeleteAsync(addressBook.Id);
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task SaveAsync_ShouldNotThrowException_Always()
    {
        // Arrange
        var repository = GetRepository();
        
        var input = new User()
        {
            Id = _faker.Int(1),
            Password = _faker.String2(1, 100).ConvertToSha512(),
            FirstName = _faker.String2(1, 100),
            LastName = _faker.String2(1, 100),
            Username = _faker.String2(1, 100),
            Email = _faker.String2(1, 100),
            BirthDate = GenerateRandomDate()
        };
        await _context.AddAsync(input);

        // Act
        await repository.SaveAsync();
        
        // Assert
        Assert.True(input.Id > 0);
    }
    
    private IGenericRepository<User> GetRepository() => new UserRepository(_context);

    private User GenerateUserAndSave()
    {
        var user = new User()
        {
            Id = _faker.Int(1),
            Password = _faker.String2(1, 100).ConvertToSha512(),
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