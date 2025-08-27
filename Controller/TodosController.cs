using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TodoApi.Common;
using TodoApi.Controller;
using TodoApi.Dtos;
using TodoApi.Models;
using TodoApi.Service;

namespace TodoApi.Controllers
{
    
    public class TodosController : ApiV1BaseController
    {
        private readonly ITodoService _service;
        private readonly ILogger<TodosController> _logger;

        public TodosController(ITodoService service, ILogger<TodosController> logger) : base(logger)
        {
            _service = service;
            _logger = logger;
        }

       

        [HttpGet("getall")]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var userId = GetUserId();
            var res = await _service.GetTodosAsync(userId, page, pageSize);
            return ToActionResult(new ApiResponse<PagedResult<TodoDto>>(res));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var userId = GetUserId();
            var dto = await _service.GetByIdAsync(userId, id);
            if (dto == null) return ToActionResult(new ApiResponse<TodoDto>(null, false, "Not found", false));
            return ToActionResult(new ApiResponse<TodoDto>(dto));
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateTodoDto dto)
        {
            var userId = GetUserId();
            var created = await _service.CreateAsync(userId, dto);
            return ToActionResult(new ApiResponse<TodoDto>(created));
        }

        [HttpPut("update/{id}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTodoDto dto)
        {
            var userId = GetUserId();
            var updated = await _service.UpdateAsync(userId, id, dto);
            if (!updated) return ToActionResult(new ApiResponse<object>(null, false, "Update failed or not found", false));
            return ToActionResult(new ApiResponse<object>(null, true, "Updated successfully"));
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = GetUserId();
            var ok = await _service.DeleteAsync(userId, id);
            if (!ok) return ToActionResult(new ApiResponse<object>(null, false, "Delete failed or not found", false));
            return ToActionResult(new ApiResponse<object>(null, true, "Deleted"));
        }
        private Guid GetUserId()
        {
            var sub = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            return Guid.TryParse(sub, out var id) ? id : Guid.Empty;
        }
    }
}
