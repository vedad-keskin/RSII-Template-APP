using System;
using System.Collections.Generic;

namespace ManiFest.Model.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public byte[]? Picture { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public string? PhoneNumber { get; set; }
        
        // Gender and City information
        public int GenderId { get; set; }
        public string GenderName { get; set; } = string.Empty;
        public int CityId { get; set; }
        public string CityName { get; set; } = string.Empty;
        
        // Collection of roles assigned to the user
        public List<RoleResponse> Roles { get; set; } = new List<RoleResponse>();
    }
} 