using System.Collections.Generic;
using UnityEngine;

namespace ShareefSoftware
{
    public class CoinPlacer : MonoBehaviour
    {
        [SerializeField] private List<GameObject> coinPrefabs;  
        [SerializeField] private LevelGeneration levelGeneration;  
        private GameObjectFactoryRandomFromList coinFactory;  
        public List<Vector3> coinPositions = new List<Vector3>();

        void Start()
        {

            if (levelGeneration == null)
            {
                Debug.LogError("LevelGeneration component is not found!");
                return;
            }

            // Initialize the coin factory with the coin prefabs
            coinFactory = new GameObjectFactoryRandomFromList(coinPrefabs);


            Maze maze = levelGeneration.GetMaze();
            if (maze == null)
            {
                Debug.LogError("Maze instance is not assigned!");
                return;
            }

            // Get the dead-end positions
            IEnumerable<(int Row, int Column)> deadEnds = MazeQuery.DeadEnds(maze);


            foreach (var (row, column) in deadEnds)
            {
                PlaceCoin(row, column);
            }
        }

        private void PlaceCoin(int row, int column)
        {

            float cellWidth = levelGeneration.GetCellWidth();
            float cellHeight = levelGeneration.GetCellHeight();


            // Calculate the world position for the coin based on the row and column
            Vector3 coinPosition = new Vector3(column * cellWidth, 1.0f, row * cellHeight);  // Placed slightly above the ground

            // Store the position in the public list
            coinPositions.Add(coinPosition);

            // Use the coin factory to create a coin at the calculated position
            GameObject coin = coinFactory.CreateAt(coinPosition);

            // Attach the CoinMovement script to the coin for floating and rotating effect
            coin.AddComponent<CoinMovement>();
        }
    }
}
