
using Microsoft.AspNetCore.Mvc;
using TodoApi.Common;

namespace TodoApi.Controller
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public abstract class ApiV1BaseController : ControllerBase
    {
        private readonly ILogger _logger;

        protected ApiV1BaseController(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected IActionResult ToActionResult<T>(ApiResponse<T> result)
        {
            if (result == null)
            {
                _logger.LogError("ApiResponse<T> is null in ToActionResult.");
                var err = new ApiResponse<object>(null, false, "Internal Server Error")
                {
                    Errors = new List<string> { "Internal Server Error" }
                };
                return StatusCode(500, err);
            }

            if (!result.Success)
            {
                if (result.Errors != null && result.Errors.Any())
                {
                    _logger.LogWarning("Validation failed: {@Errors}", result.Errors);
                    return UnprocessableEntity(result);
                }

                if (!string.IsNullOrWhiteSpace(result.Message) &&
                    (result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase) ||
                     result.Message.Contains("does not exist", StringComparison.OrdinalIgnoreCase) ||
                     result.Message.Contains("not exist", StringComparison.OrdinalIgnoreCase)))
                {
                    _logger.LogWarning("Resource not found: {Message}", result.Message);
                    return NotFound(result);
                }

                _logger.LogWarning("Bad request: {Message}", result.Message);
                return BadRequest(result);
            }

            _logger.LogInformation("Request processed successfully: {@Response}", result);
            return Ok(result);
        }

        protected IActionResult ModelStateErrorResponse()
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => string.IsNullOrEmpty(e.ErrorMessage) ? e.Exception?.Message ?? "Invalid value" : e.ErrorMessage)
                .Distinct()
                .ToList();

            var resp = new ApiResponse<object>(null, false, "Validation failed")
            {
                Errors = errors
            };

            _logger.LogWarning("Model validation failed: {@Errors}", errors);
            return UnprocessableEntity(resp);
        }
    }

}
