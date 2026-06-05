using CryoTracking.Application.DTOs.QualityAssessment;
using CryoTracking.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Globalization;
using System;

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
            // Fotoğraf yüklenmiş mi kontrolü
            if (embryoImage == null || embryoImage.Length == 0)
                return BadRequest(new { Message = "Yapay zeka analizi için embriyo görseli yüklemek zorunludur." });

            double? aiScore = null;

            // 🌟 SWAGGER VEYA ÖNYÜZDEN ELLE GİRİLEN DEĞER VAR MI KONTROLÜ (MOCK/TEST İÇİN):
            // Eğer form verisinden manuel bir AIScore gönderildiyse dil azizliğine uğramadan parse edelim
            if (Request.Form.TryGetValue("AIScore", out var incomingScore) && !string.IsNullOrEmpty(incomingScore))
            {
                string scoreStr = incomingScore.ToString().Replace(',', '.').Trim();
                if (double.TryParse(scoreStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double parsedManualScore))
                {
                    aiScore = parsedManualScore;
                }
            }

            // Eğer dışarıdan el ile bir skor simüle edilmediyse, Hugging Face yapay zeka servisine git
            if (aiScore == null)
            {
                aiScore = await _aiService.GetAIScoreAsync(embryoImage);
            }

            if (aiScore == null)
                return StatusCode(500, new { Message = "Yapay zeka analiz servisine ulaşılamadı veya geçerli bir skor üretilemedi." });

            // 🌟 KESİN BAĞLANTI SİGORTASI: 
            // Modelden veya formdan gelen saf double skoru evrensel kültüre göre normalize edip DTO'ya basıyoruz
            string secureFormated = aiScore.Value.ToString(CultureInfo.InvariantCulture);
            dto.AIScore = double.Parse(secureFormated, CultureInfo.InvariantCulture);

            // Repo üzerinden veritabanına kayıt işlemi fırlatılıyor
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