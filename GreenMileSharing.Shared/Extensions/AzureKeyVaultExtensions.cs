using Azure.Identity;
using GreenMileSharing.Shared.Managers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace GreenMileSharing.Shared.Extensions;

public static class AzureKeyVaultExtensions
{
    public static IConfigurationBuilder AddAzureKeyVault(this IConfigurationBuilder configuration)
    {
        if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable(Environments.Staging)))
        {
            return configuration;
        }
        
        return configuration.AddEnvironmentVariables()
            .AddAzureKeyVault(new Uri($"https://{Environment.GetEnvironmentVariable("AZURE_VAULT_NAME")}.vault.azure.net/"),
                new EnvironmentCredential(),
                new PrefixKeyVaultSecretManager("GreenMileSharing"));
    }
}