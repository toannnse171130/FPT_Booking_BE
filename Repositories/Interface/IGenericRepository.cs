namespace Repository.Interfaces;

public interface IGenericRepository<T> where T : class
{
    public List<T> GetAll();
    public void Create(T entity);
    public void Update(T entity);
    public void Remove(T entity);
    public T? GetById(long code);
    public T? GetById(int code);
    public T? GetById(short code);
    public int Count();
    public Task<List<T>> GetAllAsync();
    public Task<int> CreateAsync(T entity);
    public Task<int> UpdateAsync(T entity);
    public Task<bool> RemoveAsync(T entity);
    public Task<T?> GetByIdAsync(long code);
    public Task<T?> GetByIdAsync(int code);
    public Task<T?> GetByIdAsync(short code);
    public Task<int> CountAsync();
    public void PrepareCreate(T entity);
    public void PrepareUpdate(T entity);
    public void PrepareRemove(T entity);
    public int Save();
    public Task<int> SaveAsync();
}


