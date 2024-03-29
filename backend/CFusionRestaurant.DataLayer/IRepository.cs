using CFusionRestaurant.Entities.Common;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace CFusionRestaurant.DataLayer;

public interface IRepository<T> where T : BaseMongoEntity
{
    Task<T?> GetAsync(Expression<Func<T, bool>> filter);
    Task<T?> GetAsync(string id);
    Task<T?> GetAsync(ObjectId id);
    Task<T?> GetAsync(Expression<Func<T, bool>> filter, ProjectionDefinition<T> projectionDefinition);
    Task<T?> GetAsync(ObjectId id, ProjectionDefinition<T> projectionDefinition);
    Task<T?> GetAsync(string id, ProjectionDefinition<T> projectionDefinition);
    Task InsertAsync(T model);
    Task UpdateAsync(T model);
    Task<UpdateResult> UpdateAsync(ObjectId id, UpdateDefinition<T> updateDef);
    Task DeleteAsync(string id);
    Task DeleteAsync(ObjectId id);
    Task<List<T>> ListAsync(FilterDefinition<T> filter, ProjectionDefinition<T>? projection = null);
    Task<List<T>> ListAsync(List<ObjectId> ids);

    Task<List<T>> ListAsync();
}