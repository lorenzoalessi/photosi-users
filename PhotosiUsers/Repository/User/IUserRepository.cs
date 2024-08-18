namespace PhotosiUsers.Repository.User;

public interface IUserRepository : IGenericRepository<Model.User>
{
    Task<Model.User?> GetByUsernamePasswordAsync(string username, string password);
}