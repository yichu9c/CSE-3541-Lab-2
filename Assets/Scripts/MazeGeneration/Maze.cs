using System.Collections;
using System.Collections.Generic;

namespace ShareefSoftware
{
    public class Maze : IGridGraph<Direction>
    {
        private GridGraph<Direction> mazeGrid;

        private readonly int startRow = 0;
        private readonly int startColumn = 0;
        private readonly int endRow, endColumn;
        private readonly Direction startFromDirection = Direction.South;
        private readonly Direction endToDirection;

        public Maze(int numberOfRows, int numberOfColumns, System.Random random)
        {
            endRow = numberOfRows - 1;
            endColumn = numberOfColumns - 1;
            endToDirection = Direction.North;

            CreateMaze(numberOfRows, numberOfColumns, random);
            AddEntrance();
            AddExit();
        }

        private void CreateMaze(int numberOfRows, int numberOfColumns, System.Random random)
        {
            /*
             * Replace ??? with your code
             */
            var grid = new GridGraphRandomizedNeighborsDecorator<int>(new GridGraph<int>(numberOfRows, numberOfColumns, GraphValueAccessorConstant<int>.DefaultConstant), random);

            var mazeGenerator = new GridTraversal<int>(grid);

            mazeGrid = new GridGraph<Direction>(numberOfRows, numberOfColumns, new GridNodeDataStore<Direction>(numberOfRows, numberOfColumns));

            foreach (var edge in mazeGenerator.GenerateMaze(startRow, startColumn))
            {
                (Direction forwardDir, Direction backwardDir) = DetermineDirections(edge);
                CarveEdge(edge.From, forwardDir);
                CarveEdge(edge.To, backwardDir);
            }
        }

        private void AddEntrance()
        {
            Direction dirs = mazeGrid.GetCellValue(startRow, startColumn);
            dirs |= startFromDirection;
            mazeGrid.SetCellValue(startRow, startColumn, dirs);
        }

        private void AddExit()
        {
            Direction dirs = mazeGrid.GetCellValue(endRow,endColumn);
            dirs |= endToDirection;
            mazeGrid.SetCellValue(endRow, endColumn, dirs);
        }

        private void CarveEdge((int Row,int column) cell, Direction edgeDir)
        {
            Direction dirs = mazeGrid.GetCellValue(cell.Row, cell.column);
            dirs |= edgeDir;
            mazeGrid.SetCellValue(cell.Row, cell.column, dirs);
        }

        static readonly Dictionary<(int rowDelta, int columnDelta), (Direction forwardDir, Direction backwardDir)> directionMapping 
            = new Dictionary<(int rowDelta, int columnDelta), (Direction forwardDir, Direction backwardDir)>() {
                { (-1,  0), (Direction.South, Direction.North) },
                { ( 1,  0), (Direction.North, Direction.South) },
                { ( 0, -1), (Direction.West, Direction.East) },
                { ( 0,  1), (Direction.East, Direction.West) }
            };

        public int NumberOfRows => ((IGridGraph<Direction>)mazeGrid).NumberOfRows;

        public int NumberOfColumns => ((IGridGraph<Direction>)mazeGrid).NumberOfColumns;

        private static (Direction ForwardDir, Direction BackwardDir) DetermineDirections(((int FromRow, int FromColumn) From, (int ToRow, int ToColumn) To) edge)
        {
            int rowDelta = edge.To.ToRow - edge.From.FromRow;
            int columnDelta = edge.To.ToColumn - edge.From.FromColumn;
            return directionMapping[(rowDelta, columnDelta)];
        }

        public Direction GetCellValue(int row, int column)
        {
            return ((IGridGraph<Direction>)mazeGrid).GetCellValue(row, column);
        }

        public IEnumerable<(int Row, int Column)> Neighbors(int row, int column)
        {
            foreach(var neighbor in ((IGridGraph<Direction>)mazeGrid).Neighbors(row, column))
            {
                (Direction ForwardDir, Direction _) = DetermineDirections(((row,column),neighbor));
                if(mazeGrid.GetCellValue(row,column).HasFlag(ForwardDir))
                {
                    yield return neighbor;
                }
            }
        }

        public IEnumerator<(int Row, int Column, Direction NodeValue)> GetEnumerator()
        {
            return ((IEnumerable<(int Row, int Column, Direction NodeValue)>)mazeGrid).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)mazeGrid).GetEnumerator();
        }
    }
}