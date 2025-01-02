using System.Security.Cryptography.X509Certificates;
using IngClient.Client.Authentication;
using IngClient.Client.Models.Auth;
using Moq;

namespace IngClient.Tests.Client;

[TestFixture]
internal sealed class AccountInfoTests
{
    private AuthData? _authData;
    private TokenResponse? _tokenResponse;
    private IHttpClientFactory? _httpClientFactory;

    [SetUp]
    public async Task Setup()
    {
        _authData = CreateAuthData();

        var client = new HttpClient(new HttpClientHandler()
        {
            ClientCertificateOptions = ClientCertificateOption.Manual,
            SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
            ClientCertificates = { _authData.Value.RequestAuthCert }
        })
        {
            BaseAddress = new Uri("https://api.sandbox.ing.com/")
        };

        var factoryMock = new Mock<IHttpClientFactory>();
        factoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);
        _httpClientFactory = factoryMock.Object;

        var authClient = new AuthenticationClient(_httpClientFactory);

        _tokenResponse = await authClient.TokenAsync(_authData.Value, CancellationToken.None);
    }

    [TearDown]
    public void TearDown()
    {
        _authData = null;
        _tokenResponse = null;
        _httpClientFactory = null;
    }

    [Test]
    public async Task GetAccountInfo()
    {
        // Arrange
        var client = new AccountInformationClient(_httpClientFactory);

        // Act
        var info = await client.AccountDetailsAsync(_authData.Value, _tokenResponse.Value, CancellationToken.None);

        // Assert
        Assert.That(info, Is.Not.Empty);
    }

    [Test]
    public async Task GetTransactions()
    {
        Assert.Pass();
    }

    private static AuthData CreateAuthData()
    {
        var auth = X509CertificateLoader.LoadPkcs12FromFile("/home/anton/Downloads/auth.pfx", "a");
        var sign = X509CertificateLoader.LoadPkcs12FromFile("/home/anton/Downloads/sign.pfx", "a");
        var clientId = "e77d776b-90af-4684-bebc-521e5b2614dd";

        return new(auth, sign, clientId);
    }
}