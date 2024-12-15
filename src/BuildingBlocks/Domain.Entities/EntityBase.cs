using MCIO.BuildingBlocks.Core.ExecutionInfo;
using MCIO.BuildingBlocks.Domain.Entities.ValueObjects;
using MCIO.BuildingBlocks.OutputEnvelop;
using MCIO.BuildingBlocks.OutputEnvelop.Models;

namespace MCIO.BuildingBlocks.Domain.Entities;

public abstract class EntityBase
{
    // Properties
    public EntityInfoValueObject EntityInfo { get; private set; }
    public bool IsValid { get; private set; }

    // Constructors
    protected EntityBase()
    {
        
    }

    // Public Methods
    public Output Validate()
    {
        var validateOutput = Output.Create(
            ValidateEntityInfo(EntityInfo),
            ValidateInternal()
        );

        if (!validateOutput.IsSuccess)
            return validateOutput;

        IsValid = true;

        return Output.CreateSuccess();
    }

    // Protected Methods
    protected EntityBase(EntityInfoValueObject entityInfo)
    {
        FromExistingInfoInternal<EntityBase>(entityInfo);
    }

    protected static Output<TEntityBase?> RegisterNewInternal<TEntityBase>(
        ExecutionInfo executionInfo,
        Func<TEntityBase> entityFactory,
        Func<ExecutionInfo, TEntityBase, Output> additionalHandler
    ) where TEntityBase : EntityBase
    {
        var entity = entityFactory();
        var entityInfoOutput = EntityInfoValueObject.RegisterNew(executionInfo);
        var additionalHandlerOutput = additionalHandler(executionInfo, entity);

        var entityInfo = entityInfoOutput.Value;
        
        var validateOutput = entityInfo is not null 
            ? Output.Create(
                entityInfoOutput,
                ValidateEntityInfo(entityInfo!.Value),
                additionalHandlerOutput
            )
            :
            Output.Create(
                entityInfoOutput,
                additionalHandlerOutput
            );

        if (!validateOutput.IsSuccess)
            return validateOutput;
        
        entity.EntityInfo = entityInfo!.Value;
        entity.Validate();
        
        return Output<TEntityBase?>.CreateSuccess(
            value: entity,
            validateOutput
        );
    }

    protected TEntityBase FromExistingInfoInternal<TEntityBase>(
        EntityInfoValueObject entityInfo
    ) where TEntityBase : EntityBase
    {
        EntityInfo = entityInfo;
        Validate();

        return (TEntityBase)this;
    }

    protected abstract Output ValidateInternal();

