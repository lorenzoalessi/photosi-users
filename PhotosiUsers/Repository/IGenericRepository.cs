namespace PhotosiUsers.Repository;

public interface IGenericRepository<TDbEntity>
{
    Task<List<TDbEntity>> GetAsync();
    
    Task<TDbEntity?> GetByIdAsync(int id);
    
    Task<TDbEntity> AddAsync(TDbEntity dbEntity);
    
    Task<bool> DeleteAsync(int id);

    Task SaveAsync();
}