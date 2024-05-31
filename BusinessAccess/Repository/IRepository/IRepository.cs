using System.Linq.Expressions;

namespace DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T GetFirstOrDefault(Expression<Func<T, bool>> filter);
        bool Any(Expression<Func<T, bool>> filter);
        IEnumerable<T> GetAll();

        void Add(T entity);
        void Remove(T entity);

        void RemoveRange(T entity);
        void Save();

    }
}
