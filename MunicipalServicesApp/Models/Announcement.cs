using System;

namespace MunicipalServicesApp.Models
{
    public class Announcement
    {
        public int Id { get; set; }  // Unique ID
        public string Title { get; set; }  // Announcement title
        public string Message { get; set; }  // Main content
        public string? AttachmentPath { get; set; }  // Optional file
        public DateTime CreatedAt { get; set; } = DateTime.Now;  // Creation timestamp
    }
}