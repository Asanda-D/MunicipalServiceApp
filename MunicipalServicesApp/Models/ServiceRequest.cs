using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MunicipalServicesApp.Models
{
    public enum RequestStatus
    {
        Submitted,
        InProgress,
        OnHold,
        Completed,
        Cancelled
    }

    public class ServiceRequestUpdate
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Message { get; set; } = "";
        public RequestStatus Status { get; set; }
    }

    public class ServiceRequest
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = "";

        [Required]
        [StringLength(100)]
        public string Location { get; set; } = "";

        [Required]
        public string Category { get; set; } = "";

        [Required]
        [StringLength(2000)]
        public string Description { get; set; } = "";

        // Optional attachment path (e.g., "/uploads/filename.png")
        public string? AttachmentPath { get; set; }

        public RequestStatus Status { get; set; } = RequestStatus.Submitted;

        public DateTime SubmittedAt { get; set; } = DateTime.Now;

        // Priority (lower number = higher priority). You can set it via UI or auto-calc.
        public int Priority { get; set; } = 100;

        // History of updates
        public List<ServiceRequestUpdate> Updates { get; set; } = new List<ServiceRequestUpdate>();

        // For graph relationships (optional) — list of related request Ids
        public List<int> RelatedRequestIds { get; set; } = new List<int>();
    }
}