using System.Text.Json.Serialization;

namespace IngClient.Client.Models.Auth;

internal readonly record struct TokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; init; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; init; }

    [JsonPropertyName("scope")]
    public string Scope { get; init; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; init; }

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; init; }

    [JsonPropertyName("refresh_token_expires_in")]
    public int RefreshTokenExpiresIn { get; init; }

    [JsonPropertyName("client_id")]
    public string ClientId { get; init; }

    [JsonPropertyName("keys")]
    public List<Key> Keys { get; init; }

    internal readonly record struct Key
    {
        [JsonPropertyName("kty")]
        public string Kty { get; init; }

        [JsonPropertyName("alg")]
        public string Alg { get; init; }

        [JsonPropertyName("use")]
        public string Use { get; init; }

        [JsonPropertyName("kid")]
        public string Kid { get; init; }

        [JsonPropertyName("n")]
        public string N { get; init; }

        [JsonPropertyName("e")]
        public string E { get; init; }

        [JsonPropertyName("x5t")]
        public string X5t { get; init; }

        [JsonPropertyName("x5c")]
        public List<string> X5c { get; init; }
    }
}