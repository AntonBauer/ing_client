namespace IngClient.Client.Authentication.Models;

internal readonly record struct TokenResponse(
    string AccessToken,
    int ExpiresIn,
    string Scope,
    string TokenType,
    string RefreshToken,
    int RefreshTokenExpiresIn,
    string ClientId,
    List<Key> Keys
);
internal readonly record struct Key(
    string Kty,
    string Alg,
    string Use,
    string Kid,
    string N,
    string E,
    string X5t,
    List<string> X5c
);