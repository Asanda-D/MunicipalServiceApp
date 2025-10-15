using System.ComponentModel.DataAnnotations;

namespace MunicipalServicesApp.Models
{
    public class Issue
    {
        // Unique identifier for the issue
        public int Id { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(100, ErrorMessage = "Location cannot be longer than 100 characters.")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [StringLength(50)]
        public string Category { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; }

        // Path to the uploaded file (optional)
        public string? AttachmentPath { get; set; }

        // Date and time the issue was submitted
        public DateTime SubmittedAt { get; set; } = DateTime.Now;
    }
}