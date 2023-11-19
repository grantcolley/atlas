using System.Linq.Expressions;
using System.Reflection;

namespace Atlas.Blazor.Render
{
    public static class ExpressionExtensions
    {
        public static MemberInfo? GetMemberInfo<T, TProperty>(this Expression<Func<T, TProperty>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            MemberExpression? memberExpression = UnaryExpressionCheck(expression.Body) as MemberExpression;

            if (memberExpression == null)
            {
                return null;
            }

            Expression? containingExpression = memberExpression.Expression;

            if (containingExpression != null)
            {
                while (true)
                {
                    containingExpression = UnaryExpressionCheck(containingExpression);

                    if (containingExpression != null && containingExpression.NodeType == ExpressionType.MemberAccess)
                    {
                        containingExpression = ((MemberExpression)containingExpression).Expression;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (containingExpression == null || containingExpression.NodeType != ExpressionType.Parameter)
            {
                return null;
            }

            return memberExpression.Member;
        }

        private static Expression UnaryExpressionCheck(Expression? expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return expression is UnaryExpression ? ((UnaryExpression)expression).Operand : expression;
        }
    }
}
