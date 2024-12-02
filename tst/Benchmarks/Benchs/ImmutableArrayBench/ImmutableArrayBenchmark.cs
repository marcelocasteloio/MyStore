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
/*
| Method                                        | ArrayLength | Mean      | Error     | StdDev    | Median    | Ratio | RatioSD | BranchInstructions/Op | BranchMispredictions/Op | Gen0   | Allocated | Alloc Ratio |
|---------------------------------------------- |------------ |----------:|----------:|----------:|----------:|------:|--------:|----------------------:|------------------------:|-------:|----------:|------------:|
| CreateArray                                   | 1           |  5.138 ns | 0.1228 ns | 0.1206 ns |  5.091 ns |  1.00 |    0.03 |                    11 |                       0 | 0.0019 |      32 B |        1.00 |
| CreateImmutableArray                          | 1           | 14.001 ns | 0.1657 ns | 0.1550 ns | 13.970 ns |  2.73 |    0.07 |                    32 |                       0 | 0.0038 |      64 B |        2.00 |
| CreateImmutableArrayUsingCollectionExpression | 1           |  8.746 ns | 0.0961 ns | 0.0802 ns |  8.709 ns |  1.70 |    0.04 |                    20 |                       0 | 0.0038 |      64 B |        2.00 |
| CreateImmutableArrayUsingToImmutableArray     | 1           | 20.963 ns | 0.1839 ns | 0.1435 ns | 20.990 ns |  4.08 |    0.09 |                    65 |                       0 | 0.0038 |      64 B |        2.00 |
|                                               |             |           |           |           |           |       |         |                       |                         |        |           |             |
| CreateArray                                   | 5           |  6.263 ns | 0.1351 ns | 0.1198 ns |  6.270 ns |  1.00 |    0.03 |                    14 |                       0 | 0.0029 |      48 B |        1.00 |
| CreateImmutableArray                          | 5           | 16.229 ns | 0.3353 ns | 0.2972 ns | 16.222 ns |  2.59 |    0.07 |                    36 |                       0 | 0.0057 |      96 B |        2.00 |
| CreateImmutableArrayUsingCollectionExpression | 5           | 12.170 ns | 0.2703 ns | 0.7710 ns | 12.019 ns |  1.94 |    0.13 |                    25 |                       0 | 0.0057 |      96 B |        2.00 |
| CreateImmutableArrayUsingToImmutableArray     | 5           | 23.588 ns | 0.4840 ns | 0.4041 ns | 23.541 ns |  3.77 |    0.09 |                    70 |                       0 | 0.0057 |      96 B |        2.00 |
|                                               |             |           |           |           |           |       |         |                       |                         |        |           |             |
| CreateArray                                   | 10          |  8.861 ns | 0.2212 ns | 0.5301 ns |  8.649 ns |  1.00 |    0.08 |                    20 |                       0 | 0.0038 |      64 B |        1.00 |
| CreateImmutableArray                          | 10          | 18.085 ns | 0.2862 ns | 0.2677 ns | 18.143 ns |  2.05 |    0.12 |                    43 |                       0 | 0.0076 |     128 B |        2.00 |
| CreateImmutableArrayUsingCollectionExpression | 10          | 14.158 ns | 0.2333 ns | 0.1948 ns | 14.122 ns |  1.60 |    0.09 |                    32 |                       0 | 0.0076 |     128 B |        2.00 |
| CreateImmutableArrayUsingToImmutableArray     | 10          | 26.200 ns | 0.5297 ns | 0.5203 ns | 26.111 ns |  2.97 |    0.18 |                    77 |                       0 | 0.0076 |     128 B |        2.00 |
 */