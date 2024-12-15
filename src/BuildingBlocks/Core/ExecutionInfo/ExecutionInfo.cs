using MCIO.BuildingBlocks.OutputEnvelop;
using MCIO.BuildingBlocks.OutputEnvelop.Models;

namespace MCIO.BuildingBlocks.Core.ExecutionInfo;

public readonly struct ExecutionInfo
{
    // Properties
    public Guid CorrelationId { get; }
    public string ExecutionUser { get; }
    public string BusinessFlowCode { get; }
    public string Origin { get; }
    public string Language { get; }
    public bool IsValid { get; }

    // Constructors
    private ExecutionInfo(
        Guid correlationId,
        string executionUser,
        string businessFlowCode,
        string origin,
        string language,
        bool isValid
    )
    {
        CorrelationId = correlationId;
        ExecutionUser = executionUser;
        BusinessFlowCode = businessFlowCode;
        Origin = origin;
        Language = language;
        IsValid = isValid;
    }

    // Builders
    public static Output<ExecutionInfo?> Create(
        Guid correlationId,
        string executionUser,
        string businessFlowCode,
        string origin,
        string language
    )
    {
        var validateOutput = ValidateInternal(
            correlationId,
            executionUser,
            businessFlowCode,
            origin,
            language
        );
        
        if(!validateOutput.IsSuccess)
            return validateOutput;
        
        return Output<ExecutionInfo?>.CreateSuccess(
            value: new ExecutionInfo(
                correlationId,
                executionUser,
                businessFlowCode,
                origin,
                language,
                isValid: true
            )
        );
    }

    public static ExecutionInfo CreateForced(
        Guid correlationId,
        string executionUser,
        string businessFlowCode,
        string origin,
        string language
    )
    {
        var validateOutput = ValidateInternal(
            correlationId,
            executionUser,
            businessFlowCode,
            origin,
            language
        );
        
        return new ExecutionInfo(
            correlationId,
            executionUser,
            businessFlowCode,
            origin,
            language,
            isValid: validateOutput.IsSuccess
        );
    }
    
    // Private Methods
    private static Output ValidateInternal(
        Guid correlationId,
        string executionUser,
        string businessFlowCode,
        string origin,
        string language    
    )
    {
        List<Message>? messageColection = null;

        if (correlationId == Guid.Empty)
        {
            messageColection ??= new(capacity: ExecutionInfoMessages.MAX_MESSAGE_COUNT);
            messageColection.Add(
                Message.CreateError(
                    code: ExecutionInfoMessages.CORRELATION_ID_IS_REQUIRED_MESSAGE_CODE,
                    description: ExecutionInfoMessages.CORRELATION_ID_IS_REQUIRED_MESSAGE_DESCRIPTION
                )
            );
        }

        if (string.IsNullOrWhiteSpace(executionUser))
        {
            messageColection ??= new(capacity: ExecutionInfoMessages.MAX_MESSAGE_COUNT);
            messageColection.Add(
                Message.CreateError(
                    code: ExecutionInfoMessages.EXECUTION_USER_IS_REQUIRED_MESSAGE_CODE,
                    description: ExecutionInfoMessages.EXECUTION_USER_IS_REQUIRED_MESSAGE_DESCRIPTION
                )
            );
        }

        if (string.IsNullOrWhiteSpace(businessFlowCode))
        {
            messageColection ??= new(capacity: ExecutionInfoMessages.MAX_MESSAGE_COUNT);
            messageColection.Add(
                Message.CreateError(
                    code: ExecutionInfoMessages.BUSINESS_FLOW_CODE_IS_REQUIRED_MESSAGE_CODE,
                    description: ExecutionInfoMessages.BUSINESS_FLOW_CODE_IS_REQUIRED_MESSAGE_DESCRIPTION
                )
            );
        }

        if (string.IsNullOrWhiteSpace(origin))
        {
            messageColection ??= new(capacity: ExecutionInfoMessages.MAX_MESSAGE_COUNT);
            messageColection.Add(
                Message.CreateError(
                    code: ExecutionInfoMessages.ORIGIN_IS_REQUIRED_MESSAGE_CODE,
                    description: ExecutionInfoMessages.ORIGIN_IS_REQUIRED_MESSAGE_DESCRIPTION
                )
            );
        }

        if (string.IsNullOrWhiteSpace(language))
        {
            messageColection ??= new(capacity: ExecutionInfoMessages.MAX_MESSAGE_COUNT);
            messageColection.Add(
                Message.CreateError(
                    code: ExecutionInfoMessages.LANGUAGE_IS_REQUIRED_MESSAGE_CODE,
                    description: ExecutionInfoMessages.LANGUAGE_IS_REQUIRED_MESSAGE_DESCRIPTION
                )
            );
        }

        if (messageColection?.Count > 0)
            return Output<ExecutionInfo?>.CreateError(
                value: null,
                messageCollection: messageColection.ToArray(),
                exceptionCollection: null
            );
        
        return Output.CreateSuccess();
    }

    // Messages
    public static class ExecutionInfoMessages
    {
        public const short MAX_MESSAGE_COUNT = 10;

        public const string CORRELATION_ID_IS_REQUIRED_MESSAGE_CODE = "ExecutionInfo.CorrelationId.IsRequired";
        public const string CORRELATION_ID_IS_REQUIRED_MESSAGE_DESCRIPTION = "CorrelationId is required.";

        public const string EXECUTION_USER_IS_REQUIRED_MESSAGE_CODE = "ExecutionInfo.ExecutionUser.IsRequired";
        public const string EXECUTION_USER_IS_REQUIRED_MESSAGE_DESCRIPTION = "ExecutionUser is required.";

        public const string BUSINESS_FLOW_CODE_IS_REQUIRED_MESSAGE_CODE = "ExecutionInfo.BusinessFlowCode.IsRequired";
        public const string BUSINESS_FLOW_CODE_IS_REQUIRED_MESSAGE_DESCRIPTION = "BusinessFlowCode is required.";

        public const string ORIGIN_IS_REQUIRED_MESSAGE_CODE = "ExecutionInfo.Origin.IsRequired";
        public const string ORIGIN_IS_REQUIRED_MESSAGE_DESCRIPTION = "Origin is required.";

        public const string LANGUAGE_IS_REQUIRED_MESSAGE_CODE = "ExecutionInfo.Language.IsRequired";
        public const string LANGUAGE_IS_REQUIRED_MESSAGE_DESCRIPTION = "Language is required.";
    }
}