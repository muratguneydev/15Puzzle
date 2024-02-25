namespace FifteenPuzzle.Play.Cli.Tests;

using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;

public class HttpMessageHandlerStub : HttpMessageHandler
{
	private const string BaseAddress = "https://test";
    public HttpMessageHandlerStub(string clientName) => _clientName = clientName;

    private readonly Dictionary<string,HttpResponseMessage> _responses = new();
    private readonly string _clientName;

    public void AddResponse(string uri, string json)
	{
		var response = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(json, Encoding.UTF8, "application/json"),
            };
		_responses.Add($"{BaseAddress}/{uri}", response);
	}

	// public IHttpClientFactory GetHttpClientFactory()
	// {
	// 	var httpClient = new HttpClient(this);//TODO: singleton?
	// 	var httpClientFactoryStub = new Mock<IHttpClientFactory>();
	// 	httpClientFactoryStub
	// 		.Setup(factory => factory.CreateClient(It.IsAny<string>()))
	// 		.Returns(httpClient);
	// 	return httpClientFactoryStub.Object;
	// }

	public void SetUpHttpClientFactory(Mock<IHttpClientFactory> httpClientFactoryStub)
	{
        var httpClient = new HttpClient(this)
        {
            BaseAddress = new Uri(BaseAddress)
        };//TODO: singleton?
        httpClientFactoryStub
			.Setup(factory => factory.CreateClient(_clientName))
			.Returns(httpClient);
	}

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
		Task.FromResult(_responses[request.RequestUri!.ToString()]);
}