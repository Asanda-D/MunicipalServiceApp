using System;
using System.Collections.Generic;
using MunicipalServicesApp.Models;

namespace MunicipalServicesApp.Services.DataStructures
{
    // A simple MinHeap used to manage service requests by priority
    public class MinHeap
    {
        // Stores pairs of (priority, ServiceRequest)
        // Lower priority value means higher importance
        private List<(int priority, ServiceRequest req)> _data = new();

        // Returns how many items are in the heap
        public int Count => _data.Count;

        // Removes all elements from the heap
        public void Clear() => _data.Clear();

        // Adds a new service request to the heap
        public void Insert(int priority, ServiceRequest req)
        {
            _data.Add((priority, req));
            HeapifyUp(_data.Count - 1);
        }

        // Removes and returns the service request with the smallest priority
        public ServiceRequest? PopMin()
        {
            if (_data.Count == 0) return null;

            // Get the root element (smallest priority)
            var root = _data[0].req;

            // If there’s only one item, clear and return it
            if (_data.Count == 1)
            {
                _data.Clear();
                return root;
            }

            // Replace root with the last element and reheapify
            _data[0] = _data[^1];
            _data.RemoveAt(_data.Count - 1);
            HeapifyDown(0);
            return root;
        }

        // Returns all service requests in order without changing the heap
        public IEnumerable<ServiceRequest> PeekAllOrdered()
        {
            var copy = new List<(int priority, ServiceRequest req)>(_data);
            var result = new List<ServiceRequest>();
            var tempHeap = new MinHeap();

            // Build a temporary heap and extract elements in order
            foreach (var (p, r) in copy) tempHeap.Insert(p, r);
            while (tempHeap.Count > 0)
            {
                var r = tempHeap.PopMin();
                if (r != null) result.Add(r);
            }

            return result;
        }

        // Moves an element up to maintain heap order after insertion
        private void HeapifyUp(int idx)
        {
            while (idx > 0)
            {
                int parent = (idx - 1) / 2;

                // Stop if parent has smaller or equal priority
                if (_data[idx].priority >= _data[parent].priority) break;

                // Swap with parent
                (_data[idx], _data[parent]) = (_data[parent], _data[idx]);
                idx = parent;
            }
        }

        // Moves an element down to maintain heap order after deletion
        private void HeapifyDown(int idx)
        {
            int last = _data.Count - 1;
            while (true)
            {
                int left = idx * 2 + 1;
                int right = idx * 2 + 2;
                int smallest = idx;

                // Check left and right children to find the smallest
                if (left <= last && _data[left].priority < _data[smallest].priority) smallest = left;
                if (right <= last && _data[right].priority < _data[smallest].priority) smallest = right;

                // If no smaller child, stop
                if (smallest == idx) break;

                // Swap and continue down the heap
                (_data[idx], _data[smallest]) = (_data[smallest], _data[idx]);
                idx = smallest;
            }
        }
    }
}