using Microsoft.EntityFrameworkCore;
using CryoTracking.Application.DTOs.User;
using CryoTracking.Application.Interfaces;
using CryoTracking.Domain.Entities;
using CryoTracking.Domain.Response;
using CryoTracking.Infrastructure.Persistence;
using BCrypt.Net;

namespace CryoTracking.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CryoDbContext _context;

        public UserRepository(CryoDbContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<IEnumerable<UserReadDto>>> GetAllAsync()
        {
            try
            {
                var users = await _context.Users
                    .Include(u => u.Role)
                    .ToListAsync();

                if (!users.Any())
                    return new ResultResponse<IEnumerable<UserReadDto>>
                    {
                        Success = false,
                        Message = "Kayitli kullanici bulunamadi."
                    };

                return new ResultResponse<IEnumerable<UserReadDto>>
                {
                    Success = true,
                    Message = "Kullanicilar basariyla listelendi.",
                    Data = users.Select(u => new UserReadDto
                    {
                        UserId = u.UserId,
                        Name = u.Name,
                        Email = u.Email,
                        RoleName = u.Role?.RoleName ?? "",
                        IsActive = u.IsActive
                    })
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse<IEnumerable<UserReadDto>>
                {
                    Success = false,
                    Message = $"Kullanicilar getirilirken hata olustu: {ex.Message}"
                };
            }
        }

        public async Task<ResultResponse<UserReadDto>> GetByIdAsync(int id)
        {
            try
            {
                var u = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.UserId == id);

                if (u == null)
                    return new ResultResponse<UserReadDto>
                    {
                        Success = false,
                        Message = "Kullanici bulunamadi."
                    };

                return new ResultResponse<UserReadDto>
                {
                    Success = true,
                    Message = "Kullanici basariyla bulundu.",
                    Data = new UserReadDto
                    {
                        UserId = u.UserId,
                        Name = u.Name,
                        Email = u.Email,
                        RoleName = u.Role?.RoleName ?? "",
                        IsActive = u.IsActive
                    }
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse<UserReadDto>
                {
                    Success = false,
                    Message = $"Kullanici getirilirken hata olustu: {ex.Message}"
                };
            }
        }
        public async Task<ResultResponse<UserReadDto>> CreateAsync(UserCreateDto dto)
        {
            try
            {
                var emailExists = await _context.Users
                    .AnyAsync(u => u.Email == dto.Email);

                if (emailExists)
                    return new ResultResponse<UserReadDto>
                    {
                        Success = false,
                        Message = "Bu email adresi zaten kayitli."
                    };

                // Şifreyi Hashle 
               
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                var user = new User
                {
                    RoleId = dto.RoleId,
                    Name = dto.Name,
                    Email = dto.Email,
                    Password = hashedPassword, // Artık veritabanına "12345" değil, karmaşık metin gidecek
                    IsActive = true
                };
                

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                var role = await _context.Roles.FindAsync(user.RoleId);

                return new ResultResponse<UserReadDto>
                {
                    Success = true,
                    Message = "Kullanici basariyla olusturuldu.",
                    Data = new UserReadDto
                    {
                        UserId = user.UserId,
                        Name = user.Name,
                        Email = user.Email,
                        RoleName = role?.RoleName ?? "",
                        IsActive = user.IsActive
                    }
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse<UserReadDto>
                {
                    Success = false,
                    Message = $"Kullanici olusturulurken hata olustu: {ex.Message}"
                };
            }
        }

        public async Task<ResultResponse<bool>> UpdateAsync(int id, UserUpdateDto dto)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return new ResultResponse<bool>
                    {
                        Success = false,
                        Message = "Guncellenecek kullanici bulunamadi."
                    };

                var emailExists = await _context.Users
                    .AnyAsync(u => u.Email == dto.Email && u.UserId != id);

                if (emailExists)
                    return new ResultResponse<bool>
                    {
                        Success = false,
                        Message = "Bu email adresi baska bir kullaniciya ait."
                    };

                user.Name = dto.Name;
                user.Email = dto.Email;
                user.RoleId = dto.RoleId;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return new ResultResponse<bool>
                {
                    Success = true,
                    Message = "Kullanici basariyla guncellendi.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse<bool>
                {
                    Success = false,
                    Message = $"Kullanici guncellenirken hata olustu: {ex.Message}"
                };
            }
        }
    }
}