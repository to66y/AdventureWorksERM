using AdventureWorksCRM_1_0.Models.AppDbContext;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AdventureWorksCRM_1_0_Test
{
    internal class ProductTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { GetNewObjects };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private IEnumerable<Product> GetNewObjects => new Product[]
        {
            new Product { Name="Name1", Class="Class1", Color="Color1" },
            new Product { Name="Name2", Class="Class2", Color="Color2" },
            new Product { Name="Name3", Class="Class3", Color="Color3" },
            new Product { Name="Name4", Class="Class4", Color="Color4" },
        };
    }
}