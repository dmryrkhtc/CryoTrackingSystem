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
                    .Where(p => p.IsActive==true) // Sadece aktif olanları getir
                    .Select(p => new PatientReadDto
                    {
                        PatientId = p.PatientId,
                        FullName = p.FullName,
                        TCNo = p.TCNo,
                        DateOfBirth = p.DateOfBirth,
                        Gender = p.Gender, // Enum'ı string'e çeviriyoruz
                        CoupleType = p.CoupleType,
                        ContactInfo = p.ContactInfo,
                        MaritalStatus = p.MaritalStatus,
                        CreatedAt = p.CreatedAt
                    })
                    .ToListAsync();

                if (!patients.Any())
                    return new ResultResponse<IEnumerable<PatientReadDto>> { Success = false, Message = "Kayıtlı hasta bulunamadı." };

                return new ResultResponse<IEnumerable<PatientReadDto>> { Success = true, Message = "Hastalar başarıyla listelendi.", Data = patients };
            }
            catch (Exception ex)
            {
                return new ResultResponse<IEnumerable<PatientReadDto>> { Success = false, Message = $"Hata: {ex.Message}" };
            }
        }
        public async Task<ResultResponse<PatientReadDto>> GetByIdAsync(int id)
        {
            try
            {
                var p = await _context.Patients.FindAsync(id);
                if (p == null)
                    return new ResultResponse<PatientReadDto>
                    {
                        Success = false,
                        Message = "Hasta bulunamadi."
                    };

                return new ResultResponse<PatientReadDto>
                {
                    Success = true,
                    Message = "Hasta basariyla bulundu.",
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
                return new ResultResponse<PatientReadDto>
                {
                    Success = false,
                    Message = $"Hasta getirilirken hata olustu: {ex.Message}"
                };
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
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Patients.AddAsync(patient);
                await _context.SaveChangesAsync();

                return new ResultResponse<PatientReadDto>
                {
                    Success = true,
                    Message = "Hasta basariyla eklendi.",
                    Data = new PatientReadDto
                    {
                        PatientId = patient.PatientId,
                        FullName = patient.FullName,
                        TCNo = patient.TCNo,
                        DateOfBirth = patient.DateOfBirth,
                        Gender = patient.Gender,
                        CoupleType = patient.CoupleType,
                        ContactInfo = patient.ContactInfo,
                        MaritalStatus = patient.MaritalStatus,
                        CreatedAt = patient.CreatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse<PatientReadDto>
                {
                    Success = false,
                    Message = $"Hasta eklenirken hata olustu: {ex.Message}"
                };
            }
        }

        public async Task<ResultResponse<bool>> UpdateAsync(int id, PatientUpdateDto dto)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(id);
                if (patient == null)
                    return new ResultResponse<bool>
                    {
                        Success = false,
                        Message = "Guncellenecek hasta bulunamadi."
                    };

                patient.FullName = dto.FullName;
                patient.ContactInfo = dto.ContactInfo;
                patient.CoupleType = dto.CoupleType;
                patient.MaritalStatus = dto.MaritalStatus;

                _context.Patients.Update(patient);
                await _context.SaveChangesAsync();

                return new ResultResponse<bool>
                {
                    Success = true,
                    Message = "Hasta basariyla guncellendi.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse<bool>
                {
                    Success = false,
                    Message = $"Hasta guncellenirken hata olustu: {ex.Message}"
                };
            }
        }

        public async Task<ResultResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var patient = await _context.Patients.FindAsync(id);
                if (patient == null)
                    return new ResultResponse<bool>
                    {
                        Success = false,
                        Message = "Silinecek hasta bulunamadi."
                    };
                // Fiziksel silme yerine ARŞİVLEME yapıyoruz
                patient.IsActive = false;
                // Opsiyonel: Hastayı pasife alınca numunelerini de pasife çekebiliriz
                var samples = await _context.Samples.Where(s => s.PatientId == id).ToListAsync();
                foreach (var sample in samples) { sample.IsActive = false; }

                _context.Patients.Update(patient);
                await _context.SaveChangesAsync();

                return new ResultResponse<bool>
                {
                    Success = true,
                    Message = "Hasta basariyla silindi.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse<bool>
                {
                    Success = false,
                    Message = $"Hasta silinirken hata olustu: {ex.Message}"
                };
            }
        }
    }
}