using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace BlogWeb.Utilities.GoogleRecapcha
{
    public interface IGoogleService
    {
        Task<bool> IsSatisfyAsync();
    }

    public class GoogleService : IGoogleService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public GoogleService(IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        public async Task<bool> IsSatisfyAsync()
        {
            var secretKey = _configuration["GoogleRecapcha:Secretkey"];
            var response = _contextAccessor.HttpContext.Request.Form["g-recaptcha-response"];

            var http = new HttpClient();
            var result =await http.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={response}", null);
            if (result.IsSuccessStatusCode)
            {
                var googleResponse =
                    JsonConvert.DeserializeObject<RecapchaResponse>(await result.Content.ReadAsStringAsync());
                return googleResponse.Success;
            }
            return false;
        }
    }

    public class RecapchaResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("challenge_ts")]
        public DateTimeOffset ChallengeTs { get; set; }
        [JsonProperty("hostname")]
        public string HostName { get; set; }

    }
}
