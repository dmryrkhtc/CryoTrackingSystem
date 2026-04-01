using CryoTracking.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Application.Interfaces
{
    public interface IQualityAssessmentRepository
    {
        Task<IEnumerable<QualityAssessmentResponseDto>> GetBySampleIdAsync(int sampleId);
        Task<QualityAssessmentResponseDto> CreateAsync(QualityAssessmentRequestDto dto);
    }
}
