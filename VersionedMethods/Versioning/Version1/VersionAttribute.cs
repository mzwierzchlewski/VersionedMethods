namespace VersionedMethods.Versioning.Version1;

public class VersionAttribute : Attribute, IComparable<VersionAttribute>
{    
    public int Version { get; }

    public VersionAttribute(int version)
    {
        Version = version;
    }

    public int CompareTo(VersionAttribute? other)
    {
        if (ReferenceEquals(this, other)) { return 0; }
        if (ReferenceEquals(null, other)) { return 1; }
        return Version.CompareTo(other.Version);
    }
    
   
    public static bool operator <=(VersionAttribute? left, int right)
    {
        return left?.Version <= right;
    }
    
    public static bool operator >=(VersionAttribute? left, int right)
    {
        return left?.Version >= right;
    }

}