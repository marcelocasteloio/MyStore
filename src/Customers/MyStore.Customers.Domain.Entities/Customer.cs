using MCIO.BuildingBlocks.Core.ExecutionInfo;
using MCIO.BuildingBlocks.Domain.Entities;
using MCIO.BuildingBlocks.Domain.Entities.ValueObjects;
using MCIO.BuildingBlocks.OutputEnvelop;

namespace MyStore.Customers.Domain.Entities;

public class Customer
    : EntityBase
{
    // Constants
    public const int NAME_MAX_LENGTH = 255;

    // Properties
    public string Name { get; private set; } = null!;
    public DateOnly BirthDate { get; private set; }
    
    // Constructors
    private Customer() { }
    private Customer(
        string name,
        DateOnly birthDate
    )
    {
        Name = name;
        BirthDate = birthDate;
    }
    
    // Builder
    public static Output<Customer?> RegisterNew(
        ExecutionInfo executionInfo,
        string name,
        DateOnly birthDate
    )
    {
        return RegisterNewInternal(
            executionInfo,
            entityFactory: () => new Customer(),
            additionalHandler: (_, customer) => Output.Create(
                customer.ChangeNameInternal(name),
                customer.ChangeBirthDateInternal(birthDate)
            )
        );
    }

    public static Customer FromExistingInfo(
        EntityInfoValueObject entityInfo,
        string name,
        DateOnly birthDate
    )
    {
        var customer = new Customer(name, birthDate);
        customer.FromExistingInfoInternal<Customer>(entityInfo);

        return customer;
    }
    
    // Protected Methods
    protected override Output ValidateInternal()
    {
        return Output.Create(
            ValidateName(Name),
            ValidateBirthDate(BirthDate)
        );
    }
    
    // Private Methods
    private Output ChangeNameInternal(string name)
    {
        // Validation
        var validateNameOutput = ValidateName(name);
        if (!validateNameOutput.IsSuccess)
            return validateNameOutput;
            
        // Process
        Name = name;
        
        // Return
        return Output.CreateSuccess();
    }
    private Output ChangeBirthDateInternal(DateOnly birthDate)
    {
        // Validation
        var validateBirthDateOutput = ValidateBirthDate(birthDate);
        if (!validateBirthDateOutput.IsSuccess)
            return validateBirthDateOutput;
        
        // Process
        BirthDate = birthDate;
        
        // Return
        return Output.CreateSuccess();
    }

    private static Output ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Output.CreateError(
                messageCode: CustomerMessages.NAME_IS_REQUIRED_MESSAGE_CODE,
                messageDescription: CustomerMessages.NAME_IS_REQUIRED_MESSAGE_DESCRIPTION
            );
        else if(name.Length > NAME_MAX_LENGTH)
            return Output.CreateError(
                messageCode: CustomerMessages.NAME_MAX_LENGTH_MESSAGE_CODE,
                messageDescription: CustomerMessages.NAME_MAX_LENGTH_MESSAGE_DESCRIPTION
            );

        return Output.CreateSuccess();
    }

    private static Output ValidateBirthDate(DateOnly birthDate)
    {
        if (birthDate > DateTimeValueObject.Now().ToDateOnly())
            return Output.CreateError(
                messageCode: CustomerMessages.BIRTH_DATE_SHOULD_LESS_THAN_CURRENT_DATE_MESSAGE_CODE,
                messageDescription: CustomerMessages.BIRTH_DATE_SHOULD_LESS_THAN_CURRENT_DATE_MESSAGE_DESCRIPTION
            );
        
        return Output.CreateSuccess();
    }
    
    // Messages
    public static class CustomerMessages
    {
        public const string NAME_IS_REQUIRED_MESSAGE_CODE = "Customer.Name.IsRequired";
        public const string NAME_IS_REQUIRED_MESSAGE_DESCRIPTION = "Name is required";
        
        public const string NAME_MAX_LENGTH_MESSAGE_CODE = "Customer.Name.MaxLength";
        public static readonly string NAME_MAX_LENGTH_MESSAGE_DESCRIPTION = $"Customer.Name should less than {NAME_MAX_LENGTH} characters";
        
        public const string BIRTH_DATE_SHOULD_LESS_THAN_CURRENT_DATE_MESSAGE_CODE = "Customer.BirthDate.Should.LessThan.CurrentDate";
        public const string BIRTH_DATE_SHOULD_LESS_THAN_CURRENT_DATE_MESSAGE_DESCRIPTION = "Birth date should be less than current date";
    }
}