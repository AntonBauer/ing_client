namespace IngClient.Client.Models.Account;

internal readonly record struct Transactions
{
    public IReadOnlyList<Transaction> Booked { get; init; }
    public IReadOnlyList<Transaction> Info { get; init; }
    public IReadOnlyList<Transaction> Pending { get; init; }
}