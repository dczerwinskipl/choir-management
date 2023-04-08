namespace Microsoft.Extensions.Configuration;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddJsonFiles(this IConfigurationBuilder builder, string pattern, bool reloadOnChange = false)
    {
        var fileNamePattern = Path.GetFileName(pattern);
        var secretsDir = Path.GetDirectoryName(pattern);

        if (Directory.Exists(secretsDir))
        {
            Console.WriteLine("Exists");
            foreach (string filePath in Directory.GetFiles(secretsDir, fileNamePattern))
            {
                Console.WriteLine(filePath);
                builder.AddJsonFile(secretsDir + "/" + filePath, optional: false, reloadOnChange: reloadOnChange);
            }
        }

        return builder;
    }
}
