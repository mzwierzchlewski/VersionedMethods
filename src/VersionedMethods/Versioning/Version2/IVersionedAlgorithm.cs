namespace VersionedMethods.Versioning.Version2;

public interface IVersionedAlgorithm<out TReturn, in TParam>
{
    TReturn? Run(TParam parameter, int version);
}