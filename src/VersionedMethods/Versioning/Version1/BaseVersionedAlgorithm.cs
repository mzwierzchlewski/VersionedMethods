using System.Reflection;

namespace VersionedMethods.Versioning.Version1;

public abstract class BaseVersionedAlgorithm<TReturn, TParam, TVersion> : IVersionedAlgorithm<TReturn, TParam> where TVersion : VersionAttribute
{
    public TReturn? Run(TParam parameter, int version)
    {
        TVersion? GetVersionAttribute(MethodInfo m) => m.GetCustomAttribute<TVersion>();
        bool FilterMethods(MethodInfo m)
        {
            var hcqVersionAttribute = GetVersionAttribute(m);
            if (hcqVersionAttribute is null)
            {
                return false;
            }
                
            var parameters = m.GetParameters();
            return m.ReturnType == typeof(TReturn)
                   && parameters.Length is 1
                   && parameters.First().ParameterType == typeof(TParam);
        }
        bool MatchingVersion(MethodInfo m) => GetVersionAttribute(m)! <= version;

        var method = GetType()
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(FilterMethods)
            .OrderByDescending(GetVersionAttribute)
            .FirstOrDefault(MatchingVersion);

        if (method == null)
        {
            return default;
        }

        return (TReturn) method.Invoke(this, new object[] { parameter })!;
    }
}