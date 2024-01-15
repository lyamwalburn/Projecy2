using EstateAgentAPI.EF;
using EstateAgentAPI.Persistence.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace EstateAgentAPI.Persistence.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : EntityBase

    {
        protected EstateAgentContext _repositoryContext { get; set; }

        public RepositoryBase(EstateAgentContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }
        public T Create(T entity)
        {
            _repositoryContext.Set<T>().Add(entity);
            _repositoryContext.SaveChanges();
            //_repositoryContext.Dispose();
            return entity;
        }

        public void Delete(T entity)
        {
            _repositoryContext.ChangeTracker.Clear();
            _repositoryContext.Set<T>().Remove(entity);
            _repositoryContext.SaveChanges();
            //_repositoryContext.Dispose();
        }

        public IQueryable<T> FindAll() => _repositoryContext.Set<T>().AsNoTracking();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            var allEntities = _repositoryContext.Set<T>().AsNoTracking();
            var entities = allEntities.Where(expression);
            return entities;
        }

        public T FindById(int id)
        {
            return _repositoryContext.Set<T>().SingleOrDefault(predicate => predicate.Id == id);
        }

        public T Update(T entity)
        {
            _repositoryContext.Set<T>().Update(entity);
            _repositoryContext.SaveChanges();
            //_repositoryContext.Dispose();
            return entity;
        }
    }
}
