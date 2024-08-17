using Microsoft.EntityFrameworkCore;
using PhotosiUsers.Model;

namespace PhotosiUsers.Repository.User;

public class UserRepository : GenericRepository<Model.User>, IUserRepository
{
    public UserRepository(Context context) : base(context)
    {
    }

    public async Task<Model.User?> GetByUsernameAsync(string username) => 
        await Context.User.FirstOrDefaultAsync(x => x.Username == username);
}