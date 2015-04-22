using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Castle.DynamicProxy;

namespace Casper.Domain.Specifications.Helpers
{
    public class InvocationRecorder : IInterceptor
    {
        private readonly List<IInvocation> _invocations = new List<IInvocation>();

        public IEnumerable<IInvocation> Invocations
        {
            get { return _invocations.AsEnumerable(); }
        }

        public IEnumerable<IInvocation> CallsTo<T>()
        {
            return _invocations.Where(i => i.Method.DeclaringType == typeof(T));
        }

        public IEnumerable<IInvocation> CallsTo<T>(Expression<Action<T>> method)
        {
            return CallsTo<T>((MethodCallExpression)method.Body);
        }

        public IEnumerable<IInvocation> CallsTo<T>(MethodCallExpression method)
        {
            var callsToType = CallsTo<T>();
            var callsToMethod = callsToType.Where(i => i.Method.Name == method.Method.Name && ArgumentsAreEqual(i.Arguments, method.Arguments));

            return callsToMethod;
        }

        private static bool ArgumentsAreEqual(IEnumerable<object> invocationArguments, IEnumerable<Expression> expectedExpressions)
        {
            return invocationArguments.SequenceEqual(expectedExpressions.Select(ExpectedValue));
        }

        private static object ExpectedValue(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Constant:
                    return ((ConstantExpression) expression).Value;

                case ExpressionType.MemberAccess:
                    return Expression.Lambda(expression).Compile().DynamicInvoke();

                default:
                    throw new NotSupportedException(string.Format("NodeType '{0}' is not supported.", expression.NodeType));
            }
        }

        public void Intercept(IInvocation invocation)
        {
            _invocations.Add(invocation);

            invocation.Proceed();
        }
    }
}