using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreNetFramework.Repository
{
    interface IRepository<T>
    {
        int Add(T item);
        T GetItem(int Id);
        List<T> GetAll();
        void Remove(int Id);
    }
}
