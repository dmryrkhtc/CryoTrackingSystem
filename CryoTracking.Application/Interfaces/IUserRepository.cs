using CryoTracking.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Application.Interfaces
{
   public interface IUserRepository
    {
        Task<IEnumerable<UserResponseDto>> GetAllAsync();
        Task<UserResponseDto?> GetByIdAsync(int id);
        Task<UserResponseDto> CreateAsync(UserRequestDto dto);
        Task UpdateAsync(int id, UserRequestDto dto);
    }
}
