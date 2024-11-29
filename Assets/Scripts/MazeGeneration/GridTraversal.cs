using System;
using System.Collections.Generic;

namespace ShareefSoftware
{
    
    public class GridTraversal<T>
    {
        private readonly IGridGraph<T> grid;

        
        public GridTraversal(IGridGraph<T> grid)
        {
            this.grid = grid;
        }

      
        // Generates a maze starting from the specified cell using Prim's algorithm.
        public IEnumerable<((int Row, int Column) From, (int Row, int Column) To)> GenerateMaze(int startRow, int startColumn)
        {
            HashSet<(int Row, int Column)> visited = new HashSet<(int Row, int Column)>();
            HashSet<(int Row, int Column)> unvisited = new HashSet<(int Row, int Column)>();

            // Populate the Unvisited set with all vertices in the grid
            PopulateUnvisited(unvisited);

            var start = (startRow, startColumn);
            unvisited.Remove(start);
            visited.Add(start);

            List<((int Row, int Column) From, (int Row, int Column) To)> eligibleEdges = new List<((int Row, int Column) From, (int Row, int Column) To)>();

            // Add the starting cell's neighbors' edges to the eligible edges
            foreach (var neighbor in grid.Neighbors(startRow, startColumn))
            {
                if (unvisited.Contains(neighbor))
                {
                    eligibleEdges.Add((start, neighbor));
                }
            }

            while (unvisited.Count > 0 && eligibleEdges.Count > 0)
            {
                // Select a random edge
                var randomEdge = eligibleEdges[new Random().Next(eligibleEdges.Count)];
                yield return randomEdge;

                // Mark the new vertex as visited
                var (from, to) = randomEdge;
                visited.Add(to);
                unvisited.Remove(to);

                // Update eligible edges with the neighbors of the newly visited vertex
                foreach (var neighbor in grid.Neighbors(to.Row, to.Column))
                {
                    if (unvisited.Contains(neighbor))
                    {
                        eligibleEdges.Add((to, neighbor));
                    }
                }

                // Remove edges that connect to already visited vertices
                eligibleEdges.RemoveAll(edge => visited.Contains(edge.To));
            }
        }

        private void PopulateUnvisited(HashSet<(int Row, int Column)> unvisited)
        {
            // Loop through all cells in the grid
            for (int row = 0; row < grid.NumberOfRows; row++)
            {
                for (int column = 0; column < grid.NumberOfColumns; column++)
                {
                    // Add each vertex (row, column) to the Unvisited set
                    unvisited.Add((row, column));
                }
            }
        }
    }
}
