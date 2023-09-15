using Dapper;
using Npgsql;
using NetworkDashboard.services;
using NetworkDashboard.Models.InternalAPI;
using System.Drawing.Text;
using Microsoft.Graph;
using System.Drawing;
using NuGet.Protocol;

namespace NetworkDashboard.Models
{
    public class HeatmapDatabase
    {
        IConfigurationRoot _config = new ConfigurationBuilder().SetBasePath(System.IO.Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json").Build();
        InternalApiService InternalAPIService = new InternalApiService();
        private CredentialDTO credentialDTO;
        public HeatmapDatabase() { credentialDTO = InternalAPIService.GetVaultSecret("databaseName"); }
        
        public async Task<IEnumerable<Networkdevice>> GetNetworkdeviceList()
        {
            try
            {
                NpgsqlConnection connection = GetSqlConnection();
                connection.Open();

                string commandText = "SELECT * FROM public.table1";
                var deviceList = await connection.QueryAsync<Networkdevice>(commandText);
                return deviceList;
            }
            catch (Exception exception)
            {
                return Enumerable.Empty<Networkdevice>(); ;
            }
            
        }
        public async Task<IEnumerable<Networkdevice>> GetNetworkdevice(string networkdeviceName)
        {
            try
            {
                NpgsqlConnection connection = GetSqlConnection();
                connection.Open();
                string commandText = $@"SELECT * FROM public.table1 where device = @device";
                var queryArguments = new
                {
                    device = networkdeviceName
                };
                var deviceList = await connection.QueryAsync<Networkdevice>(commandText, queryArguments);
                return deviceList;
            }
            catch (Exception exception)
            {
                return Enumerable.Empty<Networkdevice>(); ;
            }

        }
        public async Task<String> AddNetworkdevice(string device, string IP, string Column7, string Column3, string Column1, string Column2, string Column4)
        {
            try
            {
                NpgsqlConnection connection = GetSqlConnection();
                connection.Open();
                string conversionCommandText = $@"SELECT * FROM public.table2 where device_name = @device_name limit 1";
                var conversionQueryArguments = new
                {
                    device_name = "Available"
                };
                IEnumerable<ConversionTable> conversionTables = await connection.QueryAsync<ConversionTable>(conversionCommandText, conversionQueryArguments);
                string availableCode = "";
                foreach (ConversionTable conversionTable in conversionTables.ToList())
                {
                    availableCode = conversionTable.code;
                }
                await connection.ExecuteAsync($"UPDATE public.table2 SET device_name = '{device}' WHERE code = '{availableCode}'");
                string commandText = $@"INSERT INTO public.table1 (device, IP, Column7, Column8, updateddate, Column3, Column1, Column2, Column4, code) VALUES (@device, @IP, @Column7, Now(), Now(), @Column3, @Column1, @Column2, @Column4, @code)";

                var queryArguments = new
                {
                    device = device,
                    IP = IP,
                    Column7 = Column7,
                    Column3 = Column3,
                    Column1 = Column1,
                    Column2 = Column2,
                    Column4 = Column4,
                    code = availableCode
                };

               
                await connection.ExecuteAsync(commandText, queryArguments);
                return "Successfully add device " + device + " to the list.";
            }
            catch (Exception exception)
            {
                return "Failed to add device with error message: " + exception.Message;
            }
            
        }
        public async Task<String> EditNetworkdevice(string device, string IP, string Column3, string Column1, string Column2, string Column4)
        {
            try
            {
                NpgsqlConnection connection = GetSqlConnection();
                connection.Open();

                string commandText = $@"UPDATE public.table1 SET IP = @IP, updateddate = Now(), Column3 = @Column3, Column1 = @Column1, Column2 = @Column2, Column4 = @Column4 WHERE device = @device";
                var queryArguments = new
                {
                    device = device,
                    IP = IP,
                    Column3 = Column3,
                    Column1 = Column1,
                    Column2 = Column2,
                    Column4 = Column4
                };

                //await connection.ExecuteAsync($"INSERT INTO public.table1 (device, IP, vrf, Column7, Column8, updateddate) VALUES ('{networkdevice.device}', '{networkdevice.IP}', '{networkdevice.vrf}', '{networkdevice.Column7}', Now(), Now())");
                await connection.ExecuteAsync(commandText, queryArguments);
                return "Successfully update information of device " + device + ".";
            }
            catch (Exception exception)
            {
                return "Failed to update device information with error message: " + exception.Message;
            }

        }
        public async Task<string> DeleteNetworkdevice(string device)
        {
            try
            {
                NpgsqlConnection connection = GetSqlConnection();
                connection.Open();

                //string commandText = $"DELETE FROM public.table1 WHERE device=(@device)";

                //var queryArguments = new { device = device };
                await connection.ExecuteAsync($"UPDATE public.table2 SET device_name = 'Available' WHERE device_name = '{device}'");
                await connection.ExecuteAsync($"DELETE FROM public.table1 WHERE device= '{device}'");
                return "Successfully remove device " + device + " from the list.";
            }
            catch (Exception exception)
            {
                return "Failed to add device with error message: " + exception.Message;
            }
            
        }
        private NpgsqlConnection GetSqlConnection()
        {
            string userID = credentialDTO.DbUsername;
            string userPassword = credentialDTO.DbPassword;
            string databaseHost = _config["DatabaseHost"];
            return new NpgsqlConnection($"User ID={userID};Password={userPassword};Host={databaseHost};Port=5432;Database=databaseName;");
        }
    }

    public class Networkdevice
    {
        public int id { get; set; }
        public string device { get; set; }
        public string Column6 { get; set; }
        public string Column7 { get; set; }
        public string Column8 { get; set; }
        public string updateddate { get; set; }
        public int Column10 { get; set; }
        public string Column3 { get; set; }
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public string Column4 { get; set; }
        public string code { get; set; }
    }
    public class ConversionTable
    {
        public string device_name { get; set; }
        public string code { get; set; }
    }
}
