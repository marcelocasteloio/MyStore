using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using Benchmarks.Base.Interfaces;

namespace Benchmarks.Benchs.ListWithCapacityBench;

[SimpleJob(RunStrategy.Throughput, launchCount: 1)]
[HardwareCounters( HardwareCounter.BranchMispredictions, HardwareCounter.BranchInstructions)]
[MemoryDiagnoser]
public class ListWithCapacityBenchmark
    : IBenchmark
{
    [Params(1, 5, 15)]
    public int ItemCount { get; set; }
    
    [Benchmark(Baseline = true)]
    public List<int> UsingListWithCapacity()
    {
        var list = new List<int>(capacity: ItemCount);
        
        for (var i = 0; i < ItemCount; i++)
            list.Add(i);

        return list;
    }
    
    [Benchmark]
    public List<int> UsingListWithoutCapacity()
    {
        var list = new List<int>();
        
        for (var i = 0; i < ItemCount; i++)
            list.Add(i);

        return list;
    }
}