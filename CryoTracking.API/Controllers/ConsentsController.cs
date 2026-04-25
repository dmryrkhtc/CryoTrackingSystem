using CryoTracking.Application.DTOs.Consent;
using CryoTracking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryoTracking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsentsController : ControllerBase
    {
        private readonly IConsentRepository _repo;

        public ConsentsController(IConsentRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var result = await _repo.GetByPatientIdAsync(patientId);
            if (!result.Success)
                return NotFound(new { result.Message });
            return Ok(result.Data);
        }

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