using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ManiFest.Model.Requests
{
    public class UserUpsertRequest
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        public byte[]? Picture { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;
        
        [Phone]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
        
        [Required]
        public int GenderId { get; set; }
        
        [Required]
        public int CityId { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        // Only used when creating a new user
        [MinLength(4)]
        public string? Password { get; set; }
        
        // Collection of role IDs to assign to the user
        public List<int> RoleIds { get; set; } = new List<int>();
    }
} 