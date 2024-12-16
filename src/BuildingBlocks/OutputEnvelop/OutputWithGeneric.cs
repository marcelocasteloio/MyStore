using System.Collections.Immutable;
using MCIO.BuildingBlocks.OutputEnvelop.Enums;
using MCIO.BuildingBlocks.OutputEnvelop.Models;

namespace MCIO.BuildingBlocks.OutputEnvelop;

public readonly struct Output<TValue>
{
    // Properties
    public bool IsSuccess => Status == Status.Success;
    public bool IsPartial => Status == Status.Partial;
    public bool IsError => Status == Status.Error;
    public bool HasMessage => MessageImmutableArray is not null && MessageImmutableArray.Value.Length > 0;
    public bool HasException => ExceptionImmutableArray is not null && ExceptionImmutableArray.Value.Length > 0;

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
        if (messageCollection is not null)
            MessageImmutableArray = [..messageCollection];

        if (exceptionCollection is not null)
            ExceptionImmutableArray = [..exceptionCollection];
    }

    // Implicit Operators
    public static implicit operator Output(Output<TValue?> output) => output.AsOutput();
    public static implicit operator Output<TValue?>(Output output) => CreateFromOutput(value: default, output);

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
    public static Output<TValue?> Create(Status status, TValue? value,
        ImmutableArray<Message>? messageImmutableArray = null,
        ImmutableArray<Exception>? exceptionImmutableArray = null)
        => new(status, value, messageImmutableArray, exceptionImmutableArray);

    public static Output<TValue?> Create(Status status, TValue? value, Message[]? messageCollection = null,
        Exception[]? exceptionCollection = null)
        => new(status, value, messageCollection, exceptionCollection);

    public static Output<TValue?> Create(Status status, TValue? value, MessageType messageType, string messageCode,
        string? messageDescription = null, Exception[]? exceptionCollection = null)
        => new(status, value, messageCollection: [Message.Create(messageType, messageCode, messageDescription)],
            exceptionCollection);

    public static Output<TValue?> Create(TValue? value = default, Message[]? messageCollection = null,
        Exception[]? exceptionCollection = null)
        => new(status: CreateStatus(messageCollection, exceptionCollection), value, messageCollection,
            exceptionCollection);

    public static Output<TValue?> Create(TValue? value = default, params Output[] outputCollection)
    {
        var messageCollection = JoinMessageCollection(outputCollection);
        var exceptionCollection = JoinExceptionCollection(outputCollection);
        
        return new(
            status: CreateStatus(messageCollection, exceptionCollection), 
            value, 
            messageCollection,
            exceptionCollection
        );
    }

    public static Output<TValue?> CreateSuccess(TValue? value = default, Message[]? messageCollection = null,
        Exception[]? exceptionCollection = null)
        => new(Status.Success, value, messageCollection, exceptionCollection);

    public static Output<TValue?> CreateSuccess(TValue? value, MessageType messageType, string messageCode,
        string? messageDescription = null, Exception[]? exceptionCollection = null)
        => new(Status.Success, value, messageCollection: [Message.Create(messageType, messageCode, messageDescription)],
            exceptionCollection);

    public static Output<TValue?> CreateSuccess(TValue? value, string messageCode, string? messageDescription = null,
        Exception[]? exceptionCollection = null)
        => CreateSuccess(value, messageType: MessageType.Success, messageCode, messageDescription, exceptionCollection);

    public static Output<TValue?> CreateSuccess(TValue? value, params Output[] outputCollection)
        => CreateSuccess(value, messageCollection: JoinMessageCollection(outputCollection), exceptionCollection: JoinExceptionCollection(outputCollection));
    
    public static Output<TValue?> CreatePartial(TValue? value = default, Message[]? messageCollection = null,
        Exception[]? exceptionCollection = null)
        => new(Status.Partial, value, messageCollection, exceptionCollection);

    public static Output<TValue?> CreatePartial(TValue? value, MessageType messageType, string messageCode,
        string? messageDescription = null, Exception[]? exceptionCollection = null)
        => new(Status.Partial, value, messageCollection: [Message.Create(messageType, messageCode, messageDescription)],
            exceptionCollection);

    public static Output<TValue?> CreatePartial(TValue? value, string messageCode, string? messageDescription = null,
        Exception[]? exceptionCollection = null)
        => CreatePartial(value, messageType: MessageType.Information, messageCode, messageDescription,
            exceptionCollection);
    
    public static Output<TValue?> CreatePartial(TValue? value, params Output[] outputCollection)
        => CreatePartial(value, messageCollection: JoinMessageCollection(outputCollection), exceptionCollection: JoinExceptionCollection(outputCollection));

    public static Output<TValue?> CreateError(TValue? value = default, Message[]? messageCollection = null,
        Exception[]? exceptionCollection = null)
        => new(Status.Error, value, messageCollection, exceptionCollection);

    public static Output<TValue?> CreateError(TValue? value, MessageType messageType, string messageCode,
        string? messageDescription = null, Exception[]? exceptionCollection = null)
        => new(Status.Error, value, messageCollection: [Message.Create(messageType, messageCode, messageDescription)],
            exceptionCollection);

    public static Output<TValue?> CreateError(TValue? value, string messageCode, string? messageDescription = null,
        Exception[]? exceptionCollection = null)
        => CreateError(value, messageType: MessageType.Error, messageCode, messageDescription, exceptionCollection);

    public static Output<TValue?> CreateError(TValue? value, params Output[] outputCollection)
        => CreateError(value, messageCollection: JoinMessageCollection(outputCollection), exceptionCollection: JoinExceptionCollection(outputCollection));

    public static Output<TValue?> CreateError(TValue? value, string messageCode, string? messageDescription = null, params Output[] outputCollection)
    {
        var messageCollection = JoinMessageCollection(
            messageToJoin: Message.CreateError(messageCode, messageDescription),
            outputCollection
        );

        return CreateError(
            value,
            messageCollection: messageCollection,
            exceptionCollection: JoinExceptionCollection(outputCollection)
        );
    }
    
    public static Output<TValue?> CreateFromException(Exception exception, TValue? value, MessageType messageType,
        string messageCode, string? messageDescription = null)
        => CreateError(value, messageType, messageCode, messageDescription, exceptionCollection: [exception]);

    public static Output<TValue?> CreateErrorFromException(Exception exception, TValue? value, string messageCode,
        string? messageDescription = null)
        => CreateFromException(exception, value, messageType: MessageType.Error, messageCode,
            messageDescription: messageDescription ?? exception.Message);

    public static Output<TValue?> CreateErrorFromException(Exception exception)
        => CreateErrorFromException(
            exception,
            value: default,
            messageCode: exception.GetType().FullName ?? exception.GetType().Name
        );

    public static Output<TValue?> CreateFromOutput(TValue? value, params Output[] outputCollection)
    {
        var newMessageCollection = JoinMessageCollection(outputCollection);
        var newExceptionCollection = JoinExceptionCollection(outputCollection);

        return Create(
            status: CreateStatus(newMessageCollection, newExceptionCollection),
            value: value,
            messageCollection: newMessageCollection,
            exceptionCollection: newExceptionCollection
        );
    }

    public static Output<TValue?> CreateFromOutput(TValue? value, Status status, params Output[] outputCollection)
    {
        var newMessageCollection = JoinMessageCollection(outputCollection);
        var newExceptionCollection = JoinExceptionCollection(outputCollection);

        return Create(
            status: status,
            value: value,
            messageCollection: newMessageCollection,
            exceptionCollection: newExceptionCollection
        );
    }

    public static Output<TValue?> CreateSuccessFromOutput(TValue? value, params Output[] outputCollection)
        => CreateFromOutput(value, Status.Success, outputCollection);

    public static Output<TValue?> CreateErrorFromOutput(TValue? value, params Output[] outputCollection)
        => CreateFromOutput(value, Status.Error, outputCollection);

    public static Output<TValue?> CreatePartialFromOutput(TValue? value, params Output[] outputCollection)
        => CreateFromOutput(value, Status.Partial, outputCollection);
    
    public static Output<TValue?> Execute(Func<Output<TValue?>> handler)
    {
        try
        {
            return handler();
        }
        catch (Exception ex)
        {
            return CreateErrorFromException(ex);
        }
    }
    public static Output<TValue?> Execute<TInput>(TInput input, Func<TInput, Output<TValue?>> handler)
    {
        try
        {
            return handler(input);
        }
        catch (Exception ex)
        {
            return CreateErrorFromException(ex);
        }
    }
    
    public static async Task<Output<TValue?>> ExecuteAsync(Func<CancellationToken, Task<Output<TValue?>>> handler, CancellationToken cancellationToken)
    {
        try
        {
            return await handler(cancellationToken);
        }
        catch (Exception ex)
        {
            return CreateErrorFromException(ex);
        }
    }
    public static async Task<Output<TValue?>> ExecuteAsync<TInput>(TInput input, Func<TInput, CancellationToken, Task<Output<TValue?>>> handler, CancellationToken cancellationToken)
    {
        try
        {
            return await handler(input, cancellationToken);
        }
        catch (Exception ex)
        {
            return CreateErrorFromException(ex);
        }
    }
    
    // Private Methods
    private static Status CreateStatus(Message[]? messageCollection, Exception[]? exceptionCollection)
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

                if (hasErrorMessage && hasSuccessMessage)
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
    // private static Status CreateStatus(params Output[] outputCollection)
    // {
    //     var hasSuccess = false;
    //     var hasError = false;
    //     var hasPartial = false;
    //
    //     for (var i = 0; i < outputCollection.Length; i++)
    //     {
    //         var output = outputCollection[i];
    //         var status = output.Status;
    //
    //         if (status == Status.Partial)
    //         {
    //             hasPartial = true;
    //             break;
    //         }
    //         else if(status == Status.Error)
    //             hasError = true;
    //         else if (status == Status.Success)
    //             hasSuccess = true;
    //     }
    //
    //     if(hasPartial)
    //         return Status.Partial;
    //     else if(hasSuccess && hasError)
    //         return Status.Partial;
    //     else if(hasSuccess)
    //         return Status.Success;
    //     else
    //         return Status.Error;
    // }

    private static Message[]? JoinMessageCollection(params Output[] outputCollection)
    {
        // Analyze the output collection to determine the size of the message collection
        var messageCollectionCount = 0;
        for (var outputCollectionIndex = 0; outputCollectionIndex < outputCollection.Length; outputCollectionIndex++)
        {
            var output = outputCollection[outputCollectionIndex];

            if (output.MessageCollection is null)
                continue;

            messageCollectionCount += output.MessageCollection.Value.Length;
        }

        // Create the message collection
        if (messageCollectionCount == 0)
            return null;

        var messageCollection = new Message[messageCollectionCount];
        var lastMessageIndex = 0;

        // Fill the message collection
        for (var outputCollectionIndex = 0; outputCollectionIndex < outputCollection.Length; outputCollectionIndex++)
        {
            var output = outputCollection[outputCollectionIndex];

            if (output.MessageCollection is null)
                continue;

            for (var messageIndex = 0; messageIndex < output.MessageCollection.Value.Length; messageIndex++)
                messageCollection[lastMessageIndex++] = output.MessageCollection.Value[messageIndex];
        }

        return messageCollection;
    }

    private static Message[]? JoinMessageCollection(Message messageToJoin, params Output[] outputCollection)
    {
        // Analyze the output collection to determine the size of the message collection
        var messageCollectionCount = 0;
        for (var outputCollectionIndex = 0; outputCollectionIndex < outputCollection.Length; outputCollectionIndex++)
        {
            var output = outputCollection[outputCollectionIndex];

            if (output.MessageCollection is null)
                continue;

            messageCollectionCount += output.MessageCollection.Value.Length;
        }
        messageCollectionCount += 1; // Add new space to join the message

        // Create the message collection
        if (messageCollectionCount == 0)
            return null;

        var messageCollection = new Message[messageCollectionCount];
        var lastMessageIndex = 0;

        // Fill the message collection
        for (var outputCollectionIndex = 0; outputCollectionIndex < outputCollection.Length; outputCollectionIndex++)
        {
            var output = outputCollection[outputCollectionIndex];

            if (output.MessageCollection is null)
                continue;

            for (var messageIndex = 0; messageIndex < output.MessageCollection.Value.Length; messageIndex++)
                messageCollection[lastMessageIndex++] = output.MessageCollection.Value[messageIndex];
        }
        
        messageCollection[lastMessageIndex++] = messageToJoin;


        return messageCollection;
    }
    private static Message[]? JoinMessageCollection(Message[] messageCollectionToJoin, params Output[] outputCollection)
    {
        // Analyze the output collection to determine the size of the message collection
        var messageCollectionCount = 0;
        for (var outputCollectionIndex = 0; outputCollectionIndex < outputCollection.Length; outputCollectionIndex++)
        {
            var output = outputCollection[outputCollectionIndex];

            if (output.MessageCollection is null)
                continue;

            messageCollectionCount += output.MessageCollection.Value.Length;
        }
        messageCollectionCount += messageCollectionToJoin.Length;

        // Create the message collection
        if (messageCollectionCount == 0)
            return null;

        var messageCollection = new Message[messageCollectionCount];
        var lastMessageIndex = 0;

        // Fill the message collection
        for (var outputCollectionIndex = 0; outputCollectionIndex < outputCollection.Length; outputCollectionIndex++)
        {
            var output = outputCollection[outputCollectionIndex];

            if (output.MessageCollection is null)
                continue;

            for (var messageIndex = 0; messageIndex < output.MessageCollection.Value.Length; messageIndex++)
                messageCollection[lastMessageIndex++] = output.MessageCollection.Value[messageIndex];
        }

        for (int i = 0; i < messageCollectionToJoin.Length; i++)
        {
            messageCollection[lastMessageIndex++] = messageCollectionToJoin[i];
        }

        return messageCollection;
    }
    
    private static Exception[]? JoinExceptionCollection(params Output[] outputCollection)
    {
        // Analyze the output collection to determine the size of the exception collection
        var exceptionCollectionCount = 0;
        for (var outputCollectionIndex = 0; outputCollectionIndex < outputCollection.Length; outputCollectionIndex++)
        {
            var output = outputCollection[outputCollectionIndex];

            if (output.ExceptionCollection is null)
                continue;

            exceptionCollectionCount += output.ExceptionCollection.Value.Length;
        }

        // Create the exception collection
        if (exceptionCollectionCount == 0)
            return null;

        var exceptionCollection = new Exception[exceptionCollectionCount];
        var lastExceptionIndex = 0;

        // Fill the exception collection
        for (var outputCollectionIndex = 0; outputCollectionIndex < outputCollection.Length; outputCollectionIndex++)
        {
            var output = outputCollection[outputCollectionIndex];

            if (output.ExceptionCollection is null)
                continue;

            for (var exceptionIndex = 0; exceptionIndex < output.ExceptionCollection.Value.Length; exceptionIndex++)
                exceptionCollection[lastExceptionIndex++] = output.ExceptionCollection.Value[exceptionIndex];
        }

        return exceptionCollection;
    }
}