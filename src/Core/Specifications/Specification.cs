using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Zord.Core.Specifications
{
    public abstract class Specification<T> : ISpecification<T> where T : class
    {
        public Expression<Func<T, bool>>? Selector { get; private set; }

        protected virtual void Where(Expression<Func<T, bool>> expression)
        {
            if (Selector is null)
            {
                Selector = expression;
            }
            else
            {
                InvocationExpression invocationExpression = Expression.Invoke(Selector, expression.Parameters.Cast<Expression>());
                Selector = Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expression.Body, invocationExpression), (IEnumerable<ParameterExpression>)expression.Parameters);
            }
        }

        protected virtual void WhereIf(bool condition, Expression<Func<T, bool>> expression)
        {
            if (condition)
            {
                Where(expression);
            }
        }
    }
}
