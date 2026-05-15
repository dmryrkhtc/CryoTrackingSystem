using CryoTracking.Application.DTOs;
using CryoTracking.Application.DTOs.Consent;
using CryoTracking.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Application.Interfaces
{
    public interface IConsentRepository
    {
        Task<ResultResponse<IEnumerable<ConsentReadDto>>> GetAllAsync();
        Task<ResultResponse<IEnumerable<ConsentReadDto>>> GetByPatientIdAsync(int patientId);
        Task<ResultResponse<ConsentReadDto>> CreateAsync(ConsentCreateDto dto);
    }
}
