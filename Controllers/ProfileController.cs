using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HNGProfileAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class ProfileController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(IHttpClientFactory httpClientFactory, ILogger<ProfileController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        // Required endpoint: GET /me
        [HttpGet("/me")]
        public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient("CatFacts");
            string catFact = "Could not fetch cat fact at this time.";

            try
            {
                using var response = await httpClient.GetAsync("fact", cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
                    var jsonDoc = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
                    if (jsonDoc.RootElement.TryGetProperty("fact", out var factElement))
                    {
                        var fact = factElement.GetString();
                        if (!string.IsNullOrWhiteSpace(fact))
                        {
                            catFact = fact;
                        }
                    }
                }
                else
                {
                    _logger.LogWarning("Cat facts API returned non-success status {StatusCode}", response.StatusCode);
                }
            }
            catch (TaskCanceledException tex) when (cancellationToken.IsCancellationRequested == false)
            {
                _logger.LogWarning(tex, "Timeout when calling Cat Facts API");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching cat fact");
            }

            var result = new
            {
                status = "success",
                user = new
                {
                    email = "abdulmuheezabiola@gmail.com",
                    name = "Abdulmuheez Ogunrinde Abiola",
                    stack = "C#/dotNet"
                },
                timestamp = DateTime.UtcNow.ToString("o"),
                fact = catFact
            };
            return Ok(result);
        }
    }
}

