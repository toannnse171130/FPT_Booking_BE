namespace Service.Interfaces;

public interface IGenericService<T> where T : class
{
    public T? GetById(long code);
    public T? GetById(int code);
    public int Count();
    public Task<List<T>> GetAllAsync();
    public Task<T?> GetByIdAsync(long code);
    public Task<T?> GetByIdAsync(int code);
    public Task<T?> GetByIdAsync(short code);
    public Task<int> CountAsync();
    public Task<T> EnsureExistAsync(long id);
    public Task<T> EnsureExistAsync(int id);
    void Create(T entity);
    void Update(T entity);
    void Remove(T entity);
    Task<int> CreateAsync(T entity);
    Task<int> UpdateAsync(T entity);
    Task<bool> RemoveAsync(T entity);
    void PrepareCreate(T entity);
    void PrepareUpdate(T entity);
    void PrepareRemove(T entity);
    int Save();
    Task<int> SaveAsync();
}


