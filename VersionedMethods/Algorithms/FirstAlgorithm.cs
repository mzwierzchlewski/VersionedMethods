using VersionedMethods.Versioning.Version1;

namespace VersionedMethods.Algorithms;

public interface IFirstAlgorithm : IVersionedAlgorithm<string, int>
{
}

public class FirstAlgorithm : BaseVersionedAlgorithm<string, int, VersionAttribute>, IFirstAlgorithm
{
    [Version(0)]
    private string V0(int number) =>
        number switch
        {
            0 => "zero",
            _ => throw new ArgumentOutOfRangeException(nameof(number))
        };
    
    [Version(1)]
    private string V1(int number) =>
        number switch
        {
            0 => "zero",
            1 => "one",
            _ => throw new ArgumentOutOfRangeException(nameof(number))
        };
    
    [Version(2)]
    private string V2(int number) =>
        number switch
        {
            0 => "zero",
            1 => "one",
            2 => "two",
            _ => throw new ArgumentOutOfRangeException(nameof(number))
        };
}