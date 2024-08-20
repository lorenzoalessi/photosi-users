using Microsoft.EntityFrameworkCore;
using PhotosiUsers.Model;
using PhotosiUsers.Utility;

namespace PhotosiUsers.Repository.User;

public class UserRepository : GenericRepository<Model.User>, IUserRepository
{
    public UserRepository(Context context) : base(context)
    {
    }

    public async Task<Model.User?> GetByUsernamePasswordAsync(string username, string password) => 
        await Context.User
            .Where(x => x.Password == password.ConvertToSha512())
            .Where(x => x.Username == username)
            .FirstOrDefaultAsync();
}