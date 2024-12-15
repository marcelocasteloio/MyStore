using MCIO.BuildingBlocks.Core.ExecutionInfo;
using MyStore.Customers.Domain.Entities;

var createExecutionInfoOutput = ExecutionInfo.Create(
    correlationId: Guid.NewGuid(),
    executionUser: Environment.UserName,
    businessFlowCode: "Sample registration",
    origin: "SampleConsole",
    language: "en-US"
);

if (!createExecutionInfoOutput.IsSuccess)
    return;

var executionInfo = createExecutionInfoOutput.Value!.Value;

var registerNewCustomerOutput = Customer.RegisterNew(
    executionInfo,
    name: "John Doe",
    birthDate: new DateOnly(1980, 1, 1)
);

if (!registerNewCustomerOutput.IsSuccess)
    return;

var customer = registerNewCustomerOutput.Value!;

Console.WriteLine($"Customer {customer.Name} registered with ID {customer.EntityInfo.Id}");

Console.WriteLine("Press [ENTER] to exit...");
Console.ReadLine();