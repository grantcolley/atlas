﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Atlas.Core.Dynamic
{
    /// <summary>
    /// Builds a new instance of a <see cref="DynamicTypeHelper"/> for the specified type and caches it for re-use.
    /// </summary>
    public static class DynamicTypeHelper
    {
        private static readonly Dictionary<Type, object> _cache = new Dictionary<Type, object>();

        private static int _counter;

        private static readonly object _cacheLock = new();

        /// <summary>
        /// Gets an instance of a <see cref="DynamicTypeHelper"/> for the specified type.
        /// Once created it is cached for re-use.
        /// </summary>
        /// <typeparam name="T">The specified type.</typeparam>
        /// <returns>An instance of a <see cref="DynamicTypeHelper"/> for the specified type.</returns>
        public static DynamicType<T> Get<T>() where T : class, new()
        {
            lock (_cacheLock)
            {
                var t = typeof(T);

                if (_cache.TryGetValue(t, out object? result))
                {
                    return (DynamicType<T>)result;
                }

                var propertyInfos = PropertyInfoHelper.GetPropertyInfos(t);
                var typeHelper = CreateTypeHelper<T>(propertyInfos);

                _cache.Add(t, typeHelper);
                return typeHelper;
            }
        }

        private static DynamicType<T> CreateTypeHelper<T>(IEnumerable<PropertyInfo> propertyInfos) where T : class, new()
        {
            var capacity = propertyInfos.Count() - 1;
            var getters = new Dictionary<string, Func<T, object?>>(capacity);
            var setters = new Dictionary<string, Action<T, object?>>(capacity);

            var createInstance = CreateInstance<T>();

            foreach (var propertyInfo in propertyInfos)
            {
                getters.Add(propertyInfo.Name, GetValue<T>(propertyInfo));

                if (propertyInfo.CanWrite)
                {
                    setters.Add(propertyInfo.Name, SetValue<T>(propertyInfo));
                }
            }

            return new DynamicType<T>(createInstance, getters, setters, propertyInfos);
        }

        private static Func<T> CreateInstance<T>() where T : class, new()
        {
            var t = typeof(T);
            var methodName = "CreateInstance_" + typeof(T).Name + "_" + GetNextCounterValue();
            var dynMethod = new DynamicMethod(methodName, t, null, typeof(DynamicTypeHelper).Module);
            ILGenerator il = dynMethod.GetILGenerator();
            il.Emit(OpCodes.Newobj, t.GetConstructor(Type.EmptyTypes) ?? throw new ArgumentNullException(nameof(t)));
            il.Emit(OpCodes.Ret);
            return (Func<T>)dynMethod.CreateDelegate(typeof(Func<T>));
        }

        private static Func<T, object> GetValue<T>(PropertyInfo propertyInfo)
        {
            var getAccessor = propertyInfo.GetGetMethod();

            if(getAccessor == null) throw new NullReferenceException(nameof(getAccessor));

            var methodName = "GetValue_" + propertyInfo.Name + "_" + GetNextCounterValue();
            var dynMethod = new DynamicMethod(methodName, typeof(T), new Type[] { typeof(object) },
                typeof(DynamicTypeHelper).Module);
            ILGenerator il = dynMethod.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.EmitCall(OpCodes.Callvirt, getAccessor, null);
            if (propertyInfo.PropertyType.IsValueType)
            {
                il.Emit(OpCodes.Box, propertyInfo.PropertyType);
            }
            il.Emit(OpCodes.Ret);
            return (Func<T, object>)dynMethod.CreateDelegate(typeof(Func<T, object>));
        }

        private static Action<T, object?> SetValue<T>(PropertyInfo propertyInfo)
        {
            var setAccessor = propertyInfo.GetSetMethod();

            if (setAccessor == null) throw new NullReferenceException(nameof(setAccessor));

            var methodName = "SetValue_" + propertyInfo.Name + "_" + GetNextCounterValue();
            var dynMethod = new DynamicMethod(methodName, typeof(void),
                new Type[] { typeof(T), typeof(object) }, typeof(DynamicTypeHelper).Module);
            ILGenerator il = dynMethod.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            if (propertyInfo.PropertyType.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
            }
            il.EmitCall(OpCodes.Callvirt, setAccessor, null);
            il.Emit(OpCodes.Ret);
            return (Action<T, object?>)dynMethod.CreateDelegate(typeof(Action<T, object?>));
        }

        private static int GetNextCounterValue()
        {
            return Interlocked.Increment(ref _counter);
        }
    }
}
