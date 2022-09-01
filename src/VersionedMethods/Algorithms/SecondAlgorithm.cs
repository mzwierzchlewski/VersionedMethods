using VersionedMethods.Versioning.Version2;

namespace VersionedMethods.Algorithms;

public interface ISecondAlgorithm : IVersionedAlgorithm<string, int>
{
}

public class SecondAlgorithm : BaseVersionedAlgorithm<string, int, VersionAttribute>, ISecondAlgorithm
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