using System.Linq.Expressions;
using DeveloperStore.Application.Helpers;
namespace DeveloperStore.Application.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> WhereDynamic<T>(this IQueryable<T> query, string field, string operation, string value)
    {
        var parameter = Expression.Parameter(typeof(T), "x");        
        var member = Expression.PropertyOrField(parameter, field);
        var comparison = ExpressionBuilder.BuildComparisonExpression<T>(member, operation, value);    
        var lambda = Expression.Lambda<Func<T, bool>>(comparison, parameter);
        return query.Where(lambda);
    }

    public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string order)
    {
        var fields = order.Split(',');
        bool isFirstOrder = true;

        foreach (var field in fields)
        {
            var parts = field.Trim().Split(' ');
            var fieldName = parts[0];
            var direction = parts.Length > 1 && parts[1].ToLower() == "desc" ? "Descending" : "Ascending";

            var parameter = Expression.Parameter(typeof(T), "x");
            var member = Expression.PropertyOrField(parameter, fieldName);
            var lambda = Expression.Lambda(member, parameter);

            if (isFirstOrder)
            {
                var method = typeof(Queryable).GetMethods()
                    .First(m => m.Name == (direction == "Descending" ? "OrderByDescending" : "OrderBy") && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), member.Type);

                query = (IQueryable<T>)method.Invoke(null, new object[] { query, lambda });
                isFirstOrder = false;
            }
            else
            {
                var method = typeof(Queryable).GetMethods()
                    .First(m => m.Name == (direction == "Descending" ? "ThenByDescending" : "ThenBy") && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), member.Type);

                query = (IQueryable<T>)method.Invoke(null, new object[] { query, lambda });
            }
        }

        return query;
    }
}