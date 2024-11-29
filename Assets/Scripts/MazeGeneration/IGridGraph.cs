using System.Collections.Generic;

namespace ShareefSoftware
{
    public interface IGridGraph<T> : IEnumerable<(int Row, int Column, T NodeValue)>
    {
        int NumberOfRows { get; }
        int NumberOfColumns { get; }

        T GetCellValue(int row, int column);
        IEnumerable<(int Row, int Column)> Neighbors(int row, int column);

        /*
         * (Optional) Define the method signature for a method that will retrieve neighbors
         * of a cell if you will not use the 'Neighbors' method above.
         * Use a different method name than 'Neighbors'.
         */
    }
}
