namespace NetworkDashboard.Models
{
    public class OpsrampToken
    {
        public string access_token { get; set; }
        public string token_type { get;}
        public int expires_in { get;}
        public string scope { get;}
        public DateTime expireTime;
    }
}
