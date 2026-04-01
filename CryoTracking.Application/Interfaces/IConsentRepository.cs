using CryoTracking.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Application.Interfaces
{
    public interface IConsentRepository
    {
        Task<IEnumerable<ConsentResponseDto>> GetByPatientIdAsync(int patientId);
        Task<ConsentResponseDto> CreateAsync(ConsentRequestDto dto);
    }
}
