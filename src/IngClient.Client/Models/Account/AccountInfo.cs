namespace IngClient.Client.Models.Account;

internal readonly record struct AccountInfo
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Iban { get; init; }
    public required string Currency { get; init; }
}
