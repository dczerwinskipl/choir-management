using Azure.Identity;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Options;

namespace NEvo.Azure.Administrating
{
    public interface IAzureServiceBusAdministrator
    {
        Task EnsureTopicExistsAsync(string topicName);
        Task EnsureTopicExistsAsync(CreateTopicOptions createTopicOptions);

        Task EnsureSubscriptionExistsAsync(string topicName, string subscription);
        Task EnsureSubscriptionExistsAsync(CreateSubscriptionOptions createTopicOptions);
    }


    public class AzureServiceBusAdministrator : IAzureServiceBusAdministrator
    {
        private AzureServiceBusClientData _options;
        private ServiceBusAdministrationClient _administrationClient;

        private SemaphoreSlim _topicsSemaphoreSlim = new SemaphoreSlim(1, 1);
        private HashSet<string> _verifiedTopics = new HashSet<string>();

        private SemaphoreSlim _subscriptionsSemaphoreSlim = new SemaphoreSlim(1, 1);
        private HashSet<string> _verifiedSubscriptions = new HashSet<string>();

        public AzureServiceBusAdministrator(IOptions<AzureServiceBusClientData> options)
        {
            _options = options.Value;
            _administrationClient = new ServiceBusAdministrationClient(_options.FullyQualifiedNamespace,
               new ClientSecretCredential(_options.TenantId, _options.ClientId, _options.ClientSecret),
               new ServiceBusAdministrationClientOptions { });
        }

        public async Task EnsureTopicExistsAsync(string topicName) => 
            await EnsureTopicExistsAsync(new CreateTopicOptions(topicName)
            {
                EnablePartitioning = true,
                RequiresDuplicateDetection = true
            });

        public async Task EnsureTopicExistsAsync(CreateTopicOptions createTopicOptions)
        {
            if (_verifiedTopics.Contains(createTopicOptions.Name))
                return;

            // dont  want to create same topic twice
            await _topicsSemaphoreSlim.WaitAsync();
            try
            {
                // double check - could be verified while waiting
                if (_verifiedTopics.Contains(createTopicOptions.Name))
                    return;

                if (!await _administrationClient.TopicExistsAsync(createTopicOptions.Name))
                {
                    await _administrationClient.CreateTopicAsync(createTopicOptions);
                    await _administrationClient.CreateSubscriptionAsync(new CreateSubscriptionOptions(createTopicOptions.Name, "BrowseMessages")
                    {
                        RequiresSession = false
                    });
                }
                _verifiedTopics.Add(createTopicOptions.Name);
            } 
            finally
            {
                _topicsSemaphoreSlim.Release();
            }
        }

        public async Task EnsureSubscriptionExistsAsync(string topicName, string subscription) => await EnsureSubscriptionExistsAsync(new CreateSubscriptionOptions(topicName, subscription)
        {
            RequiresSession = true
        });

        public async Task EnsureSubscriptionExistsAsync(CreateSubscriptionOptions createSubscription)
        {
            if (_verifiedSubscriptions.Contains($"{createSubscription.TopicName}/{createSubscription.SubscriptionName}"))
                return;

            // dont  want to create same subscription twice
            await _topicsSemaphoreSlim.WaitAsync();
            try
            {
                // double check - could be verified while waiting
                if (_verifiedSubscriptions.Contains($"{createSubscription.TopicName}/{createSubscription.SubscriptionName}"))
                    return;

                if (!await _administrationClient.SubscriptionExistsAsync(createSubscription.TopicName, createSubscription.SubscriptionName))
                {
                    await _administrationClient.CreateSubscriptionAsync(createSubscription);
                }
                _verifiedSubscriptions.Add($"{createSubscription.TopicName}/{createSubscription.SubscriptionName}");
            }
            finally
            {
                _topicsSemaphoreSlim.Release();
            }
        }
    }
}
