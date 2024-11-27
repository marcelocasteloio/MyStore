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