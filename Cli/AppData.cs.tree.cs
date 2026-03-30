using System;
using System.Collections.Generic;
using Microsoft.CST.AttackSurfaceAnalyzer.Objects;

namespace Microsoft.CST.AttackSurfaceAnalyzer.Cli.Objects
{
    public class TreeNode
    {
        public string Name { get; set; } = string.Empty;
        public string FullPath { get; set; } = string.Empty;
        public Dictionary<string, TreeNode> Children { get; set; } = new Dictionary<string, TreeNode>();
        public CompareResult? Result { get; set; }
        public bool IsExpanded { get; set; } = false;

        public void AddPath(string path, CompareResult result, int skipParts = 0, char? delimiter = null)
        {
            var parts = delimiter.HasValue ? path.Split(delimiter.Value) : path.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
            AddPathRecursive(parts, skipParts, path, result, delimiter);
        }

        private void AddPathRecursive(string[] parts, int index, string fullPath, CompareResult result, char? delimiter = null)
        {
            if (index == parts.Length)
            {
                Result = result;
                return;
            }

            var part = parts[index];
            if (!Children.ContainsKey(part))
            {
                Children[part] = new TreeNode { Name = part, FullPath = string.Join(delimiter ?? '\\', parts[..(index + 1)]) };
            }

            Children[part].AddPathRecursive(parts, index + 1, fullPath, result, delimiter);
        }
    }
}
