using Bogus;
using Microsoft.EntityFrameworkCore;
using PhotosiUsers.Model;

namespace PhotosiUsers.xUnitTest;

public abstract class TestSetup
{
    protected Context _context;

    protected Randomizer _faker;

    protected void SetUp()
    {
        _faker = new Randomizer();

        var dbOptions = new DbContextOptionsBuilder<Context>()
            .EnableSensitiveDataLogging()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new Context(dbOptions);
    }
}