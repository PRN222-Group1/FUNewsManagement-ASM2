﻿using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DataAccessLayer.Interfaces
{
    /// <summary>
    /// Custom specifications for entities.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria {  get; }
        List<Expression<Func<T, object>>> Includes { get;}
        List<Func<IQueryable<T>, IIncludableQueryable<T, object>>> ThenIncludes { get;}
        Expression<Func<T, object>> OrderBy { get;}
        Expression<Func<T, object>> OrderByDescending { get;}
        int Take {  get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }
    }
}
