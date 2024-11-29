using System.Collections.Generic;
using UnityEngine;

namespace ShareefSoftware
{
    public class LevelGeneration : MonoBehaviour
    {
        [SerializeField] TileStyle tileStyle;
        [SerializeField] int numberOfRows = 10;
        [SerializeField] int numberOfColumns = 10;
        [SerializeField] List<GameObject> barrierPrefabs;
        [SerializeField] List<GameObject> pathPrefabs;
        [SerializeField] private float cellWidth;
        [SerializeField] private float cellHeight;
        [SerializeField] private Transform parentForNewObjects;
        [SerializeField] int randomSeed = 0;

        private RotateMaze rotateMaze; // Instance of RotateMaze
        private Maze maze; // Private field to store the maze instance

        private void Awake()
        {
            System.Random random = CreateRandom();
            maze = new Maze(numberOfRows, numberOfColumns, random); // Store the maze instance
            IGridGraph<bool> occupancyGrid = ConvertMazeToOccupancyGraph(maze);
            CreatePrefabs(random, occupancyGrid);

            GameObject rotateMazeObject = new GameObject("RotateMaze");
            rotateMaze = rotateMazeObject.AddComponent<RotateMaze>();

            rotateMaze.levelGeneration = this;
        }

        // Public methods to access properties
        public float GetNumberOfRows() => numberOfRows;
        public float GetNumberOfColumns() => numberOfColumns;
        public float GetCellWidth() => cellWidth;
        public float GetCellHeight() => cellHeight;

        public Maze GetMaze() => maze; // Public method to access the maze instance

        private void CreatePrefabs(System.Random random, IGridGraph<bool> occupancyGrid)
        {
            var pathFactory = new GameObjectFactoryRandomFromList(pathPrefabs, random) { Parent = parentForNewObjects };
            var wallFactory = new GameObjectFactoryRandomFromList(barrierPrefabs, random) { Parent = parentForNewObjects };
            var gridFactory = new GridGameObjectFactory(cellWidth, cellHeight)
            {
                PrefabFactoryIfTrue = pathFactory,
                PrefabFactoryIfFalse = wallFactory
            };
            gridFactory.CreatePrefabs(occupancyGrid);
        }

        private System.Random CreateRandom()
        {
            if (randomSeed == 0)
            {
                randomSeed = (int)System.DateTime.Now.Ticks & 0x0000FFFF;
            }
            Debug.Log("Random Seed: " + randomSeed);
            System.Random random = new System.Random(randomSeed);
            return random;
        }

        private IGridGraph<bool> ConvertMazeToOccupancyGraph(Maze maze)
        {
            IGridGraph<bool> occupancyGrid;
            if (tileStyle == TileStyle.Small2x2)
                occupancyGrid = MazeUtility.Create2x2OccupancyGridFromMaze(maze);
            else
                occupancyGrid = MazeUtility.Create3x3OccupancyGridFromMaze(maze, tileStyle);
            PrintOccupancyGrid(occupancyGrid);
            return occupancyGrid;
        }

        private static void PrintOccupancyGrid(IGridGraph<bool> occupancyGrid)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            for (int row = occupancyGrid.NumberOfRows - 1; row >= 0; row--)
            {
                for (int column = 0; column < occupancyGrid.NumberOfColumns; column++)
                {
                    char symbol = (occupancyGrid.GetCellValue(row, column) == true) ? ' ' : '█';
                    stringBuilder.Append(symbol);
                }
                stringBuilder.AppendLine();
            }
            stringBuilder.AppendLine();
            Debug.Log(stringBuilder.ToString());
        }
    }
}
