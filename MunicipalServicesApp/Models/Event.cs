using System;

namespace MunicipalServicesApp.Models
{
    public class EventItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Category { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; } = "";
        // Path to the uploaded file
        public string? AttachmentPath { get; set; }
    }
}
