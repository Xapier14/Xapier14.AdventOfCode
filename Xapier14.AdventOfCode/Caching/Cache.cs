using System.Dynamic;
using System.Reflection;

namespace Xapier14.AdventOfCode.Caching
{
    public class Cache : DynamicObject
    {
        private static readonly Cache _singleton = new();
        private static readonly Dictionary<string, MethodInfo> _sharedMethods = new();
        private readonly Dictionary<string, MethodInfo> _methods = new();
        private readonly Dictionary<string, Dictionary<long, object>> _caches = new();
        public static dynamic Shared => _singleton;

        static Cache()
        {
            var methods = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetLoadableTypes())
                .Where(type => type != null).Select(type => type!)
                .SelectMany(type => type.GetMethods())
                .Where(method => method.GetCustomAttribute<CachedAttribute>() != null);
            foreach (var method in methods)
                _sharedMethods.Add(method.Name, method);
        }

        public void AddFunction(string methodName, Delegate method)
            => _methods.Add(methodName, method.Method);

        public object InvokeFunction(string methodName, object?[] args)
        {
            if (!_sharedMethods.TryGetValue(methodName, out var method))
                if (!_methods.TryGetValue(methodName, out method))
                    throw new Exception("Method not cached.");
            if (!_caches.ContainsKey(methodName))
                _caches.Add(methodName, new Dictionary<long, object>());

            var argHash = Utility.Hash(args);
            if (_caches[methodName].TryGetValue(argHash, out var returnValue))
                return returnValue;

            returnValue = method.Invoke(null, args);
            _caches[methodName].Add(argHash, returnValue!);
            return returnValue!;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
        {
            var methodName = binder.Name;
            args ??= Array.Empty<object?>();
            if (!_sharedMethods.TryGetValue(methodName, out var method))
                if (!_methods.TryGetValue(methodName, out method))
                    return base.TryInvokeMember(binder, args, out result);
            if (!_caches.ContainsKey(methodName))
                _caches.Add(methodName, new Dictionary<long, object>());

            var argHash = Utility.Hash(args);
            if (_caches[methodName].TryGetValue(argHash, out var returnValue))
            {
                result = returnValue;
                return true;
            }

            returnValue = method.Invoke(null, args);
            _caches[methodName].Add(argHash, returnValue!);
            result = returnValue;
            return true;
        }
    }
}
