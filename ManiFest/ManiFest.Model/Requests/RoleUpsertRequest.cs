using System.ComponentModel.DataAnnotations;

namespace ManiFest.Model.Requests
{
    public class RoleUpsertRequest
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
    }
} 