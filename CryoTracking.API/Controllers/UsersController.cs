using CryoTracking.Application.DTOs.User;
using CryoTracking.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryoTracking.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;

        public UsersController(IUserRepository repo)
        {
            _repo = repo;
        }
        [Authorize(Roles = "Sistem Yoneticisi")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repo.GetAllAsync();
            if (!result.Success)
                return NotFound(new { result.Message });
            return Ok(result.Data);
        }
       [Authorize(Roles = "Sistem Yoneticisi")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _repo.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(new { result.Message });
            return Ok(result.Data);
        }
        [Authorize(Roles = "Sistem Yoneticisi")]
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateDto dto)
        {
            var result = await _repo.CreateAsync(dto);
            if (!result.Success)
                return BadRequest(new { result.Message });
            return CreatedAtAction(nameof(GetById), new { id = result.Data.UserId }, result.Data);
        }
        [Authorize(Roles = "Sistem Yoneticisi")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserUpdateDto dto)
        {
            var result = await _repo.UpdateAsync(id, dto);
            if (!result.Success)
                return BadRequest(new { result.Message });
            return NoContent();
        }
    }
}