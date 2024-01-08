// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using NEvo.Benchmarks.Monads;

BenchmarkRunner.Run<ExceptionHandlingBenchmark>();