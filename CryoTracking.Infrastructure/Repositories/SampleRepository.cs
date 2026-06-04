using CryoTracking.Application.DTOs.Sample;
using CryoTracking.Application.Interfaces;
using CryoTracking.Domain.Entities;
using CryoTracking.Domain.Enums;
using CryoTracking.Domain.Response;
using CryoTracking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Infrastructure.Repositories
{
    public class SampleRepository : ISampleRepository
    {
        private readonly CryoDbContext _context;

        public SampleRepository(CryoDbContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<IEnumerable<SampleReadDto>>> GetAllAsync()
        {
            try
            {
                var now = DateTime.UtcNow;

            
                var samples = await _context.Samples
                    .Include(s => s.Patient)
                    .Include(s => s.QualityAssessments)
                    .AsNoTracking()
                    .Select(s => new SampleReadDto
                    {
                        SampleId = s.SampleId,
                        PatientId = s.PatientId,
                        PatientName = s.Patient.FullName,
                        SampleType = s.SampleType,
                        FreezeDate = s.FreezeDate,
                        ThawDate = s.ThawDate,
                        Status = s.Status,
                        Notes = s.Notes,

                        QualityAssessments = s.QualityAssessments.Select(q => new CryoTracking.Application.DTOs.QualityAssessment.QualityAssessmentReadDto
                        {
                          
                            SampleId = q.SampleId,
                            MorphologyGrade = q.MorphologyGrade,
                            EmbryologistNote = q.EmbryologistNote,
                            AIScore = q.AIScore
                        }).ToList(),

                        // 5 yıllık süre kontrolü
                        IsWarning = s.Status == StatusType.Frozen && (now - s.FreezeDate).TotalDays > 1700,
                        IsExpired = s.Status == StatusType.Frozen && (now - s.FreezeDate).TotalDays > 1825
                    })
                    .ToListAsync();

                return new ResultResponse<IEnumerable<SampleReadDto>> { Success = true, Data = samples };
            }
            catch (Exception ex)
            {
                return new ResultResponse<IEnumerable<SampleReadDto>> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ResultResponse<SampleReadDto>> GetByIdAsync(int id)
        {
            try
            {
                var s = await _context.Samples
                    .Include(s => s.Patient)
                    .Include(s => s.QualityAssessments)
                    .FirstOrDefaultAsync(s => s.SampleId == id);

                if (s == null)
                    return new ResultResponse<SampleReadDto> { Success = false, Message = "Ornek bulunamadi." };

                return new ResultResponse<SampleReadDto>
                {
                    Success = true,
                    Message = "Ornek basariyla bulundu.",
                    Data = new SampleReadDto
                    {
                        SampleId = s.SampleId,
                        PatientId = s.PatientId,
                        PatientName = s.Patient?.FullName ?? "",
                        SampleType = s.SampleType,
                        FreezeDate = s.FreezeDate,
                        ThawDate = s.ThawDate,
                        Status = s.Status,
                        Notes = s.Notes,
                        QualityAssessments = s.QualityAssessments.Select(q => new CryoTracking.Application.DTOs.QualityAssessment.QualityAssessmentReadDto
                        {
                           
                            SampleId = q.SampleId,
                            MorphologyGrade = q.MorphologyGrade,
                            EmbryologistNote = q.EmbryologistNote,
                            AIScore = q.AIScore
                        }).ToList()
                    }
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse<SampleReadDto> { Success = false, Message = $"Ornek getirilirken hata olustu: {ex.Message}" };
            }
        }

        public async Task<ResultResponse<IEnumerable<SampleReadDto>>> GetByPatientIdAsync(int patientId)
        {
            try
            {
                var samples = await _context.Samples
                    .Include(s => s.Patient)
                    .Include(s => s.QualityAssessments) // Hasta bazlı aramada da dahil ettik
                    .Where(s => s.PatientId == patientId)
                    .ToListAsync();

                if (!samples.Any())
                    return new ResultResponse<IEnumerable<SampleReadDto>> { Success = false, Message = "Bu hastaya ait ornek bulunamadi." };

                return new ResultResponse<IEnumerable<SampleReadDto>>
                {
                    Success = true,
                    Message = "Ornekler basariyla listelendi.",
                    Data = samples.Select(s => new SampleReadDto
                    {
                        SampleId = s.SampleId,
                        PatientId = s.PatientId,
                        PatientName = s.Patient?.FullName ?? "",
                        SampleType = s.SampleType,
                        FreezeDate = s.FreezeDate,
                        ThawDate = s.ThawDate,
                        Status = s.Status,
                        Notes = s.Notes,
                        QualityAssessments = s.QualityAssessments.Select(q => new CryoTracking.Application.DTOs.QualityAssessment.QualityAssessmentReadDto
                        {
                       
                            SampleId = q.SampleId,
                            MorphologyGrade = q.MorphologyGrade,
                            EmbryologistNote = q.EmbryologistNote,
                            AIScore = q.AIScore
                        }).ToList()
                    })
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse<IEnumerable<SampleReadDto>> { Success = false, Message = $"Ornekler getirilirken hata olustu: {ex.Message}" };
            }
        }

        public async Task<ResultResponse<SampleReadDto>> CreateAsync(SampleCreateDto dto)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(dto.PatientId);
                if (patient == null)
                    return new ResultResponse<SampleReadDto> { Success = false, Message = "Hasta bulunamadı." };

                // Cinsiyet enum kontrolü (Female = 1 durumuna göre esnetildi)
                if ((int)dto.SampleType == (int)SampleType.Embryo && (int)patient.Gender != 1)
                {
                    return new ResultResponse<SampleReadDto>
                    {
                        Success = false,
                        Message = "HATA: Embriyo kaydı yasal olarak sadece kadın hastalar üzerine açılabilir!"
                    };
                }

                var sample = new Sample
                {
                    PatientId = dto.PatientId,
                    SampleType = dto.SampleType,
                    FreezeDate = dto.FreezeDate,
                    Status = dto.Status,
                    CreatedBy = dto.CreatedBy,
                    Notes = dto.Notes,
                    IsActive = true
                };

                await _context.Samples.AddAsync(sample);
                await _context.SaveChangesAsync();

                return new ResultResponse<SampleReadDto>
                {
                    Success = true,
                    Message = "Ornek basariyla eklendi.",
                    Data = new SampleReadDto
                    {
                        SampleId = sample.SampleId,
                        PatientId = sample.PatientId,
                        PatientName = patient.FullName,
                        SampleType = sample.SampleType,
                        FreezeDate = sample.FreezeDate,
                        Status = sample.Status,
                        Notes = sample.Notes
                    }
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse<SampleReadDto> { Success = false, Message = $"Ornek eklenirken hata: {ex.Message}" };
            }
        }

        public async Task<ResultResponse<bool>> UpdateAsync(int id, SampleUpdateDto dto)
        {
            try
            {
                var sample = await _context.Samples.FindAsync(id);
                if (sample == null)
                    return new ResultResponse<bool> { Success = false, Message = "Guncellenecek ornek bulunamadi." };

                if (sample.Status == StatusType.LegalHold)
                    return new ResultResponse<bool> { Success = false, Message = "DİKKAT: Yasal kilit (LegalHold) altındaki numuneler üzerinde işlem yapılamaz!" };

                if (sample.Status == StatusType.Thawed)
                    return new ResultResponse<bool> { Success = false, Message = "Çözülmüş bir numune üzerinde bu işlem yapılamaz!" };

                sample.Status = dto.Status;
                sample.ThawDate = dto.ThawDate;
                sample.Notes = dto.Notes;

                _context.Samples.Update(sample);
                await _context.SaveChangesAsync();

                return new ResultResponse<bool> { Success = true, Message = "Ornek basariyla guncellendi.", Data = true };
            }
            catch (Exception ex) { return new ResultResponse<bool> { Success = false, Message = $"Ornek guncellenirken hata olustu: {ex.Message}" }; }
        }

        public async Task<ResultResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var sample = await _context.Samples.FindAsync(id);
                if (sample == null)
                    return new ResultResponse<bool> { Success = false, Message = "Silinecek ornek bulunamadi." };

                if (sample.Status == StatusType.LegalHold)
                    return new ResultResponse<bool> { Success = false, Message = "Yasal kilit altındaki numune silinemez!" };

                _context.Samples.Remove(sample);
                await _context.SaveChangesAsync();

                return new ResultResponse<bool> { Success = true, Message = "Ornek basariyla silindi.", Data = true };
            }
            catch (Exception ex) { return new ResultResponse<bool> { Success = false, Message = $"Ornek silinirken hata olustu: {ex.Message}" }; }
        }
    }
}