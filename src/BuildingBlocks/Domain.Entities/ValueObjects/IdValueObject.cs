namespace MCIO.BuildingBlocks.Domain.Entities.ValueObjects;

public readonly struct IdValueObject 
    : IEquatable<IdValueObject>
{
    // Properties
    public Guid Value { get; }
    
    // Constructors
    private IdValueObject(Guid value)
    {
        Value = value;
    }
    
    // Builders
    public static IdValueObject GenerateNew()
    {
        return new IdValueObject(
            value: Guid.CreateVersion7()
        );
    }
    public static IdValueObject FromExistingInfo(Guid value)
    {
        return new IdValueObject(value);
    }
    
    // Implicit Operators
    public static implicit operator Guid(IdValueObject idValueObject) => idValueObject.Value;
    public static implicit operator IdValueObject(Guid value) => new(value);
    
    // Operators Overloading
    public static bool operator ==(IdValueObject left, IdValueObject right) => left.Value == right.Value;
    public static bool operator !=(IdValueObject left, IdValueObject right) => !(left == right);
    
    // Equality
    public bool Equals(IdValueObject other)
    {
        return Value.Equals(other.Value);
    }
    public override bool Equals(object? obj)
    {
        return obj is IdValueObject other && Equals(other);
    }
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}