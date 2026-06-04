using CryoTracking.Application.DTOs.QualityAssessment;
using CryoTracking.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CryoTracking.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class QualityAssessmentsController : ControllerBase
    {
        private readonly IQualityAssessmentRepository _repo;
        // 1. Yeni eklediğimiz AI Servis arayüzünü tanımlıyoruz
        private readonly IEmbryoAIService _aiService;

        // Constructor'a AI servisimizi enjekte ediyoruz
        public QualityAssessmentsController(IQualityAssessmentRepository repo, IEmbryoAIService aiService)
        {
            _repo = repo;
            _aiService = aiService;
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

        // 2. DEĞİŞEN KISIM BURASI: Metodu [FromForm] yaptık ve IFormFile ekledik
        [Authorize(Roles = "Sistem Yoneticisi,Embriyolog")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] QualityAssessmentCreateDto dto, IFormFile embryoImage)
        {
            // Fotoğraf yüklenmiş mi kontrolü yapıyoruz
            if (embryoImage == null || embryoImage.Length == 0)
                return BadRequest(new { Message = "Yapay zeka analizi için embriyo görseli yüklemek zorunludur." });

            // 3. Resmi Hugging Face'teki yapay zekaya gönderip skoru alıyoruz
            double? aiScore = await _aiService.GetAIScoreAsync(embryoImage);

            if (aiScore == null)
                return StatusCode(500, new { Message = "Yapay zeka analiz servisine ulaşılamadı veya bir hata oluştu." });

            // 4. Gelen skoru DTO içerisindeki AIScore alanına otomatik yazıyoruz
            dto.AIScore = aiScore;

            // 5. Senin mevcut kayıt fonksiyonun: DTO artık AI skoru dolu şekilde DB'ye gidiyor
            var result = await _repo.CreateAsync(dto);
            if (!result.Success)
                return BadRequest(new { result.Message });

            return Ok(result.Data);
        }

        [Authorize(Roles = "Sistem Yoneticisi,Embriyolog,Doktor")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repo.GetAllAsync();
            if (!result.Success) return NotFound(new { result.Message });
            return Ok(result.Data);
        }
    }
}