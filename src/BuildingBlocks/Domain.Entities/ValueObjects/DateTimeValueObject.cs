namespace MCIO.BuildingBlocks.Domain.Entities.ValueObjects;

public readonly struct DateTimeValueObject 
    : IEquatable<DateTimeValueObject>
{
    // Properties
    public DateTimeOffset Value { get; }
    
    // Constructors
    private DateTimeValueObject(DateTimeOffset value)
    {
        Value = value;
    }
    private DateTimeValueObject(DateTime value)
    {
        Value = new DateTimeOffset(value);
    }
    private DateTimeValueObject(long ticks)
    {
        Value = new DateTimeOffset(ticks, TimeSpan.Zero);
    }
    
    // Builders
    public static DateTimeValueObject Now()
    {
        return new DateTimeValueObject(DateTimeOffset.UtcNow);
    }
    public static DateTimeValueObject FromExistingInfo(DateTimeOffset value)
    {
        return new DateTimeValueObject(value);
    }
    public static DateTimeValueObject FromExistingInfo(DateTime value)
    {
        return new DateTimeValueObject(value);
    }
    public static DateTimeValueObject FromExistingInfo(long ticks)
    {
        return new DateTimeValueObject(ticks);
    }
    
    // Public Methods
    public DateOnly ToDateOnly() => DateOnly.FromDateTime(Value.DateTime);
    
    // Implicit Operators
    public static implicit operator DateTimeOffset(DateTimeValueObject dateTimeValueObject) => dateTimeValueObject.Value;
    public static implicit operator DateTime(DateTimeValueObject dateTimeValueObject) => dateTimeValueObject.Value.DateTime;
    public static implicit operator long(DateTimeValueObject dateTimeValueObject) => dateTimeValueObject.Value.UtcTicks;
    
    public static implicit operator DateTimeValueObject(DateTimeOffset value) => new (value);
    public static implicit operator DateTimeValueObject(DateTime value) => new (value);
    public static implicit operator DateTimeValueObject(long value) => new (value);
    
    // Operators Overloading
    public static bool operator ==(DateTimeValueObject left, DateTimeValueObject right) => left.Value == right.Value;
    public static bool operator !=(DateTimeValueObject left, DateTimeValueObject right) => !(left == right);
    
    // Equality
    public bool Equals(DateTimeValueObject other)
    {
        return Value.Equals(other.Value);
    }
    public override bool Equals(object? obj)
    {
        return obj is DateTimeValueObject other && Equals(other);
    }
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}