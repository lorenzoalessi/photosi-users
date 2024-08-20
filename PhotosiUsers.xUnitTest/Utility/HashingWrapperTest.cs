

using Bogus;
using PhotosiUsers.Utility;

namespace PhotosiUsers.xUnitTest.Utility;

public class HashingWrapperTest
{
    private readonly Randomizer _faker;

    public HashingWrapperTest()
    {
        _faker = new Randomizer();
    }

    [Fact]
    public void ConvertToSha512_ShouldReturnShaString_Always()
    {
        // Arrange
        var stringToHash = _faker.String2(15, 30);
        
        // Act
        var result = stringToHash.ConvertToSha512();
        
        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }
    
}