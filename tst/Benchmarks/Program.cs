using Benchmarks.Base;
namespace Benchmarks;
public static class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunnerWrapper.Execute(typeof(Program).Assembly);
    }
}