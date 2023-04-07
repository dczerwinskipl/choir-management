using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace NEvo.Concepts.Tests.DI;

public static class Discriminator
{
    public static async Task TestAsync()
    {
        var services = new ServiceCollection();
        services
            .AddScoped<Legacy>()
            .AddScoped<LegacyDependency>()
            .AddScoped<Modern>()
            .AddScoped<ModernDependency>()
            .Configure<EnvironmentOptions>(o => o.Name = "Prod")
            .UseDiscriminator<IMyInterface, IOptions<EnvironmentOptions>>(options =>
                    options.Value.Name switch
                    {
                        "Dev" => typeof(Modern),
                        _ => typeof(Legacy),
                    });

        var serviceProvider = services.BuildServiceProvider();
        var some = serviceProvider.GetService<IMyInterface>();

        some.DoSomething();

    }
}

public static class DependencyInjectionEtensions
{
    public static IServiceCollection UseDiscriminator<TImpl, TDep>(this IServiceCollection services, Func<TDep, Type> discriminatorFunc) where TDep : class
    {
        // todo: scopes
        services.Configure<DiscriminatorOptions<TImpl, TDep>>(_ => _.TypeDiscriminator = discriminatorFunc);
        services.AddScoped<DiscriminatedImplementationFactory<TImpl, TDep>>();
        services.Add(new ServiceDescriptor(typeof(TImpl), sp => sp.GetRequiredService<DiscriminatedImplementationFactory<TImpl, TDep>>().Create(), ServiceLifetime.Transient));
        return services;
    }
}

public interface IMyInterface
{
    void DoSomething();
}

public interface IDiscriminatedImplementationFactory<TInterface>
{
    TInterface Create();
}

public class EnvironmentOptions { public string Name { get; set; } }
public class DiscriminatorOptions<TImpl, TDep>
{
    public Func<TDep, Type> TypeDiscriminator { get; set; } /* that shouldn't be options if its not serializable but... and Type isnt safe and doesn't guarantee that T implements TImpl and we cant check it */
}
public class DiscriminatedImplementationFactory<TImpl, TDep> : IDiscriminatedImplementationFactory<TImpl> where TDep : class
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TDep _dependency;
    private readonly DiscriminatorOptions<TImpl, TDep> _disOptions;

    public DiscriminatedImplementationFactory(IServiceProvider serviceProvider, TDep options, IOptions<DiscriminatorOptions<TImpl, TDep>> discriminatorOptions)
    {
        _serviceProvider = serviceProvider;
        _dependency = options;
        _disOptions = discriminatorOptions.Value;
    }

    public TImpl Create()
    {
        return (TImpl)_serviceProvider.GetRequiredService(_disOptions.TypeDiscriminator(_dependency));
    }
}

public class ModernDependency { public String Name = "Modern"; }
public class Modern : IMyInterface
{
    private readonly ModernDependency _dependency;

    public Modern(ModernDependency dependency)
    {
        _dependency = dependency;
    }
    public void DoSomething()
    {
        Console.WriteLine(_dependency.Name);
    }
}

public class LegacyDependency { public String Name => "Legacy"; }
public class Legacy : IMyInterface
{
    private readonly LegacyDependency _dependency;

    public Legacy(LegacyDependency dependency)
    {
        _dependency = dependency;
    }
    public void DoSomething()
    {
        Console.WriteLine(_dependency.Name);
    }
}