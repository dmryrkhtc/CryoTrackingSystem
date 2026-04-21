using CryoTracking.Application.DTOs;
using CryoTracking.Application.DTOs.Patient;
using CryoTracking.Domain.Entities;
using CryoTracking.Domain.Response;
namespace CryoTracking.Application.Interfaces
{
    public interface IPatientRepository
    {
        //TUM HASTALARI GETIRMEK ICIN ASYNC METOD
        Task<ResultResponse<IEnumerable<PatientReadDto>>> GetAllAsync();    
        //ID'YE GORE TEK HASTA GETIRMEK ICIN ASYNC METOD BULAMAZSA NULL DONER
        Task<ResultResponse<PatientReadDto>> GetByIdAsync(int id);
        //YENI HASTA EKLEMEK ICIN ASYNC METOD
        Task<ResultResponse<PatientReadDto>> CreateAsync(PatientCreateDto dto);
        //VAR OLAN HASTA BILGILERINI GUNCELLEMEK ICIN ASYNC METOD
        Task<ResultResponse<bool>> UpdateAsync(int id, PatientUpdateDto dto);  
        //ID'YE GORE HASTA SILMEK ICIN ASYNC METOD
        Task<ResultResponse<bool>> DeleteAsync(int id);
    }
}
