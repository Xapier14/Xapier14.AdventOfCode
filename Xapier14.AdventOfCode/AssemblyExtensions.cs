﻿﻿using System.Reflection;

namespace Xapier14.AdventOfCode;

public static class AssemblyExtensions
{
    public static IEnumerable<Type?> GetLoadableTypes(this Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            return e.Types.Where(t => t != null).ToArray();
        }
    }
}