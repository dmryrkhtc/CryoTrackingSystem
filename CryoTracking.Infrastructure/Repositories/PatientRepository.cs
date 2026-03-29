using Microsoft.EntityFrameworkCore;
using CryoTracking.Domain.Entities;
using CryoTracking.Application.Interfaces;
using CryoTracking.Infrastructure.Persistence;

namespace CryoTracking.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly CryoDbContext _context;
        public PatientRepository(CryoDbContext context)
        {
            _context = context;
        }
        //TUM HASTALARI GETIRMEK ICIN ASYNC METOD
        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await _context.Patients.ToListAsync();
        }
        //ID'YE GORE TEK HASTA GETIRMEK ICIN ASYNC METOD BULAMAZSA NULL DONER
        public async Task<Patient?> GetByIdAsync(int id)
        {
            return await _context.Patients.FindAsync(id);
        }
        //YENI HASTA EKLEMEK ICIN ASYNC METOD
        public async Task AddAsync(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
        }
        //VAR OLAN HASTA BILGILERINI GUNCELLEMEK ICIN ASYNC METOD
        public async Task UpdateAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }
        //ID'YE GORE HASTA SILMEK ICIN ASYNC METOD
        public async Task DeleteAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
            }
        }

    }
}
