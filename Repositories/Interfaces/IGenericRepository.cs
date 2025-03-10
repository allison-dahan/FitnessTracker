using System;
using System.Linq.Expressions;

namespace FitnessTracker.Repositories.Interfaces
{
    /// <summary>
    /// Generic repository interface for basic CRUD operations
    /// </summary>
    /// <typeparam name="TEntity">The type of entity</typeparam>
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Retrieve all entities
        /// </summary>
        /// <returns>Collection of entities</returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// Find entities based on a condition
        /// </summary>
        /// <param name="predicate">Condition to filter entities</param>
        /// <returns>Filtered collection of entities</returns>
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Get a single entity by its ID
        /// </summary>
        /// <param name="id">Unique identifier</param>
        /// <returns>Entity if found, otherwise null</returns>
        TEntity GetById(int id);

        /// <summary>
        /// Add a new entity
        /// </summary>
        /// <param name="entity">Entity to add</param>
        void Add(TEntity entity);

        /// <summary>
        /// Update an existing entity
        /// </summary>
        /// <param name="entity">Entity to update</param>
        void Update(TEntity entity);

        /// <summary>
        /// Delete an entity by ID
        /// </summary>
        /// <param name="id">ID of entity to delete</param>
        void Delete(int id);
    }
}