using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Vekalat.Application.Common.Helpers
{
    public interface IGoogleRecaptcha
    {
        Task<bool> IsSatisfy(string response);
    }
    public class GoogleRecaptcha : IGoogleRecaptcha
    {
        private readonly IConfiguration _configuration;
        public GoogleRecaptcha(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> IsSatisfy(string response)
        {
            var secretKey = _configuration.GetSection("GoogleReCaptcha")["SecretKey"];
            var http = new HttpClient();
            var result = await http.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={response}", null);
            if (result.IsSuccessStatusCode)
            {
                var googleResponse = JsonConvert.DeserializeObject<RecaptchaResponse>(await result.Content.ReadAsStringAsync());

                if (googleResponse == null)
                    return false;

                return googleResponse.Success;
            }

            return false;
        }
    }

    public class RecaptchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("challenge_ts")]
        public DateTimeOffset ChallengeTs { get; set; }

        [JsonProperty("hostname")]
        public string HostName { get; set; }

    }
}