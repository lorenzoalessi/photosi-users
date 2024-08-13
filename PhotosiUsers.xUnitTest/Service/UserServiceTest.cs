using AutoMapper;
using Moq;
using PhotosiUsers.Mapper;
using PhotosiUsers.Model;
using PhotosiUsers.Repository.User;
using PhotosiUsers.Service;

namespace PhotosiUsers.xUnitTest.Service;

public class UserServiceTest : TestSetup
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly IMapper _mapper;

    public UserServiceTest()
    {
        _mockUserRepository = new Mock<IUserRepository>();

        var config = new MapperConfiguration(conf =>
        {
            conf.AddProfile(typeof(UserMapperProfile));
        });
        
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task GetAsync_ShouldReturnList_Always()
    {
        // Arrange
        var service = GetService();

        var users = Enumerable.Range(0, _faker.Int(10, 30))
            .Select(_ => GenerateUser())
            .ToList();

        // Setup mock del repository
        _mockUserRepository.Setup(x => x.GetAsync())
            .ReturnsAsync(users);
        
        // Act
        var result = await service.GetAsync();
        
        // Assert
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
        
        _mockUserRepository.Verify(x => x.GetAsync(), Times.Once);
    }
    
    private IUserService GetService() => new UserService(_mockUserRepository.Object, _mapper);
    
    private User GenerateUser()
    {
        return new User()
        {
            Id = _faker.Int(1),
            FirstName = _faker.String2(1, 100),
            LastName = _faker.String2(1, 100),
            Username = _faker.String2(1, 100),
            Email = _faker.String2(1, 100),
            BirthDate = GenerateRandomDate()
        };
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