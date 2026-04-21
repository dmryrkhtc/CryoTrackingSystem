using CryoTracking.Application.DTOs;
using CryoTracking.Application.DTOs.Sample;
using CryoTracking.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Application.Interfaces
{
    public interface ISampleRepository
    {
        Task<ResultResponse<IEnumerable<SampleReadDto>>> GetAllAsync();
        Task<ResultResponse<SampleReadDto>> GetByIdAsync(int id);
        Task<ResultResponse<IEnumerable<SampleReadDto>>> GetByPatientIdAsync(int patientId);
        Task<ResultResponse<SampleReadDto>> CreateAsync(SampleCreateDto dto);
        Task<ResultResponse<bool>> UpdateAsync(int id, SampleUpdateDto dto);
        Task<ResultResponse<bool>> DeleteAsync(int id);
    }
}
