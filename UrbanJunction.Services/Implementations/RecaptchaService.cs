using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using UrbanJunction.Services.Interfaces;

namespace UrbanJunction.Services.Implementations
{
    public class RecaptchaService : IRecaptchaService
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public RecaptchaService(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> VerifyAsync(string token)
        {
            var secretKey = _config["GoogleReCaptcha:SecretKey"];
            var client = _httpClientFactory.CreateClient();

            var response = await client.PostAsync(
                "https://www.google.com/recaptcha/api/siteverify",
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "secret",   secretKey! },
                    { "response", token }
                })
            );

            var result = await response.Content.ReadFromJsonAsync<RecaptchaResponse>();
            return result?.Success ?? false;
        }

        private class RecaptchaResponse
        {
            [JsonPropertyName("success")]
            public bool Success { get; set; }
        }
    }
}
