using Microsoft.Extensions.DependencyInjection;

namespace IngClient.Client;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBankClient(this IServiceCollection services) =>
        services.AddTransient<IBankClient, BankClient>().AddIngHttpClient();

    private static IServiceCollection AddIngHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient(Constants.IngClientName, client =>
        {
            // ToDo: extract from configuration
            client.BaseAddress = new Uri("https://api.sandbox.ing.com/");
        })
        .ConfigurePrimaryHttpMessageHandler(provider =>
        {
            return new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
                // ToDo: extract certificates data from configuration
                // ClientCertificates = {}
            };
        });

        return services;
    }
}