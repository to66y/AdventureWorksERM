using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorksCRM_1_0.Models.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int PageIndex { get; set; }
        public int PageTotal { get; set; }

        public PagedList(List<T> items, int count, int pageIndex = 1, int pageSize = 10)
        {
            PageIndex = pageIndex;
            PageTotal = (int)Math.Ceiling(count/(double)pageSize);
            this.AddRange(items);
        }

        public bool HasPrevious => PageIndex > 1;
        public bool HasNext => PageIndex < PageTotal;

        public static async Task<PagedList<T>>AsPagedAsync(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            var count = await Task.Run(()=> source.Count());
            var items = await Task.Run(()=> source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());
            return new PagedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
