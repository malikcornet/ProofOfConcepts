
using Microsoft.Graph;
using Microsoft.Identity.Client;

class Program
{
    static async Task Main(string[] args)
    {
        string clientId = "";
        string username = "";
        string password = "";
        string tenantId = "consumers";
        string filePath = @"C:\temp\test.txt"; 
        string fileName = "test"; 

        var app = PublicClientApplicationBuilder.Create(clientId)
            .WithAuthority(AzureCloudInstance.AzurePublic, tenantId)
            .Build();

        var result = await app.AcquireTokenByUsernamePassword(new[] { "Files.ReadWrite.All" }, username, SecureString(password)).ExecuteAsync();

        var graphClient = new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) =>
        {
            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.AccessToken);
            return Task.FromResult(0);
        }));

        using (var stream = new FileStream(filePath, FileMode.Open))
        {
            await graphClient.Me.Drive.Root.ItemWithPath(fileName).Content.Request().PutAsync<DriveItem>(stream);
        }
    }

    // Helper method to convert string to SecureString
    private static System.Security.SecureString SecureString(string str)
    {
        System.Security.SecureString secureStr = new System.Security.SecureString();
        foreach (char c in str)
        {
            secureStr.AppendChar(c);
        }
        return secureStr;
    }
}
