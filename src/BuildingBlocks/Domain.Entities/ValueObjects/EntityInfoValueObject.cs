using MCIO.BuildingBlocks.Core.ExecutionInfo;
using MCIO.BuildingBlocks.OutputEnvelop;

namespace MCIO.BuildingBlocks.Domain.Entities.ValueObjects;

public readonly struct EntityInfoValueObject
    : IEquatable<EntityInfoValueObject>
{
    // Properties
    public IdValueObject Id { get; }
    public AuditableInfoValueObject AuditableInfo { get; }
    public RegistryVersionValueObject RegistryVersion { get; }
    
    // Constructors
    private EntityInfoValueObject(
        IdValueObject id,
        AuditableInfoValueObject auditableInfo,
        RegistryVersionValueObject registryVersion
    )
    {
        Id = id;
        AuditableInfo = auditableInfo;
        RegistryVersion = registryVersion;
    }
    
    // Builders
    public static Output<EntityInfoValueObject?> RegisterNew(ExecutionInfo executionInfo)
    {
        return Output<EntityInfoValueObject?>.CreateSuccess(
            value: new EntityInfoValueObject(
                id: IdValueObject.GenerateNew(),
                auditableInfo: AuditableInfoValueObject.GenerateNew(executionInfo),
                registryVersion: RegistryVersionValueObject.GenerateNew()
            )
        );
    }

    // Equality
    public bool Equals(EntityInfoValueObject other)
    {
        return Id.Equals(other.Id) 
            && AuditableInfo.Equals(other.AuditableInfo) 
            && RegistryVersion.Equals(other.RegistryVersion);
    }
    public override bool Equals(object? obj)
    {
        return obj is EntityInfoValueObject other && Equals(other);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, AuditableInfo, RegistryVersion);
    }
}