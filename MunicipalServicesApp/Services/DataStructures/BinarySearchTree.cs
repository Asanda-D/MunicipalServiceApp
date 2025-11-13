using System;
using System.Collections.Generic;
using MunicipalServicesApp.Models;

namespace MunicipalServicesApp.Services.DataStructures
{
    // Node of the Binary Search Tree
    public class BstNode
    {
        // Key used for ordering nodes
        public int Key { get; set; }

        // The service request stored in this node
        public ServiceRequest Value { get; set; }

        // Left child node (smaller keys)
        public BstNode? Left { get; set; }

        // Right child node (larger keys)
        public BstNode? Right { get; set; }

        // Constructor to create a new node
        public BstNode(int key, ServiceRequest value)
        {
            Key = key;
            Value = value;
        }
    }

    // Binary Search Tree to store and manage service requests
    public class BinarySearchTree
    {
        // The root node of the tree
        private BstNode? _root;

        // Insert a new service request into the tree
        public void Insert(int key, ServiceRequest value)
        {
            _root = InsertRec(_root, key, value);
        }

        // Helper method for inserting nodes recursively
        private BstNode InsertRec(BstNode? node, int key, ServiceRequest value)
        {
            // If tree is empty, create a new node
            if (node == null) return new BstNode(key, value);

            // Go to the left or right branch depending on key
            if (key < node.Key)
                node.Left = InsertRec(node.Left, key, value);
            else if (key > node.Key)
                node.Right = InsertRec(node.Right, key, value);
            else
                node.Value = value; // Update if key already exists

            return node;
        }

        // Find a service request by its key
        public ServiceRequest? Find(int key)
        {
            var node = _root;
            while (node != null)
            {
                if (key == node.Key) return node.Value;
                node = key < node.Key ? node.Left : node.Right;
            }
            return null;
        }

        // Return all service requests in sorted order
        public IEnumerable<ServiceRequest> InOrderTraversal()
        {
            var list = new List<ServiceRequest>();
            InOrderRec(_root, list);
            return list;
        }

        // Helper method to traverse the tree in-order
        private void InOrderRec(BstNode? node, List<ServiceRequest> list)
        {
            if (node == null) return;
            InOrderRec(node.Left, list);
            list.Add(node.Value);
            InOrderRec(node.Right, list);
        }

        // Remove all nodes from the tree
        public void Clear() => _root = null;
    }
}