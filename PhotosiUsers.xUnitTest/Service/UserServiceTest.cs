using AutoMapper;
using Moq;
using PhotosiUsers.Dto;
using PhotosiUsers.Exceptions;
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

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_IfNoUserFound()
    {
        // Arrange
        var service = GetService();
        var input = _faker.Int(1);
        
        // Act
        var result = await service.GetByIdAsync(input);

        // Assert
        Assert.Null(result);
        _mockUserRepository.Verify(x => x.GetByIdAsync(input), Times.Once);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnObject_IfUserFound()
    {
        // Arrange
        var service = GetService();
        var input = _faker.Int(1);
        var user = GenerateUser();
        
        _mockUserRepository.Setup(x => x.GetByIdAsync(input))
            .ReturnsAsync(user);
        
        // Act
        var result = await service.GetByIdAsync(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result.Id, user.Id);
        Assert.Equal(result.FirstName, user.FirstName);
        Assert.Equal(result.BirthDate, user.BirthDate);
        Assert.Equal(result.Email, user.Email);
        Assert.Equal(result.LastName, user.LastName);
        Assert.Equal(result.Password, user.Password);
        Assert.Equal(result.Username, user.Username);
        _mockUserRepository.Verify(x => x.GetByIdAsync(input), Times.Once);
    }

    [Fact]
    public void UpdateAsync_ShouldThrowException_IfUserNotFound()
    {
        // Arrange
        var service = GetService();
        var id = _faker.Int();
        var userDto = GenerateUserDto();
        
        // Act
        Assert.ThrowsAsync<UserException>(async () => await service.UpdateAsync(id, userDto));

        // Assert
        _mockUserRepository.Verify(x => x.GetByIdAsync(id), Times.Once);
        _mockUserRepository.Verify(x => x.SaveAsync(), Times.Never);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldUpdateObject_IfUserFound()
    {
        // Arrange
        var service = GetService();
        var id = _faker.Int(1);
        var userDto = GenerateUserDto(id);
        var user = GenerateUser(id);
        
        _mockUserRepository.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(user);
        
        // Act
        var result = await service.UpdateAsync(id, userDto);

        // Assert
        _mockUserRepository.Verify(x => x.GetByIdAsync(id), Times.Once);
        _mockUserRepository.Verify(x => x.SaveAsync(), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(userDto.FirstName, result.FirstName);
        Assert.Equal(userDto.BirthDate, result.BirthDate);
        Assert.Equal(userDto.Email, result.Email);
        Assert.Equal(userDto.LastName, result.LastName);
        Assert.Equal(userDto.Username, result.Username);
        Assert.Equal(userDto.Password, result.Password);
    }
    
    [Fact]
    public async Task AddAsync_ShouldAddObject_Always()
    {
        // Arrange
        var service = GetService();
        var userDto = GenerateUserDto();
        
        // Act
        var result = await service.AddAsync(userDto);

        // Assert
        _mockUserRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);

        Assert.NotNull(result);
        Assert.True(userDto.Id > 0);
        Assert.Equal(userDto.FirstName, result.FirstName);
        Assert.Equal(userDto.BirthDate, result.BirthDate);
        Assert.Equal(userDto.Email, result.Email);
        Assert.Equal(userDto.LastName, result.LastName);
        Assert.Equal(userDto.Username, result.Username);
        Assert.Equal(userDto.Password, result.Password);
    }

    [Fact]
    public void LoginAsync_ShouldThrowException_IfUserNotFound()
    {
        // Arrange
        var service = GetService();
        var input = GenerateLoginDto();
        
        // Act
        Assert.ThrowsAsync<UserException>(async () => await service.LoginAsync(input));
        
        // Assert
        _mockUserRepository.Verify(x => x.GetByUsernamePasswordAsync(input.Username, input.Password), Times.Once);
    }
    
    [Fact]
    public async Task LoginAsync_ShouldReturnObject_IfUserFound()
    {
        // Arrange
        var service = GetService();
        var input = GenerateLoginDto();
        var user = GenerateUser();
        
        _mockUserRepository.Setup(x => x.GetByUsernamePasswordAsync(input.Username, input.Password))
            .ReturnsAsync(user);
        
        // Act
        var result = await service.LoginAsync(input);
        
        // Assert
        _mockUserRepository.Verify(x => x.GetByUsernamePasswordAsync(input.Username, input.Password), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Password, result.Password);
        Assert.Equal(user.FirstName, result.FirstName);
        Assert.Equal(user.BirthDate, result.BirthDate);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.Username, result.Username);
        Assert.Equal(user.LastName, result.LastName);
    }
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task DeleteAsync_ShouldReturnTrueOrFalse_IfAddressBookFoundedOrNot(bool founded)
    {
        // Arrange
        var service = GetService();
        var input = _faker.Int();

        _mockUserRepository.Setup(x => x.DeleteAsync(input))
            .ReturnsAsync(founded);
        
        // Act
        var result = await service.DeleteAsync(input);

        // Assert
        _mockUserRepository.Verify(x => x.DeleteAsync(input), Times.Once);
        Assert.Equal(result, founded);
    }
    
    private IUserService GetService() => new UserService(_mockUserRepository.Object, _mapper);

    private LoginDto GenerateLoginDto()
    {
        return new LoginDto
        {
            Username = _faker.String2(15, 30),
            Password = _faker.String2(15, 30)
        };
    }
    
    private UserDto GenerateUserDto(int? id = null)
    {
        return new UserDto()
        {
            Id = id ?? _faker.Int(1),
            FirstName = _faker.String2(1, 100),
            LastName = _faker.String2(1, 100),
            Username = _faker.String2(1, 100),
            Email = _faker.String2(1, 100),
            BirthDate = GenerateRandomDate()
        };
    }
    
    private User GenerateUser(int? id = null)
    {
        return new User()
        {
            Id = id ?? _faker.Int(1),
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