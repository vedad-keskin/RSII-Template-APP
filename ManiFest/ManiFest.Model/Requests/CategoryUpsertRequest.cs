using System.ComponentModel.DataAnnotations;

namespace ManiFest.Model.Requests
{
    public class CategoryUpsertRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
