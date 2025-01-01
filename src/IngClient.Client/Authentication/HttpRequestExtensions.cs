using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using IngClient.Client.Authentication.Models;

namespace IngClient.Client.Authentication;

internal static class HttpRequestExtensions
{
    public static async Task<string> ComputeDigest(this HttpRequestMessage request)
    {
        var payload = await request.Content.ReadAsStringAsync();
        return $"SHA-256={payload.ComputeDigest()}";
    }

    public static string Sign(this HttpRequestMessage request, string date, string digest, AuthData authData)
    {
        var toSign = $"(request-target): {request.Method.ToString().ToLower()} {request.RequestUri}\ndate: {date}\ndigest: {digest}";;
        return toSign.Sign(authData.SignatureCert);
    }

    private static string ComputeDigest(this string data)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(data));
        return Convert.ToBase64String(hash);
    }

    private static string Sign(this string toSign, X509Certificate2 certificate)
    {
        var signed = certificate.GetRSAPrivateKey().SignData((byte[]?)Encoding.UTF8.GetBytes(toSign), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return Convert.ToBase64String(signed);
    }
}