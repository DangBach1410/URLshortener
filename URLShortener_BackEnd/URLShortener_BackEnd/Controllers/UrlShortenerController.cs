using Microsoft.AspNetCore.Mvc;
using URLShortener_BackEnd.Models;
using URLShortener_BackEnd.Services;

namespace URLShortener_BackEnd.Controllers
{
    [Route("")]
    [ApiController]
    public class UrlShortenerController : ControllerBase
    {
        private readonly UrlShortenerService _urlShortenerService;

        public UrlShortenerController(UrlShortenerService urlShortenerService)
        {
            _urlShortenerService = urlShortenerService;
        }
        [HttpPost("shorten")]
        public async Task<IActionResult> ShortenUrl([FromBody] ShortUrl request)
        {
            if (string.IsNullOrWhiteSpace(request.OriginalUrl))
            {
                return BadRequest("URL cannot be empty.");
            }

            // Ensure the URL has a scheme (http:// or https://)
            if (!request.OriginalUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !request.OriginalUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                request.OriginalUrl = "http://" + request.OriginalUrl; // Default to http://
            }

            if (!Uri.IsWellFormedUriString(request.OriginalUrl, UriKind.Absolute))
            {
                return BadRequest("Invalid URL format.");
            }

            // Check if the URL actually exists
            if (!await UrlExistsAsync(request.OriginalUrl))
            {
                return BadRequest("The provided URL does not exist or is unreachable.");
            }

            var shortUrl = await _urlShortenerService.CreateShortUrlAsync(request.OriginalUrl);
            return Ok(shortUrl);
        }

        // Method to check if the URL exists
        private async Task<bool> UrlExistsAsync(string url)
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(5); // Set timeout to avoid long waits

                var response = await httpClient.GetAsync(url);
                return response.IsSuccessStatusCode; // Returns true if 200-299 status code
            }
            catch
            {
                return false; // URL is unreachable or invalid
            }
        }
        [HttpGet("{shortCode}")]
        public async Task<IActionResult> RedirectToOriginalUrl(string shortCode)
        {
            var urlEntry = await _urlShortenerService.GetByShortCodeAsync(shortCode);
            if (urlEntry == null)
            {
                return NotFound("Short URL not found.");
            }

            return Redirect(urlEntry.OriginalUrl);
        }
    }
}
