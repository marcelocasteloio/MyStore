using MCIO.BuildingBlocks.Core.ExecutionInfo;

namespace MCIO.BuildingBlocks.Domain.Entities.ValueObjects;

public readonly struct AuditableInfoValueObject
    : IEquatable<AuditableInfoValueObject>
{
    // Properties
    public string CreatedBy { get; }
    public DateTimeValueObject CreatedAt { get; }
    
    public string? LastModifiedBy { get; }
    public DateTimeValueObject? LastModifiedAt { get; }
    public Guid LastCorrelationId { get; }
    public string LastBusinessFlowCode { get; }
    public string LastOrigin { get; }
    
    // Constructors
    private AuditableInfoValueObject(
        string createdBy,
        DateTimeValueObject createdAt,
        string? lastModifiedBy,
        DateTimeValueObject? lastModifiedAt,
        Guid lastCorrelationId,
        string lastBusinessFlowCode,
        string lastOrigin
    )
    {
        CreatedBy = createdBy;
        CreatedAt = createdAt;
        LastModifiedBy = lastModifiedBy;
        LastModifiedAt = lastModifiedAt;
        LastCorrelationId = lastCorrelationId;
        LastBusinessFlowCode = lastBusinessFlowCode;
        LastOrigin = lastOrigin;
    }
    
    // Builders
    public static AuditableInfoValueObject GenerateNew(
        ExecutionInfo executionInfo
    )
    {
        return new AuditableInfoValueObject(
            createdBy: executionInfo.ExecutionUser,
            createdAt: DateTimeValueObject.Now(),
            lastModifiedBy: null,
            lastModifiedAt: null,
            lastCorrelationId: executionInfo.CorrelationId,
            lastBusinessFlowCode: executionInfo.BusinessFlowCode,
            lastOrigin: executionInfo.Origin
        );
    }
    public static AuditableInfoValueObject FromExistingInfo(
        string createdBy,
        DateTimeValueObject createdAt,
        string? lastModifiedBy,
        DateTimeValueObject? lastModifiedAt,
        Guid lastCorrelationId,
        string lastBusinessFlowCode,
        string lastOrigin
    )
    {
        return new AuditableInfoValueObject(
            createdBy,
            createdAt,
            lastModifiedBy,
            lastModifiedAt,
            lastCorrelationId,
            lastBusinessFlowCode,
            lastOrigin
        );
    }
    
    // Public Methods
    public AuditableInfoValueObject RegisterModification(
        ExecutionInfo executionInfo
    )
    {
        return new AuditableInfoValueObject(
            createdBy: CreatedBy,
            createdAt: CreatedAt,
            lastModifiedBy: executionInfo.ExecutionUser,
            lastModifiedAt: DateTimeValueObject.Now(),
            lastCorrelationId: executionInfo.CorrelationId,
            lastBusinessFlowCode: executionInfo.BusinessFlowCode,
            lastOrigin: executionInfo.Origin
        );
    }

    // Equality
    public bool Equals(AuditableInfoValueObject other)
    {
        return CreatedBy == other.CreatedBy 
           && CreatedAt.Equals(other.CreatedAt) 
           && LastModifiedBy == other.LastModifiedBy 
           && Nullable.Equals(LastModifiedAt, other.LastModifiedAt) 
           && LastCorrelationId.Equals(other.LastCorrelationId) 
           && LastBusinessFlowCode == other.LastBusinessFlowCode 
           && LastOrigin == other.LastOrigin;
    }
    public override bool Equals(object? obj)
    {
        return obj is AuditableInfoValueObject other && Equals(other);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(
            CreatedBy, 
            CreatedAt, 
            LastModifiedBy, 
            LastModifiedAt, 
            LastCorrelationId, 
            LastBusinessFlowCode, 
            LastOrigin
        );
    }
}