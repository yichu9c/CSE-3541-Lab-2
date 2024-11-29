using UnityEngine;

namespace ShareefSoftware
{
    public class GridGameObjectFactory
    {
        const string baseCellNameFormat = "{0}_{1}_{2}";
        const string trueCellName = "TrueCell";
        const string falseCellName = "FalseCell";
        private readonly float cellWidth;
        private readonly float cellHeight;

        public IGameObjectFactory PrefabFactoryIfTrue { get; set; }
        public IGameObjectFactory PrefabFactoryIfFalse { get; set; }

        public GridGameObjectFactory(float cellWidth, float cellHeight)
        {
            this.cellWidth = cellWidth;
            this.cellHeight = cellHeight;
        }

        public void CreatePrefabs(IGridGraph<bool> grid)
        {
            for (int row = 0; row < grid.NumberOfRows; row++)
            {
                for (int column = 0; column < grid.NumberOfColumns; column++)
                {
                    Vector3 position = new Vector3(cellWidth * column, 0, cellHeight * row);
                    bool gridCell = grid.GetCellValue(row, column);
                    string cellName = gridCell ? trueCellName : falseCellName;
                    string name = string.Format(baseCellNameFormat, cellName, row, column);
                    if (grid.GetCellValue(row, column))
                    {
                        PrefabFactoryIfTrue.CreateAt(position, name);
                    }
                    else
                    {
                        PrefabFactoryIfFalse.CreateAt(position, name);
                    }
                }
            }
        }
    }
}