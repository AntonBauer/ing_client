namespace IngClient.Client.Models;

internal readonly record struct Transaction
{
    public required string TransactionId { get; init; }
    public required TransactionAmount Amount { get; init; }
    public required string DebtorName { get; init; }
    public required string TransactionType { get; init; }
}