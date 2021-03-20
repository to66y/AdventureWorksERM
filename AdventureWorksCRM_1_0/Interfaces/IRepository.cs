using AdventureWorksERM.Models.AppDbContext;
using System.Collections.Generic;
using System.Linq;

namespace AdventureWorksERM.Controllers
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> Storage { get; }
        void Add(T p);
    }
}