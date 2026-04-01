using CryoTracking.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Application.Interfaces
{
    public interface ISampleRepository
    {
        Task<IEnumerable<SampleResponseDto>> GetAllAsync();
        Task<SampleResponseDto?> GetByIdAsync(int id);
        Task<IEnumerable<SampleResponseDto>> GetByPatientIdAsync(int patientId);
        Task<SampleResponseDto> CreateAsync(SampleRequestDto dto);
        Task UpdateAsync(int id, SampleRequestDto dto);
        Task DeleteAsync(int id);
    }
}
