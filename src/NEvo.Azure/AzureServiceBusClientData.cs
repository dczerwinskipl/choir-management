namespace NEvo.Azure;

public class AzureServiceBusClientData
{
    public AzureServiceBusClientData() { }
    public AzureServiceBusClientData(string fullyQualifiedNamespace, string tenantId, string clientId, string clientSecret)
    {
        FullyQualifiedNamespace = fullyQualifiedNamespace;
        TenantId = tenantId;
        ClientId = clientId;
        ClientSecret = clientSecret;
    }

    public string? FullyQualifiedNamespace { get; set; }
    public string? TenantId { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
}
