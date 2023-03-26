using Microsoft.Extensions.Options;
using System.Reflection;

namespace NEvo.Core;

public record NEvoAppDetails(Guid Id, string AppName);

public interface INEvoAppDetailsProvider
{
    NEvoAppDetails GetAppDetails();
}

public class NEvoAppDetailsProvider : INEvoAppDetailsProvider
{
    private readonly NEvoAppDetails _details;
    public NEvoAppDetailsProvider(NEvoAppDetails details)
    {
        _details = details;
    }

    public NEvoAppDetailsProvider(string appName)
    {
        _details = new NEvoAppDetails(Guid.NewGuid(), appName);
    }

    public NEvoAppDetailsProvider() : this(Assembly.GetEntryAssembly().GetName().Name)
    {
    }

    public NEvoAppDetails GetAppDetails() => _details;
}