using System.Linq.Expressions;

namespace EstateAgentAPI.Persistence.Repositories.Contracts
{
    public interface IRepositoryBase<T> where T : EntityBase
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        T FindById(int id);
        T Create(T entity);
        T Update(T entity);
        void Delete(T entity);
    }
}
