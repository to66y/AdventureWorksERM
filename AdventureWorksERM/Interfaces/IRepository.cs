using System.Linq;

namespace AdventureWorksERM.Controllers
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Storage { get; }
        void Add(T p);
        IQueryable<T> GetAll();
    }
}