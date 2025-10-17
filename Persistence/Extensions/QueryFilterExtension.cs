using Domain.Interfaces;
using System.Linq.Expressions;

namespace Persistence.Extensions;

public static class QueryFilterExtension
{
    public static LambdaExpression GenerateQueryFilter(Type type)
    {
        var parameter = Expression.Parameter(type, "e");

        var falseConstant = Expression.Constant(false);
        var propertyAccess = Expression.PropertyOrField(parameter, nameof(IAuditTrail.DeletedBy));
        // var equalExpression = Expression.Equal(propertyAccess, falseConstant);
        var isNullOrEmpty = Expression.Call(typeof(string), nameof(string.IsNullOrEmpty), Type.EmptyTypes, propertyAccess);
        return Expression.Lambda(isNullOrEmpty, parameter);
    }
}
