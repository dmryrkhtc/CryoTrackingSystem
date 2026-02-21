using CryoTracking.Domain.Entities;
namespace CryoTracking.Application.Interfaces
{
    public class IPatientRepository
    {
        //TUM HASTALARI GETIRMEK ICIN ASYNC METOD
        Task<IEnumerable<Patient>> GetAllPatientsAsync();
        //ID'YE GORE TEK HASTA GETIRMEK ICIN ASYNC METOD BULAMAZSA NULL DONER
        Task<Patient?> GetPatientByIdAsync(int id);
        //YENI HASTA EKLEMEK ICIN ASYNC METOD
        Task AddPatientAsync(Patient patient);
        //VAR OLAN HASTA BILGILERINI GUNCELLEMEK ICIN ASYNC METOD
        Task UpdatePatientAsync(Patient patient);
        //ID'YE GORE HASTA SILMEK ICIN ASYNC METOD
        Task DeletePatientAsync(int id);
    }
}
