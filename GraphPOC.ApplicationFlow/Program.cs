using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.Security;
using System.IO;

string filePath = @"C:\temp\test.txt";
var fileName = "test";

var scopes = new[] { "https://graph.microsoft.com/.default" };

var tenantId = "{tenant id}";

// Values from app registration  
var clientId = "{client id}";
var clientSecret = "{client secret}";

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