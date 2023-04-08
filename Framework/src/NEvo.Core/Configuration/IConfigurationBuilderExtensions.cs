namespace Microsoft.Extensions.Configuration;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddJsonFiles(this IConfigurationBuilder builder, string pattern, bool reloadOnChange = false)
    {
        var fileNamePattern = Path.GetFileName(pattern);
        var secretsDir = Path.GetDirectoryName(pattern);

        if (Directory.Exists(secretsDir))
        {
            foreach (string filePath in Directory.GetFiles(secretsDir, fileNamePattern))
            {
                builder.AddJsonFile(filePath, optional: false, reloadOnChange: reloadOnChange);
            }
        }

        return builder;
    }
}
