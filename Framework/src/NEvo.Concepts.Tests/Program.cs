
using NEvo.Concepts.Tests.Saga.Statefull;

Console.WriteLine("Hello, World!");

//await Discriminator.TestAsync();
await MySagaHost.RunAsync(args);