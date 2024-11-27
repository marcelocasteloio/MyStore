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
| Method                                        | ArrayLength | Mean      | Error     | StdDev    | Ratio | RatioSD | BranchInstructions/Op | BranchMispredictions/Op | Gen0   | Allocated | Alloc Ratio |
|---------------------------------------------- |------------ |----------:|----------:|----------:|------:|--------:|----------------------:|------------------------:|-------:|----------:|------------:|
| CreateArray                                   | 1           |  5.039 ns | 0.1502 ns | 0.1542 ns |  1.00 |    0.04 |                    11 |                       0 | 0.0019 |      32 B |        1.00 |
| CreateImmutableArray                          | 1           | 14.741 ns | 0.1457 ns | 0.1363 ns |  2.93 |    0.09 |                    31 |                       0 | 0.0038 |      64 B |        2.00 |
| CreateImmutableArrayUsingCollectionExpression | 1           |  9.042 ns | 0.2045 ns | 0.3739 ns |  1.80 |    0.09 |                    21 |                       0 | 0.0038 |      64 B |        2.00 |
| CreateImmutableArrayUsingToImmutableArray     | 1           | 20.781 ns | 0.2248 ns | 0.1877 ns |  4.13 |    0.13 |                    65 |                       0 | 0.0038 |      64 B |        2.00 |
|                                               |             |           |           |           |       |         |                       |                         |        |           |             |
| CreateArray                                   | 5           |  6.689 ns | 0.1550 ns | 0.1374 ns |  1.00 |    0.03 |                    14 |                       0 | 0.0029 |      48 B |        1.00 |
| CreateImmutableArray                          | 5           | 16.002 ns | 0.1824 ns | 0.1617 ns |  2.39 |    0.05 |                    36 |                       0 | 0.0057 |      96 B |        2.00 |
| CreateImmutableArrayUsingCollectionExpression | 5           | 11.854 ns | 0.2207 ns | 0.1957 ns |  1.77 |    0.04 |                    25 |                       0 | 0.0057 |      96 B |        2.00 |
| CreateImmutableArrayUsingToImmutableArray     | 5           | 24.270 ns | 0.2485 ns | 0.2203 ns |  3.63 |    0.08 |                    70 |                       0 | 0.0057 |      96 B |        2.00 |
|                                               |             |           |           |           |       |         |                       |                         |        |           |             |
| CreateArray                                   | 10          |  8.964 ns | 0.1871 ns | 0.1659 ns |  1.00 |    0.03 |                    20 |                       0 | 0.0038 |      64 B |        1.00 |
| CreateImmutableArray                          | 10          | 18.789 ns | 0.3864 ns | 0.3017 ns |  2.10 |    0.05 |                    43 |                       0 | 0.0076 |     128 B |        2.00 |
| CreateImmutableArrayUsingCollectionExpression | 10          | 14.656 ns | 0.3175 ns | 0.8474 ns |  1.64 |    0.10 |                    32 |                       0 | 0.0076 |     128 B |        2.00 |
| CreateImmutableArrayUsingToImmutableArray     | 10          | 26.190 ns | 0.2916 ns | 0.2585 ns |  2.92 |    0.06 |                    76 |                       0 | 0.0076 |     128 B |        2.00 |
 */