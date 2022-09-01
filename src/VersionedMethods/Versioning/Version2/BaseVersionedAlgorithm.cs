using System.Reflection;

namespace VersionedMethods.Versioning.Version2;

public abstract class BaseVersionedAlgorithm<TReturn, TParam, TVersion> : IVersionedAlgorithm<TReturn, TParam> where TVersion : VersionAttribute
{
    private readonly (int version, Func<TParam, TReturn> method)[] _methods;

    protected BaseVersionedAlgorithm()
    {
        _methods = GetMethods();
    }
    
    public TReturn? Run(TParam parameter, int version)
    {
        bool MatchingVersion((int version, Func<TParam, TReturn>) m) => m.version <= version;

        var (_, method) = _methods.FirstOrDefault(MatchingVersion);
        if (method == null)
        {
            return default;
        }

        return method.Invoke(parameter);
    }

    private (int version, Func<TParam, TReturn> method)[] GetMethods()
    {
        TVersion? GetVersionAttribute(MethodInfo m) => m.GetCustomAttribute<TVersion>();
        bool FilterMethods(MethodInfo m)
        {
            var versionAttribute = GetVersionAttribute(m);
            if (versionAttribute is null)
            {
                return false;
            }
                
            var parameters = m.GetParameters();
            return m.ReturnType == typeof(TReturn)
                   && parameters.Length is 1
                   && parameters.First().ParameterType == typeof(TParam);
        }

        var methods = GetType()
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(FilterMethods)
            .OrderByDescending(GetVersionAttribute);

        return methods.Select(m => 
                (GetVersionAttribute(m)!.Version,
                (Func<TParam, TReturn>) Delegate.CreateDelegate(typeof(Func<TParam, TReturn>), this, m)))
            .ToArray();
    }
}