using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

public static class ReflectionHelper
{
    public static IEnumerable<Type> FindDerivedTypes(Type baseType)
    {
        return baseType.Assembly.GetTypes().Where(t => baseType.IsAssignableFrom(t) && !t.IsAbstract);
    }

    public static bool IsOrInheriting<T>(this Type type)
    {
        return typeof(T).IsAssignableFrom(type);
    }
}