    // Private Methods
    private static Output ValidateEntityInfo(EntityInfoValueObject entityInfo)
    {
        List<Message>? messageCollection = null;

        if (entityInfo.Id.Value == Guid.Empty)
        {
            messageCollection ??= new(capacity: EntityBaseMessages.MESSAGE_COUNT);

            messageCollection.Add(
                Message.CreateError(
                    code: EntityBaseMessages.ENTITY_INFO_ID_IS_REQUIRED_MESSAGE_CODE,
                    description: EntityBaseMessages.ENTITY_INFO_ID_IS_REQUIRED_MESSAGE_DESCRIPTION
                )
            );
        }

        if (entityInfo.RegistryVersion.Value == 0)
        {
            messageCollection ??= new(capacity: EntityBaseMessages.MESSAGE_COUNT);

            messageCollection.Add(
                Message.CreateError(
                    code: EntityBaseMessages.ENTITY_INFO_REGISTRY_VERSION_IS_REQUIRED_MESSAGE_CODE,
                    description: EntityBaseMessages.ENTITY_INFO_REGISTRY_VERSION_IS_REQUIRED_MESSAGE_DESCRIPTION
                )
            );
        }

        if (string.IsNullOrWhiteSpace(entityInfo.AuditableInfo.CreatedBy))
        {
            messageCollection ??= new(capacity: EntityBaseMessages.MESSAGE_COUNT);

            messageCollection.Add(
                Message.CreateError(
                    code: EntityBaseMessages.ENTITY_INFO_AUDITABLE_INFO_CREATED_BY_IS_REQUIRED_MESSAGE_CODE,
                    description: EntityBaseMessages.ENTITY_INFO_AUDITABLE_INFO_CREATED_BY_IS_REQUIRED_MESSAGE_DESCRIPTION
                )
            );
        }

        if (entityInfo.AuditableInfo.CreatedAt.Value == default)
        {
            messageCollection ??= new(capacity: EntityBaseMessages.MESSAGE_COUNT);

            messageCollection.Add(
                Message.CreateError(
                    code: EntityBaseMessages.ENTITY_INFO_AUDITABLE_INFO_CREATED_AT_IS_REQUIRED_MESSAGE_CODE,
                    description: EntityBaseMessages.ENTITY_INFO_AUDITABLE_INFO_CREATED_AT_IS_REQUIRED_MESSAGE_DESCRIPTION
                )
            );
        }

        if (string.IsNullOrWhiteSpace(entityInfo.AuditableInfo.LastOrigin))
        {
            messageCollection ??= new(capacity: EntityBaseMessages.MESSAGE_COUNT);

            messageCollection.Add(
                Message.CreateError(
                    code: EntityBaseMessages.ENTITY_INFO_AUDITABLE_INFO_LAST_ORIGIN_IS_REQUIRED_MESSAGE_CODE,
                    description: EntityBaseMessages.ENTITY_INFO_AUDITABLE_INFO_LAST_ORIGIN_IS_REQUIRED_MESSAGE_DESCRIPTION
                )
            );
        }

        if (string.IsNullOrWhiteSpace(entityInfo.AuditableInfo.LastBusinessFlowCode))
        {
            messageCollection ??= new(capacity: EntityBaseMessages.MESSAGE_COUNT);

            messageCollection.Add(
                Message.CreateError(
                    code: EntityBaseMessages.ENTITY_INFO_AUDITABLE_INFO_LAST_BUSINESS_FLOW_CODE_IS_REQUIRED_MESSAGE_CODE,
                    description: EntityBaseMessages.ENTITY_INFO_AUDITABLE_INFO_LAST_BUSINESS_FLOW_CODE_IS_REQUIRED_MESSAGE_DESCRIPTION
                )
            );
        }

        if (entityInfo.AuditableInfo.LastCorrelationId == Guid.Empty)
        {
            messageCollection ??= new(capacity: EntityBaseMessages.MESSAGE_COUNT);

            messageCollection.Add(
                Message.CreateError(
                    code: EntityBaseMessages.ENTITY_INFO_AUDITABLE_INFO_LAST_CORRELATION_ID_IS_REQUIRED_MESSAGE_CODE,
                    description: EntityBaseMessages.ENTITY_INFO_AUDITABLE_INFO_LAST_CORRELATION_ID_IS_REQUIRED_MESSAGE_DESCRIPTION
                )
            );
        }

        var haveLastModifiedBy = !string.IsNullOrWhiteSpace(entityInfo.AuditableInfo.LastModifiedBy);
        var haveLastModifiedAt = entityInfo.AuditableInfo.LastModifiedAt != null && entityInfo.AuditableInfo.LastModifiedAt.Value != default;
        var haveLastModifiedInfo = haveLastModifiedBy || haveLastModifiedAt;
        var haveBothLastModifiedInfo = haveLastModifiedBy && haveLastModifiedAt;

        if (haveLastModifiedInfo && !haveBothLastModifiedInfo)
        {
            messageCollection ??= new(capacity: EntityBaseMessages.MESSAGE_COUNT);

            messageCollection.Add(
                Message.CreateError(
                    code: EntityBaseMessages.ENTITY_INFO_AUDITABLE_INFO_LAST_MODIFIED_INFO_IS_REQUIRED_WITH_ALL_VALUES_MESSAGE_CODE,
                    description: EntityBaseMessages.ENTITY_INFO_AUDITABLE_INFO_LAST_MODIFIED_INFO_IS_REQUIRED_WITH_ALL_VALUES_MESSAGE_DESCRIPTION
                )
            );
        }

        if (messageCollection?.Count > 0)
            return Output.CreateError(
                messageCollection.ToArray()
            );

        return Output.CreateSuccess();
    }

