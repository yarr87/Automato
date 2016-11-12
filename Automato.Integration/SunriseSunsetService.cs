using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Automato.Integration
{
    public class SunriseSunsetService
    {
        private string ApiUrl = ConfigurationManager.AppSettings["SunsetSunrise.Url"];
        private readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static DateTime? SunsetToday { get; set; }
        private static DateTime? SunriseToday { get; set; }
        private static DateTime? LastRefreshDate { get; set; }
        private static int FailedApiAttemptsToday { get; set; }

        public async Task<DateTime?> GetSunriseToday()
        {
            await RefreshTimesFromApi();

            return SunriseToday;
        }

        public async Task<DateTime?> GetSunsetToday()
        {
            await RefreshTimesFromApi();

            return SunsetToday;
        }

        private async Task RefreshTimesFromApi()
        {
            if (LastRefreshDate.HasValue && LastRefreshDate.Value.Date == DateTime.Today)
            {
                return;
            }

            try
            {
                using (var client = new HttpClient())
                {
                    Logger.DebugFormat("Getting sunrise and sunset from {0}", ApiUrl);

                    var response = await client.GetAsync(ApiUrl);

                    if (response == null)
                    {
                        return;
                    }


                    var stream = response.Content as StreamContent;
                    var bytes = await stream.ReadAsByteArrayAsync();

                    var content = Encoding.UTF8.GetString(bytes);

                    JObject joResponse = JObject.Parse(content);
                    JObject ojObject = (JObject)joResponse["results"];

                    string sunrise = ojObject["sunrise"].ToString();
                    string sunset = ojObject["sunset"].ToString();

                    SunriseToday = DateTime.Parse(sunrise).ToLocalTime();
                    SunsetToday = DateTime.Parse(sunset).ToLocalTime();

                    LastRefreshDate = DateTime.Today;

                    Logger.DebugFormat("Got sunrise {0} and sunset {1} at {2}", SunriseToday, SunsetToday, DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error getting sunrise/sunset from " + ApiUrl, ex);
                FailedApiAttemptsToday++;
                SunriseToday = null;
                SunsetToday = null;
            }
        }

        private HttpClient GetClient()
        {
            return new HttpClient()
            {
                BaseAddress = new Uri(ApiUrl)
            };
        }
    }
}
