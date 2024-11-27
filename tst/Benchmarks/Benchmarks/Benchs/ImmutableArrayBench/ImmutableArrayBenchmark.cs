using System.Collections.Immutable;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using Benchmarks.Base.Interfaces;

namespace Benchmarks.Benchs.ImmutableArrayBench;

[SimpleJob(RunStrategy.Throughput, launchCount: 1)]
[HardwareCounters( HardwareCounter.BranchMispredictions, HardwareCounter.BranchInstructions)]
[MemoryDiagnoser]
public class ImmutableArrayBenchmark
    : IBenchmark
{
    [Params(1, 5, 10)]
    public int ArrayLength { get; set; }
    
    [Benchmark(Baseline = true)]
    public int[] CreateArray()
    {
        var array = new int[ArrayLength];
        
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = i;
        }

        return array;
    }
    
    [Benchmark]
    public ImmutableArray<int> CreateImmutableArray()
    {
        var array = new int[ArrayLength];
        
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = i;
        }

        return ImmutableArray.Create(array);
    }
    
    [Benchmark]
    public ImmutableArray<int> CreateImmutableArrayUsingCollectionExpression()
    {
        var array = new int[ArrayLength];
        
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = i;
        }

        return [..array];
    }
    
    [Benchmark]
    public ImmutableArray<int> CreateImmutableArrayUsingToImmutableArray()
    {
        var array = new int[ArrayLength];
        
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = i;
        }

        return array.ToImmutableArray();
    }
}