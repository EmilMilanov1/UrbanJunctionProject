using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using UrbanJunction.Services.Interfaces;

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

        // success = token was valid, score >= 0.5 = likely human
        return result?.Success == true && result.Score >= 0.5f;
    }

    private class RecaptchaResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("score")]
        public float Score { get; set; }

        [JsonPropertyName("action")]
        public string? Action { get; set; }
    }
}