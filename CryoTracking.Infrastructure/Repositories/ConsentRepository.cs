using Microsoft.EntityFrameworkCore;
using CryoTracking.Application.DTOs.Consent;
using CryoTracking.Application.Interfaces;
using CryoTracking.Domain.Entities;
using CryoTracking.Domain.Response;
using CryoTracking.Infrastructure.Persistence;

namespace CryoTracking.Infrastructure.Repositories
{
    public class ConsentRepository : IConsentRepository
    {
        private readonly CryoDbContext _context;

        public ConsentRepository(CryoDbContext context)
        {
            _context = context;
        }
        public async Task<ResultResponse<IEnumerable<ConsentReadDto>>> GetAllAsync()
        {
            try
            {
                var consents = await _context.Consents
                    .Include(c => c.Patient)
                    .ToListAsync();

                return new ResultResponse<IEnumerable<ConsentReadDto>>
                {
                    Success = true,
                    Data = consents.Select(c => new ConsentReadDto
                    {
                        ConsentId = c.ConsentId,
                        PatientId = c.PatientId,
                        PatientName = c.Patient?.FullName ?? "",
                        ConsentType = c.ConsentType,
                        FilePath = c.FilePath,
                        SignedAt = c.SignedAt
                    })
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse<IEnumerable<ConsentReadDto>> { Success = false, Message = ex.Message };
            }
        }
        public async Task<ResultResponse<IEnumerable<ConsentReadDto>>> GetByPatientIdAsync(int patientId)
        {
            try
            {
                var consents = await _context.Consents
                    .Include(c => c.Patient)
                    .Where(c => c.PatientId == patientId)
                    .ToListAsync();

                if (!consents.Any())
                    return new ResultResponse<IEnumerable<ConsentReadDto>>
                    {
                        Success = false,
                        Message = "Bu hastaya ait onay belgesi bulunamadi."
                    };

                return new ResultResponse<IEnumerable<ConsentReadDto>>
                {
                    Success = true,
                    Message = "Onay belgeleri basariyla listelendi.",
                    Data = consents.Select(c => new ConsentReadDto
                    {
                        ConsentId = c.ConsentId,
                        PatientId = c.PatientId,
                        PatientName = c.Patient?.FullName ?? "",
                        ConsentType = c.ConsentType,
                        FilePath = c.FilePath,
                        SignedAt = c.SignedAt
                    })
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse<IEnumerable<ConsentReadDto>>
                {
                    Success = false,
                    Message = $"Onay belgeleri getirilirken hata olustu: {ex.Message}"
                };
            }
        }

        public async Task<ResultResponse<ConsentReadDto>> CreateAsync(ConsentCreateDto dto)
        {
            try
            {
                var consent = new Consent
                {
                    PatientId = dto.PatientId,
                    ConsentType = dto.ConsentType,
                    FilePath = dto.FilePath,
                    SignedAt = dto.SignedAt
                };

                await _context.Consents.AddAsync(consent);
                await _context.SaveChangesAsync();

                var patient = await _context.Patients.FindAsync(consent.PatientId);

                return new ResultResponse<ConsentReadDto>
                {
                    Success = true,
                    Message = "Onay belgesi basariyla eklendi.",
                    Data = new ConsentReadDto
                    {
                        ConsentId = consent.ConsentId,
                        PatientId = consent.PatientId,
                        PatientName = patient?.FullName ?? "",
                        ConsentType = consent.ConsentType,
                        FilePath = consent.FilePath,
                        SignedAt = consent.SignedAt
                    }
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse<ConsentReadDto>
                {
                    Success = false,
                    Message = $"Onay belgesi eklenirken hata olustu: {ex.Message}"
                };
            }
        }
    }
}