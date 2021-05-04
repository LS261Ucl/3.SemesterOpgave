using System;
using System.Linq;

namespace Delpin.Application.Interfaces
{
    public interface IOrderBy<T> where T : class
    {
        Func<IQueryable<T>, IOrderedQueryable<T>> Sorting(string orderBy);
    }
}