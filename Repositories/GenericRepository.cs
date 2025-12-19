using System.Linq.Expressions;
using FPT_Booking_BE.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;

namespace Repository;

public class GenericRepository<T> : IGenericRepository<T>
where T : class
{
    protected FptFacilityBookingContext _context { get; }

    public GenericRepository(FptFacilityBookingContext context)
    {
        _context = context;
    }

    public virtual List<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public virtual void Create(T entity)
    {
        _context.Add(entity);
        _context.SaveChanges();
    }

    public virtual void Update(T entity)
    {
        var tracker = _context.Attach(entity);
        tracker.State = EntityState.Modified;
        _context.SaveChanges();
    }

    public virtual void Remove(T entity)
    {
        _context.Remove(entity);
        _context.SaveChanges();
    }

    public virtual T? GetById(long code)
    {
        return _context.Set<T>().Find(code);
    }

    public virtual T? GetById(int code)
    {
        return _context.Set<T>().Find(code);
    }

    public virtual T? GetById(short code)
    {
        return _context.Set<T>().Find(code);
    }

    public virtual int Count()
    {
        return _context.Set<T>().Count();
    }

    #region Asyncronous

    public virtual async Task<List<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public virtual async Task<int> CreateAsync(T entity)
    {
        _context.Add(entity);
        int data = await _context.SaveChangesAsync();
        return data;
    }

    public virtual async Task<int> UpdateAsync(T entity)
    {
        var tracker = _context.Attach(entity);
        tracker.State = EntityState.Modified;
        int data = await _context.SaveChangesAsync();
        return data;
    }

    public virtual async Task<bool> RemoveAsync(T entity)
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public virtual async Task<T?> GetByIdAsync(long code)
    {
        return await _context.Set<T>().FindAsync(code);
    }

    public virtual async Task<T?> GetByIdAsync(int code)
    {
        return await _context.Set<T>().FindAsync(code);
    }

    public virtual async Task<T?> GetByIdAsync(short code)
    {
        return await _context.Set<T>().FindAsync(code);
    }

    public virtual async Task<int> CountAsync()
    {
        return await _context.Set<T>().CountAsync();
    }
    #endregion


    #region Separating asigned entities and save operators        
    public virtual void PrepareCreate(T entity)
    {
        _context.Add(entity);
    }

    public virtual void PrepareUpdate(T entity)
    {
        var tracker = _context.Attach(entity);
        tracker.State = EntityState.Modified;
    }

    public virtual void PrepareRemove(T entity)
    {
        _context.Remove(entity);
    }

    public virtual int Save()
    {
        int changes = _context.SaveChanges();
        return changes;
    }

    public virtual async Task<int> SaveAsync()
    {
        int changes = await _context.SaveChangesAsync();
        return changes;
    }

    #endregion Separating asign entity and save operators


    public Task LoadCollectionAsync<TProperty>(
    T entity,
    Expression<Func<T, IEnumerable<TProperty>>> navigation)
    where TProperty : class
    {
        return _context.Entry(entity)
                       .Collection(navigation)
                       .LoadAsync();
    }

    public Task LoadReferenceAsync<TProperty>(
    T entity,
    Expression<Func<T, TProperty?>> navigation)
    where TProperty : class
    {
        return _context.Entry(entity)
                       .Reference(navigation)
                       .LoadAsync();
    }
}

