using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MunicipalServicesApp.Models;
using MunicipalServicesApp.Services.DataStructures;

namespace MunicipalServicesApp.Services
{
    // Singleton service that manages all service requests
    public class RequestsService
    {
        // Internal data storage and supporting structures
        private readonly List<ServiceRequest> _requests = new();
        private readonly BinarySearchTree _bst = new();
        private readonly MinHeap _heap = new();
        private readonly Graph _graph = new();

        public RequestsService()
        {
            // Add initial data and build structures when the service starts
            SeedSampleRequests();
            RebuildStructures();
        }

        // ===============================
        // ==== STRUCTURE MAINTENANCE ====
        // ===============================

        // Rebuild all structures from the main request list
        public void RebuildStructures()
        {
            _bst.Clear();
            _heap.Clear();
            _graph.Clear();

            foreach (var r in _requests)
            {
                _bst.Insert(r.Id, r);
                _heap.Insert(r.Priority, r);
                _graph.AddNode(r.Id);
                foreach (var rel in r.RelatedRequestIds)
                    _graph.AddEdge(r.Id, rel);
            }
        }

        // Rebuild only the heap (used when priorities or statuses change)
        private void RebuildHeap()
        {
            _heap.Clear();
            foreach (var r in _requests)
                _heap.Insert(r.Priority, r);
        }

        // ===============================
        // ========= CRUD LOGIC ==========
        // ===============================

        // Get all requests ordered by most recent
        public IEnumerable<ServiceRequest> GetAll() =>
            _requests.OrderByDescending(r => r.SubmittedAt);

        // Find a request by ID using the binary search tree
        public ServiceRequest? GetById(int id) =>
            _bst.Find(id);

        // Filter requests by their current status
        public IEnumerable<ServiceRequest> GetByStatus(RequestStatus status) =>
            _requests.Where(r => r.Status == status)
                     .OrderByDescending(r => r.SubmittedAt);

        // Return top requests based on priority using the min-heap
        public IEnumerable<ServiceRequest> TopPriority(int count = 5) =>
            _heap.PeekAllOrdered().Take(count);

        // Add a new service request
        public void AddRequest(ServiceRequest r, string? attachmentPath = null)
        {
            // Assign a unique ID if missing
            if (r.Id == 0)
                r.Id = _requests.Count > 0 ? _requests.Max(x => x.Id) + 1 : 1;

            // Attach uploaded file if provided
            if (!string.IsNullOrEmpty(attachmentPath))
                r.AttachmentPath = attachmentPath;

            // Set submission time and default update
            r.SubmittedAt = DateTime.Now;
            r.Updates.Add(new ServiceRequestUpdate
            {
                Status = RequestStatus.Submitted,
                Message = "Request submitted."
            });

            // Add to list and supporting structures
            _requests.Add(r);
            _bst.Insert(r.Id, r);
            _heap.Insert(r.Priority, r);
            _graph.AddNode(r.Id);
        }

        // Update details of an existing request
        public bool UpdateRequest(ServiceRequest updated, string? newAttachmentPath = null)
        {
            var existing = GetById(updated.Id);
            if (existing == null) return false;

            existing.Title = updated.Title;
            existing.Description = updated.Description;
            existing.Location = updated.Location;
            existing.Category = updated.Category;
            existing.Priority = updated.Priority;

            // Update attachment if a new one is provided
            if (!string.IsNullOrEmpty(newAttachmentPath))
                existing.AttachmentPath = newAttachmentPath;

            RebuildStructures();
            return true;
        }

        // Update only the status and add a new update entry
        public bool UpdateStatus(int id, RequestStatus newStatus, string note = "")
        {
            var r = GetById(id);
            if (r == null) return false;

            r.Status = newStatus;
            r.Updates.Add(new ServiceRequestUpdate
            {
                Status = newStatus,
                Message = note,
                Timestamp = DateTime.Now
            });

            RebuildHeap();
            return true;
        }

        // Remove a request completely
        public bool Delete(int id)
        {
            var r = GetById(id);
            if (r == null) return false;

            _requests.Remove(r);
            RebuildStructures();
            return true;
        }

        // ===============================
        // ====== RELATION HANDLING ======
        // ===============================

        // Create a relationship between two requests
        public void AddRelation(int fromId, int toId)
        {
            _graph.AddEdge(fromId, toId);
        }

        // Get related request IDs
        public IEnumerable<int> GetRelated(int id) =>
            _graph.GetNeighbors(id);

        // ===============================
        // ====== FILE MANAGEMENT ========
        // ===============================

        // Save uploaded files to a given path
        public string SaveAttachment(Stream fileStream, string fileName, string rootPath)
        {
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            var filePath = Path.Combine(rootPath, fileName);
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                fileStream.CopyTo(fs);
            }

            // Return relative path for display
            return $"/uploads/{fileName}";
        }

        // ===============================
        // ======= SAMPLE DATA ===========
        // ===============================

        // Add a few example requests for testing
        private void SeedSampleRequests()
        {
            var a = new ServiceRequest
            {
                Title = "Blocked Drain - Durban",
                Location = "12 Smith Street",
                Category = "Sanitation",
                Description = "Storm drain is blocked and overflowing into road.",
                Priority = 10
            };
            AddRequest(a);

            var b = new ServiceRequest
            {
                Title = "Streetlight Broken - Pinetown",
                Location = "Lupin Place",
                Category = "Utilities",
                Description = "Streetlight not working on Baker Ave near the shop.",
                Priority = 30
            };
            AddRequest(b);

            // Create a relation between sample requests
            AddRelation(a.Id, b.Id);
        }
    }
}