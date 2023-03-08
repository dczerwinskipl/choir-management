using Microsoft.Extensions.DependencyInjection;

namespace NEvo.Core;


/// <summary>
/// Internal interface to configure all extension for NEvo features
/// </summary>
public interface INEvoExtensionConfiguration
{
    void ConfigureServices(IServiceCollection services);
}

public class NEvoServicesBuilder
{
    private HashSet<INEvoExtensionConfiguration> _extensions = new HashSet<INEvoExtensionConfiguration>();

    public NEvoServicesBuilder() { }
    public NEvoServicesBuilder UseExtension<TExtension>() where TExtension : INEvoExtensionConfiguration, new() => UseExtension(new TExtension());
    public NEvoServicesBuilder UseExtension<TExtension>(TExtension extension) where TExtension : INEvoExtensionConfiguration
    {
        Check.Null(extension);
        _extensions.Add(extension);
        return this;
    }

    public void ApplyExtensions(IServiceCollection services)
    {
        foreach(var extension in _extensions)
        {
            extension.ConfigureServices(services);
        }
    }
}
