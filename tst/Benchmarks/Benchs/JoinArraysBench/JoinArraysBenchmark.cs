using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using Benchmarks.Base.Interfaces;

namespace Benchmarks.Benchs.JoinArraysBench;

[SimpleJob(RunStrategy.Throughput, launchCount: 1)]
[HardwareCounters( HardwareCounter.BranchMispredictions, HardwareCounter.BranchInstructions)]
[MemoryDiagnoser]
public class JoinArraysBenchmark
    : IBenchmark
{
    private int[] _arrayA = null!;
    private int[] _arrayB = null!;
    private int[] _arrayC = null!;
    private int[][] _arrayCollection = null!;
    
    [Params(1, 5, 10, 50)]
    public int ArrayLength { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _arrayA = new int[ArrayLength];
        _arrayB = new int[ArrayLength];
        _arrayC = new int[ArrayLength];
        
        _arrayCollection =
        [
            _arrayA,
            _arrayB,
            _arrayC
        ];
    }
    
    [Benchmark(Baseline = true)]
    public int[] UsingLinqFromArrayCollectrion()
    {
        return _arrayCollection.SelectMany(q => q).ToArray();
    }
    
    [Benchmark]
    public int[] UsingLinqFromIndividualArray()
    {
        return _arrayA.Concat(_arrayB).Concat(_arrayC).ToArray();
    }
    
    [Benchmark]
    public int[] UsingArrayCopy()
    {
        var newArrayLength = 0;
        
        for (var i = 0; i < _arrayCollection.Length; i++)
            newArrayLength += _arrayCollection[i].Length;
        
        var newArray = new int[newArrayLength];
        var currentIndex = 0;

        for (var i = 0; i < _arrayCollection.Length; i++)
        {
            var currentArray = _arrayCollection[i];
            
            Array.Copy(
                sourceArray: currentArray,
                sourceIndex: 0,
                destinationArray: newArray,
                destinationIndex: currentIndex,
                length: currentArray.Length
            );
            
            currentIndex += currentArray.Length;
        }

        return newArray;
    }
    
    [Benchmark]
    public int[] UsingManualCopy()
    {
        var newArrayLength = 0;
        
        for (var i = 0; i < _arrayCollection.Length; i++)
            newArrayLength += _arrayCollection[i].Length;
        
        var newArray = new int[newArrayLength];
        var lastIndex = 0;

        for (var i = 0; i < _arrayCollection.Length; i++)
        {
            var currentArray = _arrayCollection[i];

            for (var j = 0; j < currentArray.Length; j++)
            {
                newArray[lastIndex++] = currentArray[j];
            }
        }

        return newArray;
    }
}

/*
| Method                        | ArrayLength | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0   | BranchInstructions/Op | BranchMispredictions/Op | Allocated | Alloc Ratio |
|------------------------------ |------------ |----------:|---------:|---------:|------:|--------:|-------:|----------------------:|------------------------:|----------:|------------:|
| UsingLinqFromArrayCollectrion | 1           |  72.76 ns | 0.641 ns | 0.501 ns |  1.00 |    0.01 | 0.0081 |                   226 |                       0 |     136 B |        1.00 |
| UsingLinqFromIndividualArray  | 1           |  97.02 ns | 1.776 ns | 1.575 ns |  1.33 |    0.02 | 0.0095 |                   330 |                       0 |     160 B |        1.18 |
| UsingArrayCopy                | 1           |  19.57 ns | 0.378 ns | 0.529 ns |  0.27 |    0.01 | 0.0024 |                    60 |                       0 |      40 B |        0.29 |
| UsingManualCopy               | 1           |  10.88 ns | 0.248 ns | 0.400 ns |  0.15 |    0.01 | 0.0024 |                    31 |                       0 |      40 B |        0.29 |
|                               |             |           |          |          |       |         |        |                       |                         |           |             |
| UsingLinqFromArrayCollectrion | 5           | 114.54 ns | 2.222 ns | 2.078 ns |  1.00 |    0.02 | 0.0110 |                   333 |                       0 |     184 B |        1.00 |
| UsingLinqFromIndividualArray  | 5           |  97.50 ns | 1.032 ns | 0.806 ns |  0.85 |    0.02 | 0.0124 |                   334 |                       0 |     208 B |        1.13 |
| UsingArrayCopy                | 5           |  22.16 ns | 0.257 ns | 0.201 ns |  0.19 |    0.00 | 0.0052 |                    62 |                       0 |      88 B |        0.48 |
| UsingManualCopy               | 5           |  23.56 ns | 0.189 ns | 0.167 ns |  0.21 |    0.00 | 0.0052 |                    68 |                       0 |      88 B |        0.48 |
|                               |             |           |          |          |       |         |        |                       |                         |           |             |
| UsingLinqFromArrayCollectrion | 10          | 153.80 ns | 1.209 ns | 0.944 ns |  1.00 |    0.01 | 0.0143 |                   437 |                       0 |     240 B |        1.00 |
| UsingLinqFromIndividualArray  | 10          | 102.04 ns | 0.711 ns | 0.631 ns |  0.66 |    0.01 | 0.0157 |                   341 |                       0 |     264 B |        1.10 |
| UsingArrayCopy                | 10          |  24.77 ns | 0.521 ns | 0.435 ns |  0.16 |    0.00 | 0.0086 |                    67 |                       0 |     144 B |        0.60 |
| UsingManualCopy               | 10          |  40.25 ns | 0.857 ns | 1.456 ns |  0.26 |    0.01 | 0.0086 |                   114 |                       0 |     144 B |        0.60 |
|                               |             |           |          |          |       |         |        |                       |                         |           |             |
| UsingLinqFromArrayCollectrion | 50          | 186.00 ns | 1.958 ns | 1.635 ns |  1.00 |    0.01 | 0.0429 |                   485 |                       1 |     720 B |        1.00 |
| UsingLinqFromIndividualArray  | 50          | 127.84 ns | 2.612 ns | 4.575 ns |  0.69 |    0.02 | 0.0443 |                   377 |                       0 |     744 B |        1.03 |
| UsingArrayCopy                | 50          |  50.37 ns | 1.053 ns | 2.151 ns |  0.27 |    0.01 | 0.0373 |                   101 |                       0 |     624 B |        0.87 |
| UsingManualCopy               | 50          | 173.31 ns | 3.436 ns | 4.817 ns |  0.93 |    0.03 | 0.0372 |                   483 |                       1 |     624 B |        0.87 |
 */
 
 