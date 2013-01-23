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

        IQueryable<T> GetAll(string select);

        T GetTemplate();

        T Post(T value);

        T Put(String selector, T value);

        String GetErrorText();

        void SetTenantDataSet(string dataset, string userToken);
    }
}
