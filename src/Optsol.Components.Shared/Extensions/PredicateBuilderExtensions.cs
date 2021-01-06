using System.Linq;
using System;
using System.Linq.Expressions;

namespace Optsol.Components.Shared.Extensions
{
    public static class PredicateBuilderExtensions
    {
        public static Expression<Func<T, bool>> True<T>() { return exp => true; }

        public static Expression<Func<T, bool>> False<T>() { return exp => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> sourceExp, Expression<Func<T, bool>> compareExp)
        {
            var invokedExp = GetInvokedExp(sourceExp, compareExp);

            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(sourceExp.Body, invokedExp), sourceExp.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> sourceExp, Expression<Func<T, bool>> compareExp)
        {
            var invokedExp = GetInvokedExp(sourceExp, compareExp);

            return Expression.Lambda<Func<T, bool>>(Expression.And(sourceExp.Body, invokedExp), sourceExp.Parameters);
        }

        private static InvocationExpression GetInvokedExp<T>(Expression<Func<T, bool>> sourceExp, Expression<Func<T, bool>> compareExp)
        {
            return Expression.Invoke(compareExp, sourceExp.Parameters.Cast<Expression>());
        }
    }
}
