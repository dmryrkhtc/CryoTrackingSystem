using CryoTracking.Application.DTOs.QualityAssessment;
using CryoTracking.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryoTracking.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class QualityAssessmentsController : ControllerBase
    {
        private readonly IQualityAssessmentRepository _repo;

        public QualityAssessmentsController(IQualityAssessmentRepository repo)
        {
            _repo = repo;
        }
        [Authorize(Roles = "Sistem Yoneticisi,Embriyolog,Doktor")]
        [HttpGet("sample/{sampleId}")]
        public async Task<IActionResult> GetBySample(int sampleId)
        {
            var result = await _repo.GetBySampleIdAsync(sampleId);
            if (!result.Success)
                return NotFound(new { result.Message });
            return Ok(result.Data);
        }
        [Authorize(Roles = "Sistem Yoneticisi,Embriyolog")]
        [HttpPost]
        public async Task<IActionResult> Create(QualityAssessmentCreateDto dto)
        {
            var result = await _repo.CreateAsync(dto);
            if (!result.Success)
                return BadRequest(new { result.Message });
            return Ok(result.Data);
        }
    }
}