using CryoTracking.Application.DTOs;
using CryoTracking.Application.DTOs.QualityAssessment;
using CryoTracking.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Application.Interfaces
{
    public interface IQualityAssessmentRepository
    {

        Task<ResultResponse<IEnumerable<QualityAssessmentReadDto>>> GetBySampleIdAsync(int sampleId);
        Task<ResultResponse<QualityAssessmentReadDto>> CreateAsync(QualityAssessmentCreateDto dto);
    }
}
