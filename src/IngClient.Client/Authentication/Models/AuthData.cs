using System.Security.Cryptography.X509Certificates;

namespace IngClient.Client.Authentication.Models;

internal readonly record struct AuthData(X509Certificate2 RequestAuthCert,
                                         X509Certificate2 SignatureCert,
                                         string ClientId);