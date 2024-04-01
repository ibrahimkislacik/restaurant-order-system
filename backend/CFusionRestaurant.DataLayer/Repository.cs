using CFusionRestaurant.Entities.Common;
using CFusionRestaurant.ViewModel.Settings;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace CFusionRestaurant.DataLayer;

/// <summary>
/// Generic repository implementation for MongoDB.
/// </summary>
/// <typeparam name="T">The type of entity managed by the repository.</typeparam>
public class Repository<T> : IRepository<T> where T : BaseMongoEntity
{
    internal readonly IMongoCollection<T?> Entities;
    private MongoClient Client { get; set; }

    private readonly FindOptions<T> _options;

    /// <summary>
    /// Constructor to initialize the repository with a MongoDB settings instance.
    /// </summary>
    /// <param name="dbSettings">The MongoDB settings instance.</param>
    public Repository(IDbSettings dbSettings)
    {
        Client = new MongoClient(dbSettings.ConnectionString);
        var database = Client.GetDatabase(dbSettings.DatabaseName);

        var collation = new Collation("en", strength: CollationStrength.Secondary);
        _options = new FindOptions<T>
        {
            Collation = collation
        };

        Entities = database.GetCollection<T>(typeof(T).Name);
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> filter)
    {
        var entities = await Entities.FindAsync<T>(filter, _options).ConfigureAwait(false);
        return await entities.FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task<T?> GetAsync(string id)
    {
        return await (await Entities.FindAsync(p => p.Id == ObjectId.Parse(id))).FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task<T?> GetAsync(ObjectId id)
    {
        return await (await Entities.FindAsync(p => p.Id == id)).FirstOrDefaultAsync().ConfigureAwait(false);
    }
    public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, ProjectionDefinition<T> projectionDefinition)
    {
        var options = new FindOptions<T>
        {
            Collation = _options.Collation,
            Projection = projectionDefinition
        };
        IAsyncCursor<T?> entities = await Entities.FindAsync<T>(filter, options).ConfigureAwait(false);
        return await entities.FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task<T?> GetAsync(ObjectId id, ProjectionDefinition<T> projectionDefinition)
    {
        var options = new FindOptions<T>
        {
            Collation = _options.Collation,
            Projection = projectionDefinition
        };
        var entities = await Entities.FindAsync<T>(p => p.Id == id, options).ConfigureAwait(false);
        return await entities.FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task<T?> GetAsync(string id, ProjectionDefinition<T> projectionDefinition)
    {
        var options = new FindOptions<T>
        {
            Collation = _options.Collation,
            Projection = projectionDefinition
        };
        var entities = await Entities.FindAsync<T>(p => p.Id == ObjectId.Parse(id), options).ConfigureAwait(false);
        return await entities.FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public Task InsertAsync(T model)
    {
        model.CreatedDateTime = DateTime.UtcNow;
        return Entities.InsertOneAsync(model);
    }

    public Task UpdateAsync(T model)
    {
        return Entities.ReplaceOneAsync(p => p.Id == model.Id, model);
    }
    public Task<UpdateResult> UpdateAsync(ObjectId id, UpdateDefinition<T> updateDef)
    {
        return Entities!.UpdateOneAsync(p => p.Id == id, updateDef);
    }

    public Task DeleteAsync(string id)
    {
        return Entities.DeleteOneAsync(p => p.Id == ObjectId.Parse(id));
    }

    public Task DeleteAsync(ObjectId id)
    {
        return Entities.DeleteOneAsync(p => p.Id == id);
    }

    public async Task<List<T>> ListAsync(FilterDefinition<T> filter, ProjectionDefinition<T>? projection = null)
    {
        var findOptions = new FindOptions<T>
        {
            Collation = _options.Collation
        };

        if (projection != null)
        {
            findOptions.Projection = projection;
        }

        var cursor = await Entities.FindAsync(filter, findOptions).ConfigureAwait(false);
        return await cursor.ToListAsync().ConfigureAwait(false);
    }

    public async Task<List<T>> ListAsync(List<ObjectId> ids)
    {
        var filter = Builders<T>.Filter.In(p => p.Id, ids.Select(p => p));
        var entities = await Entities.FindAsync<T>(filter).ConfigureAwait(false);
        return await entities.ToListAsync().ConfigureAwait(false);
    }

    public async Task<List<T>> ListAsync()
    {
        var entities = await Entities.FindAsync<T>(FilterDefinition<T>.Empty).ConfigureAwait(false);
        return await entities.ToListAsync().ConfigureAwait(false);
    }

}
