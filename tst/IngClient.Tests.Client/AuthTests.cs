using System.Security.Cryptography.X509Certificates;
using IngClient.Client.Authentication;
using IngClient.Client.Models.Auth;
using Moq;

namespace IngClient.Tests.Client;

[TestFixture]
internal sealed class AuthTests
{
    [Test]
    public async Task Auth()
    {
        // Arrange
        var authData = CreateAuthData();
        using var client = new HttpClient(new HttpClientHandler()
        {
            ClientCertificateOptions = ClientCertificateOption.Manual,
            SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
            ClientCertificates = { authData.RequestAuthCert }
        });
        client.BaseAddress = new Uri("https://api.sandbox.ing.com/");

        var factoryMock = new Mock<IHttpClientFactory>();
        factoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);

        var authClient = new AuthenticationClient(factoryMock.Object);

        // Act
        var tokenResponse = await authClient.TokenAsync(authData, CancellationToken.None);

        // Assert
        Assert.That(tokenResponse.AccessToken, Is.Not.Null);
    }

    private static AuthData CreateAuthData()
    {
        var auth = X509CertificateLoader.LoadPkcs12FromFile("/home/anton/Downloads/auth.pfx", "a");
        var sign = X509CertificateLoader.LoadPkcs12FromFile("/home/anton/Downloads/sign.pfx", "a");
        var clientId = "e77d776b-90af-4684-bebc-521e5b2614dd";

        return new(auth, sign, clientId);
    }
}
