using Microsoft.EntityFrameworkCore;
using CryoTracking.Application.DTOs.Patient;
using CryoTracking.Application.Interfaces;
using CryoTracking.Domain.Entities;
using CryoTracking.Domain.Response;
using CryoTracking.Infrastructure.Persistence;
using CryoTracking.Domain.Enums;

namespace CryoTracking.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly CryoDbContext _context;

        public PatientRepository(CryoDbContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<IEnumerable<PatientReadDto>>> GetAllAsync()
        {
            try
            {
                var patients = await _context.Patients
                    .AsNoTracking()
                    .Where(p => p.IsActive)
                    .Select(p => new PatientReadDto
                    {
                        PatientId = p.PatientId,
                        FullName = p.FullName,
                        TCNo = p.TCNo,
                        DateOfBirth = p.DateOfBirth,
                        Gender = p.Gender,
                        CoupleType = p.CoupleType,
                        ContactInfo = p.ContactInfo,
                        MaritalStatus = p.MaritalStatus,
                        CreatedAt = p.CreatedAt
                    })
                    .ToListAsync();

                return new ResultResponse<IEnumerable<PatientReadDto>> { Success = true, Message = "Hastalar listelendi.", Data = patients };
            }
            catch (Exception ex)
            {
                return new ResultResponse<IEnumerable<PatientReadDto>> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ResultResponse<PatientReadDto>> GetByIdAsync(int id)
        {
            try
            {
                var p = await _context.Patients
                    .FirstOrDefaultAsync(p => p.PatientId == id && p.IsActive);

                if (p == null)
                    return new ResultResponse<PatientReadDto> { Success = false, Message = "Hasta bulunamadı." };

                return new ResultResponse<PatientReadDto>
                {
                    Success = true,
                    Data = new PatientReadDto
                    {
                        PatientId = p.PatientId,
                        FullName = p.FullName,
                        TCNo = p.TCNo,
                        DateOfBirth = p.DateOfBirth,
                        Gender = p.Gender,
                        CoupleType = p.CoupleType,
                        ContactInfo = p.ContactInfo,
                        MaritalStatus = p.MaritalStatus,
                        CreatedAt = p.CreatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse<PatientReadDto> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ResultResponse<PatientReadDto>> CreateAsync(PatientCreateDto dto)
        {
            try
            {
                var patient = new Patient
                {
                    FullName = dto.FullName,
                    TCNo = dto.TCNo,
                    DateOfBirth = dto.DateOfBirth,
                    Gender = dto.Gender,
                    CoupleType = dto.CoupleType,
                    ContactInfo = dto.ContactInfo,
                    MaritalStatus = dto.MaritalStatus,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                await _context.Patients.AddAsync(patient);
                await _context.SaveChangesAsync();

                return await GetByIdAsync(patient.PatientId);
            }
            catch (Exception ex)
            {
                return new ResultResponse<PatientReadDto> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ResultResponse<bool>> UpdateAsync(int id, PatientUpdateDto dto)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(id);
                if (patient == null || !patient.IsActive)
                    return new ResultResponse<bool> { Success = false, Message = "Hasta bulunamadı." };

                // İş Mantığı: Boşanma durumunda otomatik kilit (Enum bazlı kontrol)
                if (patient.MaritalStatus == MaritalStatusType.Married && dto.MaritalStatus == MaritalStatusType.Divorced)
                {
                    var samples = await _context.Samples
                        .Where(s => s.PatientId == id && s.SampleType == SampleType.Embryo)
                        .ToListAsync();

                    foreach (var s in samples)
                    {
                        s.Status = StatusType.LegalHold;
                        s.Notes += " [SİSTEM: Boşanma nedeniyle kilitlendi.]";
                    }
                }

                patient.FullName = dto.FullName;
                patient.ContactInfo = dto.ContactInfo;
                patient.CoupleType = dto.CoupleType;
                patient.MaritalStatus = dto.MaritalStatus;

                _context.Patients.Update(patient);
                await _context.SaveChangesAsync();

                return new ResultResponse<bool> { Success = true, Data = true };
            }
            catch (Exception ex)
            {
                return new ResultResponse<bool> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ResultResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(id);
                if (patient == null) return new ResultResponse<bool> { Success = false, Message = "Hasta bulunamadı." };

                patient.IsActive = false;

                var samples = await _context.Samples.Where(s => s.PatientId == id).ToListAsync();
                foreach (var s in samples) s.IsActive = false;

                await _context.SaveChangesAsync();
                return new ResultResponse<bool> { Success = true, Data = true };
            }
            catch (Exception ex)
            {
                return new ResultResponse<bool> { Success = false, Message = ex.Message };
            }
        }
    }
}