using Microsoft.Extensions.DependencyInjection;

namespace IngClient.Client;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBankClient(this IServiceCollection services) =>
        services.AddTransient<IBankClient, BankClient>().AddIngHttpClient();

    private static IServiceCollection AddIngHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient<BankClient>(client =>
        {
            client.BaseAddress = new Uri("");
        });

        return services;
    }
}