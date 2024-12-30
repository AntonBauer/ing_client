using System.Net;
using System.Text;

namespace IngClient.Tests.Client;

public class AuthTests
{
    [Test]
    public async Task Auth()
    {
        // Arrange
        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://api.sandbox.ing.com/");

        var clientId = "e77d776b-90af-4684-bebc-521e5b2614dd";
        var cert = @"MIIDEDCCAfigAwIBAgIEP8TNVTANBgkqhkiG9w0BAQsFADBKMRMwEQYDVQQKDApT
YW1wbGUgT3JnMRYwFAYDVQQLDA1JVCBEZXBhcnRtZW50MRswGQYDVQQDDBJleGFt
cGxlX2NsaWVudF90bHMwHhcNMjQwNTA3MTI1MDIyWhcNMjcwNTA4MTI1MDIyWjBK
MRMwEQYDVQQKDApTYW1wbGUgT3JnMRYwFAYDVQQLDA1JVCBEZXBhcnRtZW50MRsw
GQYDVQQDDBJleGFtcGxlX2NsaWVudF90bHMwggEiMA0GCSqGSIb3DQEBAQUAA4IB
DwAwggEKAoIBAQCpFifUxN3Q6Qsd90cASHZaJ1IBeugUsayk6Bn7fnXvUEm/VrgM
KK3N577WtW+FBLhjhK2gfMgIurctyfsMhg9swBdG0/ORz5Fr803u9FuMK29laX9w
IBPZRVPu+Y1nD0SBNDk2HrP7GFld390lyTr4rgH0dVHDBZVEDpKDzOBfBEP4Uv5u
6+zqM3CnUFoD3E01z1+80PUfakkUpCKG8eKvgpz5FABwYryrChMa0KIsdxf6+P8G
oqhaeIS8nT/CRxN5JcCpy7svuZN8lGeLC49xDRtCoaC8/Nk4UiOTWOWqeHsnfrnb
oC5ID/G/ZX6cCqWdVH8+Xg/tRmDblOgtx+wdAgMBAAEwDQYJKoZIhvcNAQELBQAD
ggEBADzd2pB6aiFSabPCsYwznPQFk18eJtHAJMiySrFF7uK/WkL8PojnRGZQjv32
9J1tPxh5IFVYWEAZXMyvdYctGbPh3M6RRJwhu7ioyK2NmevwqiD+ATnOioPPwoC2
EqmnWBCpAPLGQ/tCY7xQeltIo+3kwC6iXjqCnxOW2sd0em1zYjDk1xWg4G6zCNBc
Jc/SLNZIc4brk4TyaR7+5ysrNIy8KaS+QodEywmFDk4QLh6rPRyMqDengrOUyRce
sgrAv5bKzBOLRiVO3pXm7kdIW2eP4CuMmOiwU5uxhZXWxpiiHrTdrvSuL6oA5Y4l
0tk0bnC1xrioMGQgTjFyTzz61bw=".Replace("\r\n", string.Empty);

        var parameters = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", clientId }
        };
        var content = new FormUrlEncodedContent(parameters);

        content.Headers.Add("TPP-Signature-Certificate", cert);

        // Act
        var response = await client.PostAsync("oauth2/token", content);
        var raw = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}
