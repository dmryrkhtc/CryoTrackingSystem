using CryoTracking.Application.DTOs.Sample;
using CryoTracking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryoTracking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SamplesController : ControllerBase
    {
        private readonly ISampleRepository _repo;

        public SamplesController(ISampleRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repo.GetAllAsync();
            if (!result.Success)
                return NotFound(new { result.Message });
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _repo.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(new { result.Message });
            return Ok(result.Data);
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
        public async Task<IActionResult> Create(SampleCreateDto dto)
        {
            var result = await _repo.CreateAsync(dto);
            if (!result.Success)
                return BadRequest(new { result.Message });
            return CreatedAtAction(nameof(GetById), new { id = result.Data.SampleId }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SampleUpdateDto dto)
        {
            var result = await _repo.UpdateAsync(id, dto);
            if (!result.Success)
                return BadRequest(new { result.Message });
            return NoContent();
        }

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