using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using Benchmarks.Base.Interfaces;

namespace Benchmarks.Benchs.ClosureBench;

[SimpleJob(RunStrategy.Throughput, launchCount: 1)]
[HardwareCounters( HardwareCounter.BranchMispredictions, HardwareCounter.BranchInstructions)]
[MemoryDiagnoser]
public class ClosureBenchmark
    : IBenchmark
{
    [Params(1, 10, 50)]
    public int LoopCount { get; set; }
    
    [Benchmark(Baseline = true)]
    public int WithoutClosure()
    {
        var lastNumber = 0;

        for (int i = 0; i < LoopCount; i++)
        {
            var number = 10;
        
            var twiceFunction = new Func<int, int>((n) =>
            {
                return n * 2;
            });

            lastNumber = twiceFunction(number);
        }

        return lastNumber;
    }
    
    [Benchmark()]
    public int WithClosure()
    {
        var lastNumber = 0;

        for (int i = 0; i < LoopCount; i++)
        {
            var number = 10;
        
            var twiceFunction = new Func< int>(() =>
            {
                return number * 2;
            });

            lastNumber = twiceFunction();
        }

        return lastNumber;
    }
}