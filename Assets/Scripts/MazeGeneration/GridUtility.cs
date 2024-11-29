using System;

namespace ShareefSoftware
{
    public static class GridUtility
    {
        public static void StampInto<T>(GridGraph<T> grid, IGridGraph<T> stamp, int lowerLeftRow, int lowerLeftColumn)
        {
            // Assumes the stamp is smaller than the grid.
            int stampWidth = stamp.NumberOfColumns;
            int stampHeight = stamp.NumberOfRows;
            int stampColumnEnd = (lowerLeftColumn + stampWidth > grid.NumberOfColumns) ? grid.NumberOfColumns : lowerLeftColumn + stampWidth;
            int stampRowEnd = Math.Min(grid.NumberOfRows, stampHeight + lowerLeftRow);
            int iStart = 0;
            int jStart = 0;
            if(lowerLeftColumn < 0)
            {
                iStart = -lowerLeftColumn;
                lowerLeftColumn = 0;
            }
            if(lowerLeftRow < 0)
            {
                jStart = -lowerLeftRow;
                lowerLeftRow = 0;
            }

            int i = iStart;
            for(int row = lowerLeftRow; row < stampRowEnd; row++)
            {
                int j = jStart;
                for(int column = lowerLeftColumn; column < stampColumnEnd; column++)
                {
                    grid.SetCellValue(row, column, stamp.GetCellValue(i,j));
                    j++;
                }
                i++;
            }
        }
    }
}