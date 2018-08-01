using System.Linq;
using ScalableWeb.Domain.Models;

namespace ScalableWeb.Domain.Patterns
{
    public interface IRepository<T>
    {
        void Insert(T entity);
        IQueryable<T> Queriable();
    }
}