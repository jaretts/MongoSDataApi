using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mobile.Models;

namespace Mobile.Models
{
    public interface IRepository<T> where T : MobileModelEntity // class
    {
        IQueryable<T> GetAll();

        T GetTemplate();

        void Post(String selector, T value);

        void Put(String selector, T value);
    }
}
