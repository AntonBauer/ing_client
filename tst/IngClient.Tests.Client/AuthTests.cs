using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace IngClient.Tests.Client;

public class AuthTests
{
    [Test]
    public async Task Auth()
    {
        // Arrange
        var certs = LoadCertificates();
        using var client = new HttpClient(new HttpClientHandler()
        {
            ClientCertificateOptions = ClientCertificateOption.Manual,
            SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
            ClientCertificates = { certs.RequestAuth }
        });
        client.BaseAddress = new Uri("https://api.sandbox.ing.com/");

        var clientId = "e77d776b-90af-4684-bebc-521e5b2614dd";
        var request = await CreateRequest(certs.Signature, clientId);

        // Act
        var response = await client.SendAsync(request);
        var raw = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private static async Task<HttpRequestMessage> CreateRequest(X509Certificate2 certificate, string clientId)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "oauth2/token")
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string> { { "grant_type", "client_credentials" } }),
        };

        AddContent(request);
        await AddHeaders(request, certificate, clientId);

        return request;
    }

    private static void AddContent(HttpRequestMessage request)
    {
        var parameters = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
        };

        request.Content = new FormUrlEncodedContent(parameters);
    }

    private static async Task AddHeaders(HttpRequestMessage request, X509Certificate2 certificate, string clientId)
    {
        var payload = await request.Content.ReadAsStringAsync();
        var digest = $"SHA-256={ComputeDigest(payload)}";

        var date = DateTime.UtcNow.ToString("r");

        var toSign = $"(request-target): post /oauth2/token\ndate: {date}\ndigest: {digest}";;
        var signature = Sign(certificate, toSign);

        request.Headers.Clear();
        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("Date", date);
        request.Headers.Add("Digest", digest);
        request.Headers.Add("Authorization", $"Signature keyId=\"{clientId}\",algorithm=\"rsa-sha256\",headers=\"(request-target) date digest\",signature=\"{signature}\"");
    }

    private static (X509Certificate2 RequestAuth, X509Certificate2 Signature) LoadCertificates()
    {
        var auth = X509CertificateLoader.LoadPkcs12FromFile("/home/anton/Downloads/auth.pfx", "a");
        var sign = X509CertificateLoader.LoadPkcs12FromFile("/home/anton/Downloads/sign.pfx", "a");

        return (auth, sign);
    }

    private static string Sign(X509Certificate2 certificate, string toSign)
    {
        var data = Encoding.UTF8.GetBytes(toSign);
        var signed = certificate.GetRSAPrivateKey().SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        var base64 = Convert.ToBase64String(signed);

        return base64;
    }

    private static string ComputeDigest(string data)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(data));
        return Convert.ToBase64String(hash);
    }
}
