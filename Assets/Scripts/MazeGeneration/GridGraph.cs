using System.Collections;
using System.Collections.Generic;

namespace ShareefSoftware
{
    public class GridGraph<T> : IGridGraph<T>
    {
        private readonly IGraphValueAccessor<T> graphNodes;
        public int NumberOfRows { get; }
        public int NumberOfColumns { get; }

        public GridGraph(int numberOfRows, int numberOfColumns, IGraphValueAccessor<T> graphNodes)
        {
            this.NumberOfRows = numberOfRows;
            this.NumberOfColumns = numberOfColumns;
            this.graphNodes = graphNodes;
        }

        public T GetCellValue(int row, int column)
        {
            return graphNodes.GetNodeValue(row,column);
        }
        public IEnumerable<(int Row, int Column)> Neighbors(int row, int column)
        {
            if (row < NumberOfRows - 1) yield return (row + 1, column);
            if (row > 0) yield return (row - 1, column);
            if (column < NumberOfColumns - 1) yield return (row, column + 1);
            if (column > 0) yield return (row, column - 1);
        }

        /*
         * (Optional) If you defined your own method signature in IGridGraph
         * to determine neighbors (instead of using method 'Neighbors'), 
         * then define it here. Use the same method name as you defined in IGridGraph.
         */

        public void SetCellValue(int row, int column, T cellValue)
        {
            graphNodes.SetNodeValue(row, column, cellValue);
        }

        public IEnumerator<(int Row, int Column, T NodeValue)> GetEnumerator()
        {
            for (int row = 0; row < NumberOfRows; row++)
            {
                for (int column = 0; column < NumberOfColumns; column++)
                {
                    T nodeValue = GetCellValue(row, column);
                        yield return (row, column, nodeValue);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}