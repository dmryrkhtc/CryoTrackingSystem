using Microsoft.EntityFrameworkCore;
using CryoTracking.Application.DTOs.QualityAssessment;
using CryoTracking.Application.Interfaces;
using CryoTracking.Domain.Entities;
using CryoTracking.Domain.Response;
using CryoTracking.Infrastructure.Persistence;

namespace CryoTracking.Infrastructure.Repositories
{
    public class QualityAssessmentRepository : IQualityAssessmentRepository
    {
        private readonly CryoDbContext _context;

        public QualityAssessmentRepository(CryoDbContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<IEnumerable<QualityAssessmentReadDto>>> GetBySampleIdAsync(int sampleId)
        {
            try
            {
                var list = await _context.QualityAssessments
                    .Where(q => q.SampleId == sampleId)
                    .ToListAsync();

                if (!list.Any())
                    return new ResultResponse<IEnumerable<QualityAssessmentReadDto>>
                    {
                        Success = false,
                        Message = "Bu ornege ait kalite degerlendirmesi bulunamadi."
                    };

                return new ResultResponse<IEnumerable<QualityAssessmentReadDto>>
                {
                    Success = true,
                    Message = "Kalite degerlendirmeleri basariyla listelendi.",
                    Data = list.Select(q => new QualityAssessmentReadDto
                    {
                        QAId = q.QAId,
                        SampleId = q.SampleId,
                        MorphologyGrade = q.MorphologyGrade,
                        EmbryologistNote = q.EmbryologistNote,
                        AIScore = q.AIScore,
                        ScoredAt = q.ScoredAt
                    })
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse<IEnumerable<QualityAssessmentReadDto>>
                {
                    Success = false,
                    Message = $"Kalite degerlendirmeleri getirilirken hata olustu: {ex.Message}"
                };
            }
        }

        public async Task<ResultResponse<QualityAssessmentReadDto>> CreateAsync(QualityAssessmentCreateDto dto)
        {
            try
            {
                var qa = new QualityAssessment
                {
                    SampleId = dto.SampleId,
                    MorphologyGrade = dto.MorphologyGrade,
                    EmbryologistNote = dto.EmbryologistNote,
                    AIScore = dto.AIScore,
                    ScoredAt = DateTime.UtcNow
                };

                await _context.QualityAssessments.AddAsync(qa);
                await _context.SaveChangesAsync();

                return new ResultResponse<QualityAssessmentReadDto>
                {
                    Success = true,
                    Message = "Kalite degerlendirmesi basariyla eklendi.",
                    Data = new QualityAssessmentReadDto
                    {
                        QAId = qa.QAId,
                        SampleId = qa.SampleId,
                        MorphologyGrade = qa.MorphologyGrade,
                        EmbryologistNote = qa.EmbryologistNote,
                        AIScore = qa.AIScore,
                        ScoredAt = qa.ScoredAt
                    }
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse<QualityAssessmentReadDto>
                {
                    Success = false,
                    Message = $"Kalite degerlendirmesi eklenirken hata olustu: {ex.Message}"
                };
            }
        }
    }
}