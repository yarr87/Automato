using Automato.Integration.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Integration.Router
{
    /// <summary>
    /// Wrapper around the fios router, which has its own api for getting connected devices
    /// </summary>
    public class RouterApi : IDisposable
    {
        private bool _isLoggedIn = false;
        private HttpClient _client;

        private string _sessionCookie = string.Empty;
        private string _xsrfCookie = string.Empty;

        public RouterApi()
        {
            var url = ConfigurationManager.AppSettings["Router.Url"];

            _client = new HttpClient
            {
                BaseAddress = new Uri(url)
            };
        }

        /// <summary>
        /// Returns a list of all devices known by the local network and their connected status
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<NetworkDevice>> GetNetworkDevices()
        {
            await Login();

            var request = GetAuthenticatedRequest(HttpMethod.Get, "devices");

            var deviceResult = await _client.SendAsync(request);
            deviceResult.EnsureSuccessStatusCode();

            var deviceString = await deviceResult.Content.ReadAsStringAsync();

            var devices = JsonConvert.DeserializeObject<IEnumerable<NetworkDevice>>(deviceString);

            await Logout();

            return devices;
        }

        private async Task Login()
        {
            if (_isLoggedIn) return;

            var payload = new
            {
                password = ConfigurationManager.AppSettings["Router.Password"]
            };

            var content = JsonConvert.SerializeObject(payload);

            var result = await _client.PostAsync("login", new StringContent(content, Encoding.UTF8, "application/json"));

            var cookies = result.Headers.GetValues("Set-Cookie");

            var sessionCookie = string.Empty;
            var xsrfCookie = string.Empty;

            foreach (var cookie in cookies)
            {
                var cookieNameAndValue = cookie.Split(';')[0];

                var cookieParts = cookieNameAndValue.Split('=');

                var cookieName = cookieParts[0];
                var cookieValue = cookieParts[1];

                if (cookieName == "Session")
                {
                    _sessionCookie = cookieValue;
                }
                else if (cookieName == "XSRF-TOKEN")
                {
                    _xsrfCookie = cookieValue;
                }
            }

            if (string.IsNullOrWhiteSpace(_sessionCookie))
            {
                throw new Exception("Could not get session cookie from router login");
            }

            if (string.IsNullOrWhiteSpace(_xsrfCookie))
            {
                throw new Exception("Could not get xsrf cookie from router login");
            }
        }

        private async Task Logout()
        {
            var request = GetAuthenticatedRequest(HttpMethod.Get, "logout");
            await _client.SendAsync(request);
        }

        /// <summary>
        /// Return a request with login information included - session and xsrf cookies
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private HttpRequestMessage GetAuthenticatedRequest(HttpMethod method, string url)
        {
            var request = new HttpRequestMessage(method, url);
            request.Headers.Add("Cookie", "Session=" + _sessionCookie + "; XSRF-TOKEN=" + _xsrfCookie);
            request.Headers.Add("X-XSRF-TOKEN", _xsrfCookie);

            return request;
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
