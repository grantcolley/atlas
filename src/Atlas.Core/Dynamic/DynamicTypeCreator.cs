using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Atlas.Core.Dynamic
{
    public static class DynamicTypeCreator<T>
    {
        private static readonly Func<T> creator =
            Expression.Lambda<Func<T>>(Expression.New(typeof(T).GetConstructor(Type.EmptyTypes) ?? throw new InvalidOperationException(nameof(T))))
            .Compile();

        public static T Create()
        {
            return creator();
        }

        public static IEnumerable<T> CreateList()
        {
            var listType = typeof(List<>);
            var genericListType = listType.MakeGenericType(typeof(T));
            var instance = Activator.CreateInstance(genericListType);
            if (instance == null) throw new InvalidOperationException(nameof(instance));
            return (IEnumerable<T>)instance;
        }
    }
}
