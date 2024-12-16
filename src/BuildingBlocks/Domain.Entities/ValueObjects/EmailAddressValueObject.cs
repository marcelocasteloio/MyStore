using System.Net.Mail;
using MCIO.BuildingBlocks.OutputEnvelop;
using System.Linq;

namespace MCIO.BuildingBlocks.Domain.Entities.ValueObjects;

public readonly struct EmailAddressValueObject 
    : IEquatable<EmailAddressValueObject>
{
    // Properties
    public string Value { get; }
    public bool IsValid { get; }

    // Constructors
    private EmailAddressValueObject(string value, bool isValid)
    {
        Value = value;
        IsValid = isValid;
    }

    // Implicit Operators
    public static implicit operator string(EmailAddressValueObject email) => email.Value;

    public static implicit operator EmailAddressValueObject(string email)
    {
        var createOutput = Create(email);
        
        if (!createOutput.IsSuccess)
        {
            var message = string.Empty;
            if (createOutput.HasMessage)
            {
                message = string.Join("|", createOutput.MessageImmutableArray!.Value.Select(q => q));
            }
            
            throw new ArgumentException(message, paramName: nameof(email));
        }

        return createOutput.Value!.Value;
    }

    public static bool operator ==(EmailAddressValueObject left, EmailAddressValueObject right) => left.Equals(right);
    public static bool operator !=(EmailAddressValueObject left, EmailAddressValueObject right) => !left.Equals(right);
    
    // Builders
    public static Output<EmailAddressValueObject?> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Output<EmailAddressValueObject?>.CreateError(
                value: null,
                messageCode: "Email cannot be null or whitespace.",
                messageDescription: ""
            );

        var validateEmailOutput = ValidateEmail(email);

        if (!validateEmailOutput.IsSuccess)
            return validateEmailOutput;

        return Output<EmailAddressValueObject?>.CreateSuccess(
            value: new EmailAddressValueObject(email, isValid: true)
        );
    }

    // Private Methods
    private static Output ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Output.CreateError(
                EmailAddressValueObjectMessages.EMAIL_ADDRESS_IS_REQUIRED_MESSAGE_CODE,
                EmailAddressValueObjectMessages.EMAIL_ADDRESS_IS_REQUIRED_MESSAGE_DESCRIPTION
            );

        try
        {
            var _ = new MailAddress(email);
        }
        catch (Exception exception)
        {
            return Output.CreateErrorFromException(
                exception,
                EmailAddressValueObjectMessages.EMAIL_ADDRESS_SHOULD_BE_VALID_MESSAGE_CODE,
                EmailAddressValueObjectMessages.EMAIL_ADDRESS_SHOULD_BE_VALID_MESSAGE_DESCRIPTION
            );
        }
        
        return Output.CreateSuccess();
    }

    // Equality
    public bool Equals(EmailAddressValueObject other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is EmailAddressValueObject other && Equals(other);
    public override int GetHashCode() => Value?.GetHashCode(StringComparison.OrdinalIgnoreCase) ?? 0;
    public override string ToString() => Value;
    
    // Messages
    public static class EmailAddressValueObjectMessages
    {
        public const string EMAIL_ADDRESS_IS_REQUIRED_MESSAGE_CODE = "EmailAddressValueObject.EmailAddress.IsRequired";
        public const string EMAIL_ADDRESS_IS_REQUIRED_MESSAGE_DESCRIPTION = "Email is required";
        
        public const string EMAIL_ADDRESS_SHOULD_BE_VALID_MESSAGE_CODE = "EmailAddressValueObject.EmailAddress.ShouldBeValid";
        public const string EMAIL_ADDRESS_SHOULD_BE_VALID_MESSAGE_DESCRIPTION = "Email should be valid";
    }
}