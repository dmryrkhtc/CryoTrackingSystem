using CryoTracking.Application.DTOs.QualityAssessment;
using CryoTracking.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CryoTracking.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class QualityAssessmentsController : ControllerBase
    {
        private readonly IQualityAssessmentRepository _repo;
        private readonly IEmbryoAIService _aiService;

        public QualityAssessmentsController(IQualityAssessmentRepository repo, IEmbryoAIService aiService)
        {
            _repo = repo;
            _aiService = aiService;
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
        public async Task<IActionResult> Create([FromForm] QualityAssessmentCreateDto dto, IFormFile embryoImage)
        {
            if (embryoImage == null || embryoImage.Length == 0)
                return BadRequest(new { Message = "Yapay zeka analizi için embriyo görseli yüklemek zorunludur." });

            // 🌟 Sadece ve sadece Hugging Face mikroservisine gidiyoruz
            double? aiScore = await _aiService.GetAIScoreAsync(embryoImage);

            if (aiScore == null)
                return StatusCode(500, new { Message = "Hugging Face model servisinden yanıt alınamadı. Lütfen Space durumunu kontrol edin." });

            // Modelden gelen saf float skoru doğrudan DTO'ya bağlıyoruz
            dto.AIScore = aiScore.Value;

            var result = await _repo.CreateAsync(dto);
            if (!result.Success)
                return BadRequest(new { result.Message });

            return Ok(result.Data);
        }
    }
}