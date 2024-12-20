﻿using SocialChitChat.DataAccess.Specifications;

namespace SocialChitChat.DataAccess.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(bool asNoTracking = false);

    Task<int> GetCountAsync();

    Task<T?> GetAsync(int id);
    Task<T?> GetAsync(long id);
    Task<T?> GetAsync(string id);
    Task<T?> GetAsync(Guid id);

    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);

    Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> spec, bool asNoTracking = false);
    Task<T?> GetWithSpecAsync(ISpecification<T> spec, bool asNoTracking = false);
}
