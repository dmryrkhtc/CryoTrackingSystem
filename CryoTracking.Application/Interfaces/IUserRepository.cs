using CryoTracking.Application.DTOs;
using CryoTracking.Application.DTOs.User;
using CryoTracking.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryoTracking.Application.Interfaces
{
   public interface IUserRepository
    {
        Task<ResultResponse<IEnumerable<UserReadDto>>> GetAllAsync();
        Task<ResultResponse<UserReadDto>> GetByIdAsync(int id);
        Task<ResultResponse<UserReadDto>> CreateAsync(UserCreateDto dto);
        Task<ResultResponse<bool>> UpdateAsync(int id, UserUpdateDto dto);
    }
}
