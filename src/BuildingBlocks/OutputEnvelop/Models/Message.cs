using MCIO.BuildingBlocks.OutputEnvelop.Enums;

namespace MCIO.BuildingBlocks.OutputEnvelop.Models;

public readonly struct Message
{
    // Properties
    public MessageType Type { get; }
    public string Code { get; }
    public string? Description { get; }

    // Constructors
    private Message(MessageType type, string code, string? description)
    {
        // Validating the Type with a range check for performance reasons instead of Enum.IsDefined.
        // See EnumValueComparerBenchmark in benchmark project.
        // Note: This requires maintenance if the enum values change.
        var typeValue = (byte)type;
        if(typeValue is < 1 or > 4)
            throw new ArgumentOutOfRangeException(nameof(type), "Invalid message type");

        ArgumentNullException.ThrowIfNull(argument: code, paramName: nameof(code));

        Type = type;
        Code = code;
        Description = description;
    }

    // Static factory methods
    public static Message Create(MessageType type, string code, string? description = null)
    {
        return new Message(type, code, description);
    }

    public static Message CreateInformation(string code, string? description = null)
    {
        return new Message(MessageType.Information, code, description);
    }

    public static Message CreateSuccess(string code, string? description = null)
    {
        return new Message(MessageType.Success, code, description);
    }

    public static Message CreateWarning(string code, string? description = null)
    {
        return new Message(MessageType.Warning, code, description);
    }

    public static Message CreateError(string code, string? description = null)
    {
        return new Message(MessageType.Error, code, description);
    }
}