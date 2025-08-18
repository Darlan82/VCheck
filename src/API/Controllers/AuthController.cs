using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;

namespace VCheck.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public AuthController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var keycloakUrl = _config["services:keycloak:http:0"] ?? "http://localhost:8080";
            var realm = _config["Keycloak:Realm"] ?? "transport";
            var clientId = _config["Keycloak:Audience"] ?? "vcheck-api";
            var clientSecret = _config["Keycloak:Secret"] ?? "S3cr3t";

            var tokenEndpoint = $"{keycloakUrl}/realms/{realm}/protocol/openid-connect/token";
            var client = _httpClientFactory.CreateClient();

            var content = new StringContent($"grant_type=password&client_id={clientId}&client_secret={clientSecret}&username={request.Username}&password={request.Password}", Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await client.PostAsync(tokenEndpoint, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return BadRequest(responseBody);

            var json = JsonDocument.Parse(responseBody);
            var accessToken = json.RootElement.GetProperty("access_token").GetString();
            return Ok(new { access_token = accessToken });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
