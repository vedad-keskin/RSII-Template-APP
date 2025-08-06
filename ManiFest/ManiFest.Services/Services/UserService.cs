using ManiFest.Services.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Security.Cryptography;
using ManiFest.Model.Responses;
using ManiFest.Model.SearchObjects;
using ManiFest.Model.Requests;
using ManiFest.Services.Interfaces;
using MapsterMapper;

namespace ManiFest.Services.Services
{
    public class UserService : BaseService<UserResponse, UserSearchObject, User>, IUserService
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 10000;

        public UserService(ManiFestDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override async Task<PagedResult<UserResponse>> GetAsync(UserSearchObject search)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search.Username))
            {
                query = query.Where(u => u.Username.Contains(search.Username));
            }

            if (!string.IsNullOrEmpty(search.Email))
            {
                query = query.Where(u => u.Email.Contains(search.Email));
            }

            if (!string.IsNullOrEmpty(search.FTS))
            {
                query = query.Where(u =>
                    u.FirstName.Contains(search.FTS) ||
                    u.LastName.Contains(search.FTS) ||
                    u.Username.Contains(search.FTS) ||
                    u.Email.Contains(search.FTS));
            }

            if (search.GenderId.HasValue)
            {
                query = query.Where(u => u.GenderId == search.GenderId.Value);
            }

            if (search.CityId.HasValue)
            {
                query = query.Where(u => u.CityId == search.CityId.Value);
            }

            if (search.RoleId.HasValue)
            {
                query = query.Where(u => u.UserRoles.Any(ur => ur.RoleId == search.RoleId.Value));
            }

            query = query
                .Include(u => u.Gender)
                .Include(u => u.City)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role);

            int? totalCount = null;
            if (search.IncludeTotalCount)
            {
                totalCount = await query.CountAsync();
            }

            if (!search.RetrieveAll)
            {
                if (search.Page.HasValue)
                {
                    query = query.Skip(search.Page.Value * search.PageSize.Value);
                }
                if (search.PageSize.HasValue)
                {
                    query = query.Take(search.PageSize.Value);
                }
            }

            var users = await query.ToListAsync();
            return new PagedResult<UserResponse>
            {
                Items = users.Select(MapToResponse).ToList(),
                TotalCount = totalCount
            };
        }

        public override async Task<UserResponse?> GetByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.Gender)
                .Include(u => u.City)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return null;

            return MapToResponse(user);
        }

        private string HashPassword(string password, out byte[] salt)
        {
            salt = new byte[SaltSize];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                return Convert.ToBase64String(pbkdf2.GetBytes(KeySize));
            }
        }

        public async Task<UserResponse> CreateAsync(UserUpsertRequest request)
        {
            // Check if user with same email or username already exists
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                throw new InvalidOperationException("User with this email already exists.");
            }

            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            {
                throw new InvalidOperationException("User with this username already exists.");
            }

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Username = request.Username,
                PhoneNumber = request.PhoneNumber,
                GenderId = request.GenderId,
                CityId = request.CityId,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                Picture = request.Picture
            };

            // Hash password if provided
            if (!string.IsNullOrEmpty(request.Password))
            {
                user.PasswordHash = HashPassword(request.Password, out byte[] salt);
                user.PasswordSalt = Convert.ToBase64String(salt);
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Assign roles if provided
            if (request.RoleIds != null && request.RoleIds.Any())
            {
                foreach (var roleId in request.RoleIds)
                {
                    var userRole = new UserRole
                    {
                        UserId = user.Id,
                        RoleId = roleId,
                        DateAssigned = DateTime.UtcNow
                    };
                    _context.UserRoles.Add(userRole);
                }
                await _context.SaveChangesAsync();
            }

            return await GetUserResponseWithRolesAsync(user.Id);
        }

        public async Task<UserResponse?> UpdateAsync(int id, UserUpsertRequest request)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return null;

            // Check if email is being changed and if it already exists
            if (request.Email != user.Email && await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                throw new InvalidOperationException("User with this email already exists.");
            }

            // Check if username is being changed and if it already exists
            if (request.Username != user.Username && await _context.Users.AnyAsync(u => u.Username == request.Username))
            {
                throw new InvalidOperationException("User with this username already exists.");
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.Username = request.Username;
            user.PhoneNumber = request.PhoneNumber;
            user.GenderId = request.GenderId;
            user.CityId = request.CityId;
            user.IsActive = request.IsActive;
            user.Picture = request.Picture;

            // Update password if provided
            if (!string.IsNullOrEmpty(request.Password))
            {
                user.PasswordHash = HashPassword(request.Password, out byte[] salt);
                user.PasswordSalt = Convert.ToBase64String(salt);
            }

            // Update roles if provided
            if (request.RoleIds != null)
            {
                // Remove existing roles
                _context.UserRoles.RemoveRange(user.UserRoles);

                // Add new roles
                foreach (var roleId in request.RoleIds)
                {
                    var userRole = new UserRole
                    {
                        UserId = user.Id,
                        RoleId = roleId,
                        DateAssigned = DateTime.UtcNow
                    };
                    _context.UserRoles.Add(userRole);
                }
            }

            await _context.SaveChangesAsync();
            return await GetUserResponseWithRolesAsync(user.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        protected override UserResponse MapToResponse(User user)
        {
            var response = new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username,
                Picture = user.Picture,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                PhoneNumber = user.PhoneNumber,
                GenderId = user.GenderId,
                GenderName = user.Gender?.Name ?? string.Empty,
                CityId = user.CityId,
                CityName = user.City?.Name ?? string.Empty,
                Roles = user.UserRoles?.Select(ur => new RoleResponse
                {
                    Id = ur.Role.Id,
                    Name = ur.Role.Name,
                    Description = ur.Role.Description
                }).ToList() ?? new List<RoleResponse>()
            };

            return response;
        }

        private async Task<UserResponse> GetUserResponseWithRolesAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Gender)
                .Include(u => u.City)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new InvalidOperationException("User not found.");

            return MapToResponse(user);
        }

        public async Task<UserResponse?> AuthenticateAsync(UserLoginRequest request)
        {
            var user = await _context.Users
                .Include(u => u.Gender)
                .Include(u => u.City)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null || !VerifyPassword(request.Password, user.PasswordHash, user.PasswordSalt))
                return null;

            // Update last login time
            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return MapToResponse(user);
        }

        private bool VerifyPassword(string password, string passwordHash, string passwordSalt)
        {
            var salt = Convert.FromBase64String(passwordSalt);
            var hash = Convert.FromBase64String(passwordHash);
            var hashBytes = new Rfc2898DeriveBytes(password, salt, Iterations).GetBytes(KeySize);
            return hash.SequenceEqual(hashBytes);
        }
    }
}