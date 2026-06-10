using Microsoft.EntityFrameworkCore;
using CryoTracking.Application.DTOs.QualityAssessment;
using CryoTracking.Application.Interfaces;
using CryoTracking.Domain.Entities;
using CryoTracking.Domain.Response;
using CryoTracking.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryoTracking.Infrastructure.Repositories
{
    public class QualityAssessmentRepository : IQualityAssessmentRepository
    {
        private readonly CryoDbContext _context;

        public QualityAssessmentRepository(CryoDbContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<IEnumerable<QualityAssessmentReadDto>>> GetAllAsync()
        {
            try
            {
                var list = await _context.QualityAssessments
                    .Include(q => q.Sample)
                    .OrderByDescending(q => q.ScoredAt)
                    .ToListAsync();

                return new ResultResponse<IEnumerable<QualityAssessmentReadDto>>
                {
                    Success = true,
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
                    Message = ex.Message
                };
            }
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
                // 🌟 SIFIR MANİPÜLASYON: Model ne ürettiyse veritabanına o yazılır.
                double finalScore = dto.AIScore ?? 0.0;

                var qa = new QualityAssessment
                {
                    SampleId = dto.SampleId,
                    MorphologyGrade = dto.MorphologyGrade,
                    EmbryologistNote = dto.EmbryologistNote,
                    AIScore = finalScore, // Modelin Hugging Face'ten dönen özgün, saf skoru!
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
                        EmbryologistNote = qa.EmbryologistNote,
                        AIScore = qa.AIScore,
                        MorphologyGrade = qa.MorphologyGrade,
                        ScoredAt = qa.ScoredAt
                    }
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse<QualityAssessmentReadDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}