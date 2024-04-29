using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.Security;
using System.IO;

var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

IConfiguration config = builder.Build();

string filePath = @"C:\temp\test.txt";
var fileName = "test";

var scopes = new[] { "https://graph.microsoft.com/.default" };

var tenantId = config["tenantId"];

// Values from app registration  
var clientId = config["clientId"];
var clientSecret = config["clientSecret"];

// using Azure.Identity;  
var options = new TokenCredentialOptions
{
    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
};

// https://learn.microsoft.com/dotnet/api/azure.identity.clientsecretcredential  
var clientSecretCredential = new ClientSecretCredential(
    tenantId, clientId, clientSecret, options);

var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

using (var stream = new FileStream(filePath, FileMode.Open))
{
    await graphClient.Drives["test"].Root.ItemWithPath(fileName).Content.PutAsync(stream);
}