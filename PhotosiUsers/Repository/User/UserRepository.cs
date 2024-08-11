using PhotosiUsers.Model;

namespace PhotosiUsers.Repository.User;

public class UserRepository : GenericRepository<Model.User>, IUserRepository
{
    public UserRepository(Context context) : base(context)
    {
    }
}