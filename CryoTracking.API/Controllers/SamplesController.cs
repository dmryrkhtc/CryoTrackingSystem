using CryoTracking.Application.DTOs.Sample;
using CryoTracking.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryoTracking.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SamplesController : ControllerBase
    {
        private readonly ISampleRepository _repo;

        public SamplesController(ISampleRepository repo)
        {
            _repo = repo;
        }
        [Authorize(Roles = "Sistem Yoneticisi,Embriyolog,Doktor")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repo.GetAllAsync();
            if (!result.Success)
                return NotFound(new { result.Message });
            return Ok(result.Data);
        }
        [Authorize(Roles = "Sistem Yoneticisi,Embriyolog,Doktor")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _repo.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(new { result.Message });
            return Ok(result.Data);
        }
        [Authorize(Roles = "Sistem Yoneticisi,Embriyolog,Doktor")]
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var result = await _repo.GetByPatientIdAsync(patientId);
            if (!result.Success)
                return NotFound(new { result.Message });
            return Ok(result.Data);
        }
        [Authorize(Roles = "Sistem Yoneticisi,Embriyolog")]
        [HttpPost]
        public async Task<IActionResult> Create(SampleCreateDto dto)
        {
            var result = await _repo.CreateAsync(dto);
            if (!result.Success)
                return BadRequest(new { result.Message });
            return CreatedAtAction(nameof(GetById), new { id = result.Data.SampleId }, result.Data);
        }
        [Authorize(Roles = "Sistem Yoneticisi,Embriyolog")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SampleUpdateDto dto)
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