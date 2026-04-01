using CryoTracking.Application.DTOs;
using CryoTracking.Domain.Entities;
namespace CryoTracking.Application.Interfaces
{
    public interface IPatientRepository
    {
        //TUM HASTALARI GETIRMEK ICIN ASYNC METOD
        Task<IEnumerable<PatientResponseDto>> GetAllAsync();
        //ID'YE GORE TEK HASTA GETIRMEK ICIN ASYNC METOD BULAMAZSA NULL DONER
        Task<PatientResponseDto?> GetByIdAsync(int id);
        //YENI HASTA EKLEMEK ICIN ASYNC METOD
        Task<PatientResponseDto> CreateAsync(PatientRequestDto dto);
        //VAR OLAN HASTA BILGILERINI GUNCELLEMEK ICIN ASYNC METOD
        Task UpdateAsync(int id,PatientRequestDto dto);
        //ID'YE GORE HASTA SILMEK ICIN ASYNC METOD
        Task DeleteAsync(int id);
    }
}
