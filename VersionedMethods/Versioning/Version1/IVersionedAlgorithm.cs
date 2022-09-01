namespace VersionedMethods.Versioning.Version1;

public interface IVersionedAlgorithm<out TReturn, in TParam>
{
    TReturn? Run(TParam parameter, int version);
}