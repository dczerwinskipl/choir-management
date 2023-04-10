using NEvo.Core;
using NEvo.CQRS.Messaging;
using NEvo.Monads;
using System.Net;
using System.Text.Json;

namespace NEvo.CQRS.Transporting;

public class RestTransportChannel : ITransportChannel
{
    private readonly ICQRSRestClient _cQRSRestClient;

    public RestTransportChannel(ICQRSRestClient cQRSRestClient)
    {
        _cQRSRestClient = cQRSRestClient;
    }

    public async Task<Either<Exception, TResult>> DispatchMessageAsync<TMessage, TResult>(MessageEnvelope<TMessage> messageEnvelope) where TMessage : IMessage<TResult>
        => await _cQRSRestClient.DispatchMessage<TResult>(messageEnvelope.ToRawMessageEnvelope());

    public Task<Either<Exception, Unit>> PublishMessageAsync<TMessage>(MessageEnvelope<TMessage> messsageEnvelope) where TMessage : IMessage<Unit>
    {
        throw new NotImplementedException();
    }
}

public class CQRSRestClient : RestClient, ICQRSRestClient
{
    public CQRSRestClient(IHttpClientFactory httpClientFactory, Uri baseAddress) : base(httpClientFactory, baseAddress)
    {
    }

    public async Task<Either<Exception, TResult>> DispatchMessage<TResult>(MessageEnvelope messageEnvelope) =>
        await PostAsync<TResult>("/DispatchCommand", messageEnvelope);
}

public interface ICQRSRestClient : IRestClient
{
    Task<Either<Exception, TResult>> DispatchMessage<TResult>(MessageEnvelope messageEnvelope);
}

public class RestClient : IRestClient
{
    protected readonly IHttpClientFactory _httpClientFactory;
    private readonly Uri _baseAddress;

    public RestClient(IHttpClientFactory httpClientFactory, Uri baseAddress)
    {
        _httpClientFactory = httpClientFactory;
        _baseAddress = baseAddress;
    }

    public async Task<Either<Exception, TResult>> PostAsync<TResult>(string method, object data)
    {
        var requrestUri = new Uri(_baseAddress, _baseAddress.AbsolutePath + method);
        using (var client = _httpClientFactory.CreateClient())
        {
            try
            {
                var httpContent = CreatePostBody(data); //todo: change to config
                var response = await client.PostAsync(requrestUri, httpContent);
                if (response.IsSuccessStatusCode)
                {
                    return await CreateResultAsync<TResult>(response.Content); //todo: chanage to config
                }
                else
                {
                    if (response.Content != null)
                    {
                        var message = await response.Content.ReadAsStringAsync();
                        return new HttpClientException(response.StatusCode, message);
                    }
                    return new HttpClientException(response.StatusCode);
                }
            }
            catch (Exception e)
            {
                return e;
            }
        }
    }

    private async Task<TResult> CreateResultAsync<TResult>(HttpContent responseContent)
    {
        var responseString = await responseContent.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResult>(responseString, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
    }

    private HttpContent CreatePostBody(object data)
    {
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        return new StringContent(json, System.Text.Encoding.UTF8, "application/json");
    }
}

public interface IRestClient
{

}

public class HttpClientException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public HttpClientException(HttpStatusCode statusCode, string? message = null) : base(message ?? $"Uknown error ({statusCode})")
    {
        StatusCode = statusCode;
    }
}