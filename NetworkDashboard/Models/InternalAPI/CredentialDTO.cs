namespace NetworkDashboard.Models.InternalAPI
{
    public class CredentialDTO
    {
        private string server;
        private string subDomain;

        public string DbPassword { get; set; }
        public string DbUsername { get; set; }
        public string RouterPassword { get; set; }
        public string RouterUsername { get; set; }
        public string Server { get => server; set => server = value; }
        public string SubDomain { get => subDomain; set => subDomain = value; }
    }
}
