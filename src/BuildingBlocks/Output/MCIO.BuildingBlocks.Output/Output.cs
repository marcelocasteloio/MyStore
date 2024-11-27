using MCIO.BuildingBlocks.Output.Enums;
using MCIO.BuildingBlocks.Output.Models;

using System.Collections.Immutable;
using MCIO.BuildingBlocks.Output.Enums;
using MCIO.BuildingBlocks.Output.Models;

namespace MCIO.BuildingBlocks.Output;

public readonly struct Output
{
    // Internal Output<object?> instance
    private readonly Output<object?> _innerOutput;

    // Properties
    public Status Status => _innerOutput.Status;
    public ImmutableArray<Message>? MessageCollection => _innerOutput.MessageImmutableArray;
    public ImmutableArray<Exception>? ExceptionCollection => _innerOutput.ExceptionImmutableArray;

    // Constructor
    private Output(Output<object?> innerOutput)
    {
        _innerOutput = innerOutput;
    }

    // Static factory methods

    public static Output Create(Status status, ImmutableArray<Message>? messageImmutableArray = null, ImmutableArray<Exception>? exceptionImmutableArray = null)
        => new(Output<object?>.Create(status, value: null, messageImmutableArray, exceptionImmutableArray));
    
    public static Output Create(Status status, Message[]? messageCollection = null, Exception[]? exceptionCollection = null)
        => new(Output<object?>.Create(status, value: null, messageCollection, exceptionCollection));

    public static Output Create(Status status, MessageType messageType, string messageCode, string? messageDescription = null, Exception[]? exceptionCollection = null)
        => new(Output<object?>.Create(status, value: null, messageType, messageCode, messageDescription, exceptionCollection));

    public static Output Create(Message[]? messageCollection = null, Exception[]? exceptionCollection = null)
        => new(Output<object?>.Create(value: null, messageCollection, exceptionCollection));

    public static Output CreateSuccess(Message[]? messageCollection = null, Exception[]? exceptionCollection = null)
        => new(Output<object?>.CreateSuccess(value: null, messageCollection, exceptionCollection));

    public static Output CreateSuccess(MessageType messageType, string messageCode, string? messageDescription = null, Exception[]? exceptionCollection = null)
        => new(Output<object?>.CreateSuccess(value: null, messageType, messageCode, messageDescription, exceptionCollection));

    public static Output CreateSuccess(string messageCode, string? messageDescription = null, Exception[]? exceptionCollection = null)
        => new(Output<object?>.CreateSuccess(value: null, messageCode, messageDescription, exceptionCollection));

    public static Output CreatePartial(Message[]? messageCollection = null, Exception[]? exceptionCollection = null)
        => new(Output<object?>.CreatePartial(value: null, messageCollection, exceptionCollection));

    public static Output CreatePartial(MessageType messageType, string messageCode, string? messageDescription = null, Exception[]? exceptionCollection = null)
        => new(Output<object?>.CreatePartial(value: null, messageType, messageCode, messageDescription, exceptionCollection));

    public static Output CreatePartial(string messageCode, string? messageDescription = null, Exception[]? exceptionCollection = null)
        => new(Output<object?>.CreatePartial(value: null, messageCode, messageDescription, exceptionCollection));

    public static Output CreateError(Message[]? messageCollection = null, Exception[]? exceptionCollection = null)
        => new(Output<object?>.CreateError(value: null, messageCollection, exceptionCollection));

    public static Output CreateError(MessageType messageType, string messageCode, string? messageDescription = null, Exception[]? exceptionCollection = null)
        => new(Output<object?>.CreateError(value: null, messageType, messageCode, messageDescription, exceptionCollection));

    public static Output CreateError(string messageCode, string? messageDescription = null, Exception[]? exceptionCollection = null)
        => new(Output<object?>.CreateError(value: null, messageCode, messageDescription, exceptionCollection));

    public static Output CreateFromException(Exception exception, MessageType messageType, string messageCode, string? messageDescription = null)
        => new(Output<object?>.CreateFromException(exception, value: null, messageType, messageCode, messageDescription));

    public static Output CreateErrorFromException(Exception exception, string messageCode, string? messageDescription = null)
        => new(Output<object?>.CreateErrorFromException(exception, value: null, messageCode, messageDescription));

    public static Output CreateErrorFromException(Exception exception)
        => new(Output<object?>.CreateErrorFromException(exception));
}
