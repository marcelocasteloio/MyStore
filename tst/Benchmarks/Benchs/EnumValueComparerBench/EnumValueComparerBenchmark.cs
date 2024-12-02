using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using Benchmarks.Benchs.EnumValueComparerBench.Enums;
using Benchmarks.Benchs.EnumValueComparerBench.Interfaces;

namespace Benchmarks.Benchs.EnumValueComparerBench;

[SimpleJob(RunStrategy.Throughput, launchCount: 1)]
[HardwareCounters( HardwareCounter.BranchMispredictions, HardwareCounter.BranchInstructions)]
[MemoryDiagnoser]
public class EnumValueComparerBenchmark
    : IEnumValueComparerBenchmark
{
    private CustomerType[] _allCustomerTypes = null!;
    [GlobalSetup]
    public void GlobalSetup()
    {
        _allCustomerTypes = Enum.GetValues<CustomerType>();
    }
    
    [Benchmark(Baseline = true)]
    public bool WithEnumIsDefinied()
    {
        var lastValidation = false;
        
        for (var i = 0; i < _allCustomerTypes.Length; i++)
        {
            var customerType = _allCustomerTypes[i];
            lastValidation = Enum.IsDefined(customerType);
        }
        return lastValidation;
    }
    
    [Benchmark]
    public bool WithValueComparer()
    {
        var lastValidation = false;
        
        for (var i = 0; i < _allCustomerTypes.Length; i++)
        {
            var customerType = (byte)_allCustomerTypes[i];
            lastValidation = customerType is > 0 and < 5;
        }
        
        return lastValidation;
    }
}
/*
| Method             | Mean      | Error     | StdDev    | Ratio | BranchInstructions/Op | BranchMispredictions/Op | Allocated | Alloc Ratio |
|------------------- |----------:|----------:|----------:|------:|----------------------:|------------------------:|----------:|------------:|
| WithEnumIsDefinied | 17.830 ns | 0.0732 ns | 0.0649 ns |  1.00 |                    63 |                       0 |         - |          NA |
| WithValueComparer  |  3.218 ns | 0.0399 ns | 0.0373 ns |  0.18 |                    13 |                       0 |         - |          NA |
 */
 