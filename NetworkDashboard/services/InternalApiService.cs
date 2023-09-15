using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using NetworkDashboard.Models.InternalAPI;
using System.Net.Http;
using System.Net.Security;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace NetworkDashboard.services
{
    public class InternalApiService
    {
        IConfigurationRoot _config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json").Build();
        private TokenDTO? token;
        private bool GetToken()
        {
            //string sReturnToken = "";
            string sBasicAuthString, sBase64Auth;
            string sAPIEndPoint;
            int tryCount = 0;
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            do
            {
                try
                {
                    string sKey = _config["InternalAPISubscribeKey"];
                    string sSecret = _config["InternalAPISubscribeSecret"];
                    sAPIEndPoint = _config["InternalAPIHost"];

                    HttpClient client = new HttpClient(handler);
                    sBasicAuthString = $"{sKey}:{sSecret}";
                    sBase64Auth = Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes(sBasicAuthString));
                    client.BaseAddress = new Uri(sAPIEndPoint);
                    var values = new Dictionary<string, string> { { "grant_type", "client_credentials" }, { "Content-Type", "application/x-www-form-urlencoded" } };
                    string endpoint = "token";

                    client.DefaultRequestHeaders.Add("Authorization", string.Format("Basic {0}", sBase64Auth));

                    HttpResponseMessage response = client.PostAsync(endpoint, new FormUrlEncodedContent(values)).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        //token = response.Content.ReadAsAsync<TokenDTO>().Result;
                        token = JsonConvert.DeserializeObject<TokenDTO>(response.Content.ReadAsStringAsync().Result);
                        if (token is not null) token.expireTime = DateTime.Now.AddSeconds(token.expires_in);
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception ex)
                {
                    ++tryCount;
                    Thread.Sleep(3000);
                    Console.WriteLine(ex.Message);
                }
            } while (tryCount < 3);
            return false;
        }

        private static bool RemoteCertValidate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            return true;
        }


        private bool isTokenExpire()
        {
            if (token == null)
            {
                return true;
            }
            else
            {
                return DateTime.Compare(token.expireTime, DateTime.Now) < 0;
            }
        }
        public CredentialDTO? GetVaultSecret(string path)
        {
            int tryCount = 0;
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            do
            {
                try
                {
                    if (isTokenExpire())
                    {
                        this.GetToken();
                    }
                    string sAPIEndPoint = _config["InternalAPIHost"];
                    HttpClient client = new HttpClient(handler);
                    client.BaseAddress = new Uri(sAPIEndPoint);
                    string endpoint = _config["InternalAPIEndpoint:InternalVault"] + "GetSecret?customEngineName=" + _config["SecretsEngines"];
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", string.Format("{0} {1}", token.token_type, token.access_token));

                    var data = new StringContent("{ \"typeOfAuth\": \"LDAP\", \"engine\": \"KV\", \"namespace\": \"automation\", \"userName\": \"username1\", \"password\": \"password1\", \"path\": \"" + path + "\", \"typeOfResponse\": \"json\", \"role_id\": \"" + _config["roleId"] + "\", \"secret_id\": \"" + _config["secretId"] + "\"}", Encoding.UTF8, "application/json");
                    HttpResponseMessage response = client.PostAsync(endpoint, data).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<CredentialDTO>(response.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        return null;
                    }
                }
                catch 
                {
                    ++tryCount;
                    Thread.Sleep(3000);
                }
            } while (tryCount < 3);
            return null;
        }
    }

}