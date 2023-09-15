using Microsoft.Graph;
using Microsoft.Identity.Client;
using NetworkDashboard.Controllers;

namespace NetworkDashboard.classes
{
    public class GraphApiClientUI
    {
        private readonly GraphServiceClient _graphServiceClient;

        public GraphApiClientUI(GraphServiceClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
        }

        public async Task<string> GetPhoto()
        {
            try
            {
                // Get user photo
                using (var photoStream = await _graphServiceClient.Me.Photo.Content.Request().GetAsync())
                {
                    byte[] photoByte = ((MemoryStream)photoStream).ToArray();
                    return Convert.ToBase64String(photoByte);
                }
            }
            catch (Exception pex)
            {
                Console.WriteLine($"{pex.Message}");
                return "";
            }
        }
    }
}
