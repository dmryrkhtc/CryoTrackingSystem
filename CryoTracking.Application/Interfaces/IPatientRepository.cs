using CryoTracking.Domain.Entities;
namespace CryoTracking.Application.Interfaces
{
    public interface IPatientRepository
    {
        //TUM HASTALARI GETIRMEK ICIN ASYNC METOD
        Task<IEnumerable<Patient>> GetAllAsync();
        //ID'YE GORE TEK HASTA GETIRMEK ICIN ASYNC METOD BULAMAZSA NULL DONER
        Task<Patient?> GetByIdAsync(int id);
        //YENI HASTA EKLEMEK ICIN ASYNC METOD
        Task AddAsync(Patient patient);
        //VAR OLAN HASTA BILGILERINI GUNCELLEMEK ICIN ASYNC METOD
        Task UpdateAsync(Patient patient);
        //ID'YE GORE HASTA SILMEK ICIN ASYNC METOD
        Task DeleteAsync(int id);
    }
}
