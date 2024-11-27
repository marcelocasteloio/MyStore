using System.Collections.Immutable;
using MCIO.BuildingBlocks.Output.Enums;
using MCIO.BuildingBlocks.Output.Models;

namespace MCIO.BuildingBlocks.Output;

public readonly struct Output<TValue>
{
    // Properties
    public Status Status { get; }
    public TValue? Value { get; }
    public ImmutableArray<Message>? MessageImmutableArray { get; }
    public ImmutableArray<Exception>? ExceptionImmutableArray { get; }
    
    // Constructors
    private Output(
        Status status, 
        TValue? value
    )
    {
        Status = status;
        Value = value;
    }
    private Output(
        Status status, 
        TValue? value, 
        ImmutableArray<Message>? messageImmutableArray,
        ImmutableArray<Exception>? exceptionImmutableArray
    ) : this(status, value)
    {
        MessageImmutableArray = messageImmutableArray;
        ExceptionImmutableArray = exceptionImmutableArray;
    }
    private Output(
        Status status, 
        TValue? value, 
        Message[]? messageCollection,
        Exception[]? exceptionCollection
    ) : this(status, value)
    {
        if(messageCollection is not null)
            MessageImmutableArray = [..messageCollection];
        
        if(exceptionCollection is not null)
            ExceptionImmutableArray = [..exceptionCollection];
    }

    // Public Methods
    public Output AsOutput()
    {
        return Output.Create(
            Status,
            MessageImmutableArray,
            ExceptionImmutableArray
        );
    }
    
    // Static factory methods    
    public static Output<TValue?> Create(Status status, TValue? value, ImmutableArray<Message>? messageImmutableArray = null, ImmutableArray<Exception>? exceptionImmutableArray = null)
        => new(status, value, messageImmutableArray, exceptionImmutableArray);
    
    public static Output<TValue?> Create(Status status, TValue? value, Message[]? messageCollection = null, Exception[]? exceptionCollection = null)
        => new(status, value, messageCollection, exceptionCollection);
    
    public static Output<TValue?> Create(Status status, TValue? value, MessageType messageType, string messageCode, string? messageDescription = null, Exception[]? exceptionCollection = null)
        => new(status, value, messageCollection: [ Message.Create(messageType, messageCode, messageDescription) ], exceptionCollection);
    
    public static Output<TValue?> Create(TValue? value = default, Message[]? messageCollection = null, Exception[]? exceptionCollection = null)
        => new(status: AnalyzeStatus(messageCollection, exceptionCollection), value, messageCollection, exceptionCollection);
    
    public static Output<TValue?> CreateSuccess(TValue? value = default, Message[]? messageCollection = null, Exception[]? exceptionCollection = null)
        => new(Status.Success, value, messageCollection, exceptionCollection);
    
    public static Output<TValue?> CreateSuccess(TValue? value, MessageType messageType, string messageCode, string? messageDescription = null, Exception[]? exceptionCollection = null)
        => new(Status.Success, value, messageCollection: [ Message.Create(messageType, messageCode, messageDescription) ], exceptionCollection);
    
    public static Output<TValue?> CreateSuccess(TValue? value, string messageCode, string? messageDescription = null, Exception[]? exceptionCollection = null)
        => CreateSuccess(value, messageType: MessageType.Success, messageCode, messageDescription, exceptionCollection);
    
    public static Output<TValue?> CreatePartial(TValue? value = default, Message[]? messageCollection = null, Exception[]? exceptionCollection = null)
        => new(Status.Partial, value, messageCollection, exceptionCollection);

    public static Output<TValue?> CreatePartial(TValue? value, MessageType messageType, string messageCode, string? messageDescription = null, Exception[]? exceptionCollection = null)
        => new(Status.Partial, value, messageCollection: [ Message.Create(messageType, messageCode, messageDescription) ], exceptionCollection);

    public static Output<TValue?> CreatePartial(TValue? value, string messageCode, string? messageDescription = null, Exception[]? exceptionCollection = null)
        => CreatePartial(value, messageType: MessageType.Information, messageCode, messageDescription, exceptionCollection);
    
    public static Output<TValue?> CreateError(TValue? value = default, Message[]? messageCollection = null, Exception[]? exceptionCollection = null)
        => new(Status.Error, value, messageCollection, exceptionCollection);

    public static Output<TValue?> CreateError(TValue? value, MessageType messageType, string messageCode, string? messageDescription = null, Exception[]? exceptionCollection = null)
        => new(Status.Error, value, messageCollection: [ Message.Create(messageType, messageCode, messageDescription) ], exceptionCollection);

    public static Output<TValue?> CreateError(TValue? value, string messageCode, string? messageDescription = null, Exception[]? exceptionCollection = null)
        => CreateError(value, messageType: MessageType.Error, messageCode, messageDescription, exceptionCollection);
    
    public static Output<TValue?> CreateFromException(Exception exception, TValue? value, MessageType messageType, string messageCode, string? messageDescription = null)
        => CreateError(value, messageType, messageCode, messageDescription, exceptionCollection: [exception]);
    
    public static Output<TValue?> CreateErrorFromException(Exception exception, TValue? value, string messageCode, string? messageDescription = null)
        => CreateFromException(exception, value, messageType: MessageType.Error, messageCode, messageDescription: messageDescription ?? exception.Message);

    public static Output<TValue?> CreateErrorFromException(Exception exception)
        => CreateErrorFromException(exception, value: default, messageCode: exception.GetType().FullName ?? exception.GetType().Name);
    
    // Private Methods
    private static Status AnalyzeStatus(Message[]? messageCollection, Exception[]? exceptionCollection)
    {
        var hasErrorMessage = false;
        var hasSuccessMessage = false;
        var hasExceptions = false;

        if (messageCollection is not null)
            for (var i = 0; i < messageCollection.Length; i++)
            {
                var messageType = messageCollection[i].Type;
                
                if (messageType == MessageType.Error)
                    hasErrorMessage = true;
                else if (messageType == MessageType.Success)
                    hasSuccessMessage = true;
                
                if(hasErrorMessage && hasSuccessMessage)
                    break;
            }

        if (exceptionCollection is not null)
            hasExceptions = exceptionCollection.Length > 0;

        if (!hasErrorMessage && !hasExceptions)
            return Status.Success;
        
        if ((hasErrorMessage || hasExceptions) && hasSuccessMessage)
            return Status.Partial;
        
        return Status.Error;
    }
}