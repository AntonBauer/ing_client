namespace IngClient.Client.Models.Account;

internal readonly record struct TransactionAmount
{
    public required string Currency { get; init; }
    public required decimal Content { get; init; }
}