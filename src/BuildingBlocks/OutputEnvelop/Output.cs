using System.Collections.Immutable;
using MCIO.BuildingBlocks.OutputEnvelop.Enums;
using MCIO.BuildingBlocks.OutputEnvelop.Models;

namespace MCIO.BuildingBlocks.OutputEnvelop;

public readonly struct Output
{
    // Internal Output<object?> instance
    private readonly Output<object?> _innerOutput;

    // Properties
    public Status Status => _innerOutput.Status;
    public ImmutableArray<Message>? MessageCollection => _innerOutput.MessageImmutableArray;
    public ImmutableArray<Exception>? ExceptionCollection => _innerOutput.ExceptionImmutableArray;
    
    public bool IsSuccess => _innerOutput.IsSuccess;
    public bool IsPartial => _innerOutput.IsPartial;
    public bool IsError => _innerOutput.IsError;
    public bool HasMessage => _innerOutput.HasMessage;
    public bool HasException => _innerOutput.HasException;

    // Constructor
    private Output(Output<object?> innerOutput)
    {
        _innerOutput = innerOutput;
    }

    // Implicit Operators
    public static implicit operator Output(Output<object?> innerOutput) => new(innerOutput);
    
    // Static factory methods

    public static Output Create(Status status, ImmutableArray<Message>? messageImmutableArray = null, ImmutableArray<Exception>? exceptionImmutableArray = null)
        => new(Output<object?>.Create(status, value: null, messageImmutableArray, exceptionImmutableArray));
    
    public static Output Create(Status status, Message[]? messageCollection = null, Exception[]? exceptionCollection = null)
        => new(Output<object?>.Create(status, value: null, messageCollection, exceptionCollection));

    public static Output Create(Status status, MessageType messageType, string messageCode, string? messageDescription = null, Exception[]? exceptionCollection = null)
        => new(Output<object?>.Create(status, value: null, messageType, messageCode, messageDescription, exceptionCollection));

    public static Output Create(Message[]? messageCollection = null, Exception[]? exceptionCollection = null)
        => new(Output<object?>.Create(value: null, messageCollection, exceptionCollection));

    public static Output Create(params Output[] outputCollection)
        => new(Output<object?>.Create(value: null, outputCollection));

    public static Output CreateSuccess(Message[]? messageCollection = null, Exception[]? exceptionCollection = null)
        => new(Output<object?>.CreateSuccess(value: null, messageCollection, exceptionCollection));

    public static Output CreateSuccess(MessageType messageType, string messageCode, string? messageDescription = null, Exception[]? exceptionCollection = null)
        => new(Output<object?>.CreateSuccess(value: null, messageType, messageCode, messageDescription, exceptionCollection));

    public static Output CreateSuccess(string messageCode, string? messageDescription = null, Exception[]? exceptionCollection = null)
        => new(Output<object?>.CreateSuccess(value: null, messageCode, messageDescription, exceptionCollection));
    
    public static Output CreateSuccess(params Output[] outputCollection)
        => new(Output<object?>.CreateSuccess(value: null, outputCollection));

    public static Output CreatePartial(Message[]? messageCollection = null, Exception[]? exceptionCollection = null)
        => new(Output<object?>.CreatePartial(value: null, messageCollection, exceptionCollection));

    public static Output CreatePartial(MessageType messageType, string messageCode, string? messageDescription = null, Exception[]? exceptionCollection = null)
        => new(Output<object?>.CreatePartial(value: null, messageType, messageCode, messageDescription, exceptionCollection));

    public static Output CreatePartial(string messageCode, string? messageDescription = null, Exception[]? exceptionCollection = null)
        => new(Output<object?>.CreatePartial(value: null, messageCode, messageDescription, exceptionCollection));
    
    public static Output CreatePartial(params Output[] outputCollection)
        => new(Output<object?>.CreatePartial(value: null, outputCollection));

    public static Output CreateError(Message[]? messageCollection = null, Exception[]? exceptionCollection = null)
        => new(Output<object?>.CreateError(value: null, messageCollection, exceptionCollection));

    public static Output CreateError(MessageType messageType, string messageCode, string? messageDescription = null, Exception[]? exceptionCollection = null)
        => new(Output<object?>.CreateError(value: null, messageType, messageCode, messageDescription, exceptionCollection));

    public static Output CreateError(string messageCode, string? messageDescription = null, Exception[]? exceptionCollection = null)
        => new(Output<object?>.CreateError(value: null, messageCode, messageDescription, exceptionCollection));
    
    public static Output CreateError(string messageCode, string? messageDescription = null, params Output[] outputCollection)
        => new(Output<object?>.CreateError(value: null, messageCode, messageDescription, outputCollection));
    
    public static Output CreateError(params Output[] outputCollection)
        => new(Output<object?>.CreateError(value: null, outputCollection));

    public static Output CreateFromException(Exception exception, MessageType messageType, string messageCode, string? messageDescription = null)
        => new(Output<object?>.CreateFromException(exception, value: null, messageType, messageCode, messageDescription));

    public static Output CreateErrorFromException(Exception exception, string messageCode, string? messageDescription = null)
        => new(Output<object?>.CreateErrorFromException(exception, value: null, messageCode, messageDescription));

    public static Output CreateErrorFromException(Exception exception)
        => new(Output<object?>.CreateErrorFromException(exception));
    
    public static Output CreateFromOutput(params Output[] outputCollection)
        => new(Output<object?>.CreateFromOutput(value: null, outputCollection));
    
    public static Output CreateFromOutput(Status status, params Output[] outputCollection)
        => new(Output<object?>.CreateFromOutput(value: null, status, outputCollection));
    
    public static Output CreateSuccessFromOutput(params Output[] outputCollection)
        => new(Output<object?>.CreateSuccessFromOutput(value: null, outputCollection));
    
    public static Output CreateErrorFromOutput(params Output[] outputCollection)
        => new(Output<object?>.CreateErrorFromOutput(value: null, outputCollection));
    
    public static Output CreatePartialFromOutput(params Output[] outputCollection)
        => new(Output<object?>.CreatePartialFromOutput(value: null, outputCollection));
}
