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
            if (!Uri.IsWellFormedUriString(request.OriginalUrl, UriKind.Absolute))
            {
                return BadRequest("Invalid URL format.");
            }

            var shortUrl = await _urlShortenerService.CreateShortUrlAsync(request.OriginalUrl);
            return Ok(shortUrl);
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
