/*
* This class implements the IRepository interface.
* It is used to define the generic repository pattern.
*/
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreGenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {

        // DbContext is used to connect to the database.
        protected readonly DbContext context;

        // Constructor: Initializes the context property.
        public EfCoreGenericRepository(DbContext ctx) { context = ctx; }
        
        // Gets all the entities
        public List<TEntity> GetAll()
        {
            return context.Set<TEntity>().ToList();
        }

        // Gets the entity by the id
        public TEntity GetById(int id)
        {
            return context.Set<TEntity>().Find(id);
        }

        // Creates the entity
        public void Create(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
        }

        // Deletes the entity
        public void Delete(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }

        // Updates the entity
        public virtual void Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }
    }
}