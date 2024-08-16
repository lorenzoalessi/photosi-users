using Microsoft.EntityFrameworkCore;
using PhotosiUsers.Model;

namespace PhotosiUsers.Repository;

public class GenericRepository<TDbEntity> : IGenericRepository<TDbEntity> where TDbEntity : class
{
    protected readonly Context _context;

    public GenericRepository(Context context)
    {
        _context = context;
    }

    public async Task<List<TDbEntity>> GetAsync()
    {
        return await _context.Set<TDbEntity>().ToListAsync();
    }
    
    public async Task<TDbEntity> GetByIdAsync(int id) => await _context.Set<TDbEntity>().FindAsync(id);
    
    public async Task<TDbEntity> AddAsync(TDbEntity dbEntity)
    {
        await _context.Set<TDbEntity>().AddAsync(dbEntity);
        await _context.SaveChangesAsync();

        return dbEntity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Set<TDbEntity>().FindAsync(id);
        if (entity == null)
            return false;
            
        _context.Set<TDbEntity>().Remove(entity);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task SaveAsync() => await _context.SaveChangesAsync();
}