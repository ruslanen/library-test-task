using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace library_test_task
{
    /// <summary>
    /// Методы расширения для IEnumerable&lt;Expression&lt;Func&lt;T, bool&gt;&gt;&gt;
    /// </summary>
    public static class FilterExpressionExtensions
    {
        /// <summary>
        /// Составить выражение для предиката Where из одного и более условий
        /// https://stackoverflow.com/questions/4429956/iqueryablet-where-suitable-expression-in-where
        /// </summary>
        /// <param name="filters">Последовательность фильтров</param>
        /// <returns>Выражение фильтрации</returns>
        public static Expression<Func<T, bool>> OrTheseFiltersTogether<T>( 
            this IEnumerable<Expression<Func<T, bool>>> filters) 
        { 
            Expression<Func<T, bool>> firstFilter = filters.FirstOrDefault(); 
            if (firstFilter == null) 
            { 
                Expression<Func<T, bool>> alwaysTrue = x => true; 
                return alwaysTrue; 
            } 

            var body = firstFilter.Body; 
            var param = firstFilter.Parameters.ToArray(); 
            foreach (var nextFilter in filters.Skip(1)) 
            { 
                var nextBody = Expression.Invoke(nextFilter, param); 
                body = Expression.AndAlso(body, nextBody); 
            } 
            Expression<Func<T, bool>> result = Expression.Lambda<Func<T, bool>>(body, param); 
            return result; 
        } 
    }
}