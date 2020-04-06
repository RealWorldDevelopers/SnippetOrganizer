
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using System;


namespace SnippetOrganizer
{
   public class Program
   {
      public static void Main(string[] args)
      {
         var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
         if (environment == Environments.Development)
            CreateWebHostBuilder(args).Build().Run();
         else
            CreateHostBuilder(args).Build().Run();
      }

      // original with no key vault
      public static IHostBuilder CreateWebHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
              .ConfigureWebHostDefaults(webBuilder =>
              {
                 webBuilder.UseStartup<Startup>();
              });

      // using key vault
      public static IHostBuilder CreateHostBuilder(string[] args) =>
              Host.CreateDefaultBuilder(args)
                  .ConfigureAppConfiguration((context, config) =>
                  {
                     var keyVaultEndpoint = GetKeyVaultEndpoint();
                     if (!string.IsNullOrEmpty(keyVaultEndpoint))
                     {
                        var azureServiceTokenProvider = new AzureServiceTokenProvider();
                        var keyVaultClient = new KeyVaultClient(
                               new KeyVaultClient.AuthenticationCallback(
                                   azureServiceTokenProvider.KeyVaultTokenCallback));
                        config.AddAzureKeyVault(keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
                     }
                  })
                  .ConfigureWebHostDefaults(webBuilder =>
                  {
                     webBuilder.UseStartup<Startup>();
                  });

      private static string GetKeyVaultEndpoint() => "https://RWD-Secrets.vault.azure.net";

   }
}
