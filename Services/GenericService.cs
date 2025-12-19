using Repository.Interfaces;
using Service.Interfaces;

namespace Service;

public class GenericService<T> : IGenericService<T> where T : class
{
    private readonly IGenericRepository<T> _repository;

    public GenericService(IGenericRepository<T> repository)
    {
        _repository = repository;
    }

    public virtual T? GetById(long code) => _repository.GetById(code);

    public virtual T? GetById(int code) => _repository.GetById(code);

    public virtual T? GetById(short code) => _repository.GetById(code);

    public virtual int Count() => _repository.Count();

    public virtual async Task<List<T>> GetAllAsync() => await _repository.GetAllAsync();

    public virtual async Task<T?> GetByIdAsync(long code)
    {
        return await _repository.GetByIdAsync(code);
    }

    public virtual async Task<T?> GetByIdAsync(short code)
    {
        return await _repository.GetByIdAsync(code);
    }

    public virtual async Task<T?> GetByIdAsync(int code)
    {
        return await _repository.GetByIdAsync(code);
    }

    public virtual async Task<T?> GetByIdAsync(byte code)
    {
        return await _repository.GetByIdAsync(code);
    }

    public virtual async Task<int> CountAsync()
    {
        return await _repository.CountAsync();
    }

    public virtual async Task<T> EnsureExistAsync(long id)
    {
        T? data = await _repository.GetByIdAsync(id);
        if (data == null)
            throw new Exception("Cannot find " + id);
        return data;
    }

    public virtual async Task<T> EnsureExistAsync(int id)
    {
        T? data = await _repository.GetByIdAsync(id);
        if (data == null)
            throw new Exception("Cannot find " + id);
        return data;
    }

    void IGenericService<T>.Create(T entity) => _repository.Create(entity);

    void IGenericService<T>.Update(T entity) => _repository.Update(entity);

    void IGenericService<T>.Remove(T entity) => _repository.Remove(entity);

    async Task<int> IGenericService<T>.CreateAsync(T entity) => await _repository.CreateAsync(entity);

    async Task<int> IGenericService<T>.UpdateAsync(T entity) => await _repository.UpdateAsync(entity);

    async Task<bool> IGenericService<T>.RemoveAsync(T entity) => await _repository.RemoveAsync(entity);

    void IGenericService<T>.PrepareCreate(T entity) => _repository.PrepareCreate(entity);

    void IGenericService<T>.PrepareUpdate(T entity) => _repository.PrepareUpdate(entity);

    void IGenericService<T>.PrepareRemove(T entity) => _repository.PrepareRemove(entity);

    int IGenericService<T>.Save() => _repository.Save();

    async Task<int> IGenericService<T>.SaveAsync() => await _repository.SaveAsync();

}


