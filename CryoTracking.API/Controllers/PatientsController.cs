using CryoTracking.Application.DTOs.Patient;
using CryoTracking.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryoTracking.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientRepository _repo;

        public PatientsController(IPatientRepository repo)
        {
            _repo = repo;
        }
        [Authorize(Roles = "Sistem Yoneticisi,Doktor,Kayit Veri Yetkilisi")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repo.GetAllAsync();
            if (!result.Success)
                return NotFound(new { result.Message });
            return Ok(result.Data);
        }
        [Authorize(Roles = "Sistem Yoneticisi,Doktor,Kayit Veri Yetkilisi")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _repo.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(new { result.Message });
            return Ok(result.Data);
        }
        [Authorize(Roles = "Sistem Yoneticisi,Kayit Veri Yetkilisi")]
        [HttpPost]
        public async Task<IActionResult> Create(PatientCreateDto dto)
        {
            var result = await _repo.CreateAsync(dto);
            if (!result.Success)
                return BadRequest(new { result.Message });
            return CreatedAtAction(nameof(GetById), new { id = result.Data.PatientId }, result.Data);
        }
        [Authorize(Roles = "Sistem Yoneticisi,Kayit Veri Yetkilisi")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PatientUpdateDto dto)
        {
            var result = await _repo.UpdateAsync(id, dto);
            if (!result.Success)
                return BadRequest(new { result.Message });
            return NoContent();
        }
        [Authorize(Roles = "Sistem Yoneticisi")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repo.DeleteAsync(id);
            if (!result.Success)
                return BadRequest(new { result.Message });
            return NoContent();
        }
    }
}