    // Messages
    public static class EntityBaseMessages
    {
        public const int MESSAGE_COUNT = 8;

        public const string ENTITY_INFO_ID_IS_REQUIRED_MESSAGE_CODE = "EntityBase.EntityInfo.Id.IsRequired";
        public const string ENTITY_INFO_ID_IS_REQUIRED_MESSAGE_DESCRIPTION = "EntityInfo.Id should be valid";

        public const string ENTITY_INFO_REGISTRY_VERSION_IS_REQUIRED_MESSAGE_CODE = "EntityBase.EntityInfo.RegistryVerion.IsRequired";
        public const string ENTITY_INFO_REGISTRY_VERSION_IS_REQUIRED_MESSAGE_DESCRIPTION = "EntityInfo.RegistryVerion should be valid";

        public const string ENTITY_INFO_AUDITABLE_INFO_CREATED_BY_IS_REQUIRED_MESSAGE_CODE = "EntityBase.EntityInfo.AuditableInfo.CreatedBy.IsRequired";
        public const string ENTITY_INFO_AUDITABLE_INFO_CREATED_BY_IS_REQUIRED_MESSAGE_DESCRIPTION = "EntityInfo.AuditableInfo.CreatedBy should be valid";

        public const string ENTITY_INFO_AUDITABLE_INFO_CREATED_AT_IS_REQUIRED_MESSAGE_CODE = "EntityBase.EntityInfo.AuditableInfo.CreatedAt.IsRequired";
        public const string ENTITY_INFO_AUDITABLE_INFO_CREATED_AT_IS_REQUIRED_MESSAGE_DESCRIPTION = "EntityInfo.AuditableInfo.CreatedAt should be valid";

        public const string ENTITY_INFO_AUDITABLE_INFO_LAST_ORIGIN_IS_REQUIRED_MESSAGE_CODE = "EntityBase.EntityInfo.AuditableInfo.LastOrigin.IsRequired";
        public const string ENTITY_INFO_AUDITABLE_INFO_LAST_ORIGIN_IS_REQUIRED_MESSAGE_DESCRIPTION = "EntityInfo.AuditableInfo.LastOrigin should be valid";

        public const string ENTITY_INFO_AUDITABLE_INFO_LAST_BUSINESS_FLOW_CODE_IS_REQUIRED_MESSAGE_CODE = "EntityBase.EntityInfo.AuditableInfo.LastBusinessFlowCode.IsRequired";
        public const string ENTITY_INFO_AUDITABLE_INFO_LAST_BUSINESS_FLOW_CODE_IS_REQUIRED_MESSAGE_DESCRIPTION = "EntityInfo.AuditableInfo.LastBusinessFlowCode should be valid";

        public const string ENTITY_INFO_AUDITABLE_INFO_LAST_CORRELATION_ID_IS_REQUIRED_MESSAGE_CODE = "EntityBase.EntityInfo.AuditableInfo.LastCorrelationId.IsRequired";
        public const string ENTITY_INFO_AUDITABLE_INFO_LAST_CORRELATION_ID_IS_REQUIRED_MESSAGE_DESCRIPTION = "EntityInfo.AuditableInfo.LastCorrelationId should be valid";

        public const string ENTITY_INFO_AUDITABLE_INFO_LAST_MODIFIED_INFO_IS_REQUIRED_WITH_ALL_VALUES_MESSAGE_CODE = "EntityBase.EntityInfo.AuditableInfo.LastModifiedInfo.IsRequired.WithAllValues";
        public const string ENTITY_INFO_AUDITABLE_INFO_LAST_MODIFIED_INFO_IS_REQUIRED_WITH_ALL_VALUES_MESSAGE_DESCRIPTION = "EntityInfo.AuditableInfo.LastModifiedInfo is required with all values";
    }
}