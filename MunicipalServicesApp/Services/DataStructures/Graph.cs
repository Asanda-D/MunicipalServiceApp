using System;
using System.Collections.Generic;

namespace MunicipalServicesApp.Services.DataStructures
{
    // Simple directed graph for request dependencies or relationships
    public class Graph
    {
        // adjacency list: node id -> neighbours
        private readonly Dictionary<int, List<int>> _adj = new();

        public void AddNode(int id)
        {
            if (!_adj.ContainsKey(id)) _adj[id] = new List<int>();
        }

        public void RemoveNode(int id)
        {
            if (_adj.ContainsKey(id)) _adj.Remove(id);
            // remove edges to id
            foreach (var kv in _adj.Values)
                kv.RemoveAll(x => x == id);
        }

        public void AddEdge(int from, int to)
        {
            AddNode(from); AddNode(to);
            if (!_adj[from].Contains(to)) _adj[from].Add(to);
        }

        public IEnumerable<int> GetNeighbors(int id)
        {
            if (_adj.TryGetValue(id, out var neighbours)) return neighbours;
            return Array.Empty<int>();
        }

        public IEnumerable<int> Nodes => _adj.Keys;

        // BFS traversal from start
        public IEnumerable<int> BreadthFirstSearch(int start)
        {
            var result = new List<int>();
            if (!_adj.ContainsKey(start)) return result;
            var q = new Queue<int>();
            var visited = new HashSet<int>();
            q.Enqueue(start); visited.Add(start);
            while (q.Count > 0)
            {
                var n = q.Dequeue();
                result.Add(n);
                foreach (var nb in _adj[n])
                {
                    if (!visited.Contains(nb)) { visited.Add(nb); q.Enqueue(nb); }
                }
            }
            return result;
        }
        
        public void Clear()
        {
            _adj.Clear();
        }
    }
}