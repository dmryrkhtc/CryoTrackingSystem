using CryoTracking.Application.DTOs.Consent;
using CryoTracking.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryoTracking.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ConsentsController : ControllerBase
    {
        private readonly IConsentRepository _repo;

        public ConsentsController(IConsentRepository repo)
        {
            _repo = repo;
        }
        [Authorize(Roles = "Sistem Yoneticisi,Doktor,Kayit Veri Yetkilisi")]
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var result = await _repo.GetByPatientIdAsync(patientId);
            if (!result.Success)
                return NotFound(new { result.Message });
            return Ok(result.Data);
        }
        [Authorize(Roles = "Sistem Yoneticisi,Kayit Veri Yetkilisi")]
        [HttpPost]
        public async Task<IActionResult> Create(ConsentCreateDto dto)
        {
            var result = await _repo.CreateAsync(dto);
            if (!result.Success)
                return BadRequest(new { result.Message });
            return Ok(result.Data);
        }
    }
}