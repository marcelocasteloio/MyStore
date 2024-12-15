namespace MCIO.BuildingBlocks.Domain.Entities.ValueObjects;

public readonly struct RegistryVersionValueObject
    : IEquatable<RegistryVersionValueObject>
{
    // Properties
    public long Value { get; }
    
    // Constructors
    private RegistryVersionValueObject(long value)
    {
        Value = value;
    }
    
    // Builders
    public static RegistryVersionValueObject GenerateNew()
    {
        return new RegistryVersionValueObject(
            value: DateTimeValueObject.Now()
        );
    }
    public static RegistryVersionValueObject FromExistingInfo(long value)
    {
        return new RegistryVersionValueObject(value);
    }
    
    // Implicit Operators
    public static implicit operator long(RegistryVersionValueObject registryVersionValueObject) => registryVersionValueObject.Value;
    public static implicit operator RegistryVersionValueObject(long value) => new(value);

    // Operators Overloading
    public static bool operator ==(RegistryVersionValueObject left, RegistryVersionValueObject right) => left.Value == right.Value;
    public static bool operator !=(RegistryVersionValueObject left, RegistryVersionValueObject right) => !(left == right);
    
    // Equality
    public bool Equals(RegistryVersionValueObject other)
    {
        return Value.Equals(other.Value);
    }
    public override bool Equals(object? obj)
    {
        return obj is RegistryVersionValueObject other && Equals(other);
    }
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}