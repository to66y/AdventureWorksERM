using AdventureWorksCRM_1_0.Models.AppDbContext;
using System.Collections.Generic;
using System.Linq;

namespace AdventureWorksCRM_1_0.Controllers
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> Storage { get; }
        void Add(T p);
    }
}