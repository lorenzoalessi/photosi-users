namespace PhotosiUsers.Repository.User;

public interface IUserRepository : IGenericRepository<Model.User>
{
    Task<Model.User?> GetByUsernameAsync(string username);
}