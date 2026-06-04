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

        // 🌟 DÜZELTME: Doktor ve Embriyologların da kullanıcı listesini görmesine (veya rollerini denetlemesine) izin veriyoruz
        [Authorize(Roles = "Sistem Yoneticisi,Doktor,Embriyolog")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repo.GetAllAsync();
            if (!result.Success)
                return NotFound(new { result.Message });
            return Ok(result.Data);
        }

        [Authorize(Roles = "Sistem Yoneticisi,Doktor,Embriyolog")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _repo.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(new { result.Message });
            return Ok(result.Data);
        }

        // Yeni personel ekleme yetkisi yine sadece Sistem Yöneticisinde (Admin) kalsın, güvenliği koruyalım 🛡️
        [Authorize(Roles = "Sistem Yoneticisi")]
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateDto dto)
        {
            var result = await _repo.CreateAsync(dto);
            if (!result.Success)
                return BadRequest(new { result.Message });
            return CreatedAtAction(nameof(GetById), new { id = result.Data.UserId }, result.Data);
        }

        // 🌟 DÜZELTME: Durum güncelleme (Soft Delete) işlemini diğer yetkili personellerin de tetiklemesine izin veriyoruz
        [Authorize(Roles = "Sistem Yoneticisi,Doktor,Embriyolog")]
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