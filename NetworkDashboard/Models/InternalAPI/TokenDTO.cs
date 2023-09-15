using Newtonsoft.Json;

namespace NetworkDashboard.Models.InternalAPI
{
    public class TokenDTO
    {
        public string refresh_token_expires_in;
        public string api_product_list;
        public List<string> api_product_list_json;
        public string organization_name;

        [JsonProperty("developer.email")]
        public string email;
        public string token_type;
        public string issued_at;
        public string client_id;
        public string access_token;
        public string application_name;
        public string scope;
        public int expires_in;
        public string refresh_count;
        public string status;
        public DateTime expireTime;
    }
}
