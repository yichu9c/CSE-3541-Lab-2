using System;

namespace ShareefSoftware
{
    public enum TileStyle { Small2x2, Tight3x3, Open3x3, Wrong3x3 };

    public static class MazeUtility
    {
        public static IGridGraph<TStamp> ReplaceDirectionsWithStamps<TStamp>(IGridGraph<Direction> maze, StampSet<Direction, TStamp> stampSet)
        {
            int width = stampSet.Width* maze.NumberOfColumns;
            int height = stampSet.Height * maze.NumberOfRows;
            var stampBackingStore = new GridNodeDataStore<TStamp>(height, width);
            var newMaze = new GridGraph<TStamp>(height, width, stampBackingStore);
            for(int row = 0; row < maze.NumberOfRows; row++)
            {
                for(int column = 0; column < maze.NumberOfColumns; column++)
                {
                    var stamp = stampSet.GetStamp(maze.GetCellValue(row, column));
                    GridUtility.StampInto(newMaze, stamp, stampSet.Height * row, stampSet.Width * column);
                }
            }
            return newMaze;
        }

        public static IGridGraph<bool> Create3x3OccupancyGridFromMaze(IGridGraph<Direction> maze, TileStyle openStyle = TileStyle.Tight3x3)
        {
            var stampSet = CreateStampSet3x3Bool(openStyle);
            return ReplaceDirectionsWithStamps(maze, stampSet);
        }

        public static IGridGraph<bool> Create2x2OccupancyGridFromMaze(IGridGraph<Direction> maze)
        {
            var stampSet = CreateStampSet2x2Bool();
            var tempGrid = ReplaceDirectionsWithStamps(maze, stampSet);
            var backingStore = new GridNodeDataStore<bool>(tempGrid.NumberOfRows + 1, tempGrid.NumberOfColumns + 1);
            var occupancyGraph = new GridGraph<bool>(tempGrid.NumberOfRows + 1, tempGrid.NumberOfColumns + 1, backingStore);
            GridUtility.StampInto(occupancyGraph, tempGrid, 1, 0);
            for (int column = 0; column < maze.NumberOfColumns; column++)
            {
                if (maze.GetCellValue(0, column).HasFlag(Direction.South))
                {
                    occupancyGraph.SetCellValue(0, 2 * column + 1, true);
                    occupancyGraph.SetCellValue(1, 2 * column + 1, true);
                }
            }
            for (int row = 0; row < maze.NumberOfRows; row++)
            {
                if (maze.GetCellValue(row, maze.NumberOfColumns-1).HasFlag(Direction.East))
                {
                    occupancyGraph.SetCellValue(2*row, occupancyGraph.NumberOfColumns - 1, true);
                    occupancyGraph.SetCellValue(2*row, occupancyGraph.NumberOfColumns - 2, true);
                }
            }
            return occupancyGraph;
        }

        public static void FillAllCells<T>(this GridGraph<T> occupancyGrid, T fillValue)
        {
            for (int row = 0; row < occupancyGrid.NumberOfRows; row++)
            {
                for (int column = 0; column < occupancyGrid.NumberOfColumns; column++)
                {
                    occupancyGrid.SetCellValue(row, column, fillValue);
                }
            }
        }

        public static StampSet<Direction,bool> CreateStampSet3x3Bool(TileStyle openStyle)
        {
            Func<Direction,GridGraph<bool>> createStampFunc = CreateStampClosed3x3;
            if (openStyle == TileStyle.Open3x3)
                createStampFunc = CreateStampOpen3x3;
            if (openStyle == TileStyle.Wrong3x3)
                createStampFunc = CreateStampOpenMore3x3;

            var stampSet = new StampSet<Direction, bool>(3, 3);
            for(int i=0; i <= 15; i++)
            {
                Direction direction = (Direction)i;
                var stamp = createStampFunc(direction);
                stampSet.RegisterStamp(direction, stamp);
            }
            return stampSet;
        }

        public static StampSet<Direction, bool> CreateStampSet2x2Bool()
        {
            var stampSet = new StampSet<Direction, bool>(2, 2);
            for (int i = 0; i <= 15; i++)
            {
                Direction direction = (Direction)i;
                var stamp = CreateStampClosed2x2(direction);
                stampSet.RegisterStamp(direction, stamp);
            }
            return stampSet;
        }

        private static GridGraph<bool> CreateStampOpen3x3(Direction dir)
        {
            var backingStore = new GridNodeDataStore<bool>(3, 3);
            var occupancyGrid = new GridGraph<bool>(3, 3, backingStore);
            occupancyGrid.FillAllCells(true);
            bool closedCell = (dir == Direction.Blank);
            bool west = (dir & Direction.West) == Direction.West;
            bool north = (dir & Direction.North) == Direction.North;
            bool east = (dir & Direction.East) == Direction.East;
            bool south = (dir & Direction.South) == Direction.South;
            // If no entrances, seal the center
            occupancyGrid.SetCellValue(1, 1, !closedCell);
            // If no entrance from the west, add a left wall.
            if (!west)
            {
                occupancyGrid.SetCellValue(0, 0, false);
                occupancyGrid.SetCellValue(1, 0, false);
                occupancyGrid.SetCellValue(2, 0, false);
            }
            // If no entrance from the south, add a bottom wall
            if (!south)
            {
                occupancyGrid.SetCellValue(0, 0, false);
                occupancyGrid.SetCellValue(0, 1, false);
                occupancyGrid.SetCellValue(0, 2, false);
            }
            // If no entrance from the east, add a right wall
            if (!east)
            {
                occupancyGrid.SetCellValue(0, 2, false);
                occupancyGrid.SetCellValue(1, 2, false);
                occupancyGrid.SetCellValue(2, 2, false);
            }
            // If no entrance from the north, add a top wall
            if (!north)
            {
                occupancyGrid.SetCellValue(2, 0, false);
                occupancyGrid.SetCellValue(2, 1, false);
                occupancyGrid.SetCellValue(2, 2, false);
            }
            return occupancyGrid;
        }

        // This will produce non-valid tiles for a perfect maze, but interesting dungeons.
        private static GridGraph<bool> CreateStampOpenMore3x3(Direction dir)
        {
            var backingStore = new GridNodeDataStore<bool>(3, 3);
            var occupancyGrid = new GridGraph<bool>(3, 3, backingStore);
            //occupancyGrid.FillAllCells(true);
            bool closedCell = (dir == Direction.Blank);
            bool west = (dir & Direction.West) == Direction.West;
            bool north = (dir & Direction.North) == Direction.North;
            bool east = (dir & Direction.East) == Direction.East;
            bool south = (dir & Direction.South) == Direction.South;
            // If no entrances, seal the center
            occupancyGrid.SetCellValue(1, 1, !closedCell);
            // If no entrance from the west, add a left wall.
            if (west)
            {
                occupancyGrid.SetCellValue(0, 0, true);
                occupancyGrid.SetCellValue(1, 0, true);
                occupancyGrid.SetCellValue(2, 0, true);
            }
            // If no entrance from the south, add a bottom wall
            if (south)
            {
                occupancyGrid.SetCellValue(0, 0, true);
                occupancyGrid.SetCellValue(0, 1, true);
                occupancyGrid.SetCellValue(0, 2, true);
            }
            // If no entrance from the east, add a right wall
            if (east)
            {
                occupancyGrid.SetCellValue(0, 2, true);
                occupancyGrid.SetCellValue(1, 2, true);
                occupancyGrid.SetCellValue(2, 2, true);
            }
            // If no entrance from the north, add a top wall
            if (north)
            {
                occupancyGrid.SetCellValue(2, 0, true);
                occupancyGrid.SetCellValue(2, 1, true);
                occupancyGrid.SetCellValue(2, 2, true);
            }
            return occupancyGrid;
        }

        private static GridGraph<bool> CreateStampClosed3x3(Direction dir)
        {
            var backingStore = new GridNodeDataStore<bool>(3, 3);
            var occupancyGrid = new GridGraph<bool>(3, 3, backingStore);
            bool any = (dir != Direction.Blank);
            bool west = (dir & Direction.West) == Direction.West;
            bool north = (dir & Direction.North) == Direction.North;
            bool east = (dir & Direction.East) == Direction.East;
            bool south = (dir & Direction.South) == Direction.South;
            // The following lines that set the grid to false are not needed, as it is false by default with C#.
            occupancyGrid.SetCellValue(0, 0, false);
            occupancyGrid.SetCellValue(0, 1, south);
            occupancyGrid.SetCellValue(0, 2, false);
            occupancyGrid.SetCellValue(1, 0, west);
            occupancyGrid.SetCellValue(1, 1, any);
            occupancyGrid.SetCellValue(1, 2, east);
            occupancyGrid.SetCellValue(2, 0, false);
            occupancyGrid.SetCellValue(2, 1, north);
            occupancyGrid.SetCellValue(2, 2, false);
            return occupancyGrid;
        }

        private static GridGraph<bool> CreateStampClosed2x2(Direction dir)
        {
            var backingStore = new GridNodeDataStore<bool>(2,2);
            var occupancyGrid = new GridGraph<bool>(2,2, backingStore);
            bool any = (dir != Direction.Blank);
            bool west = (dir & Direction.West) == Direction.West;
            bool north = (dir & Direction.North) == Direction.North;
            occupancyGrid.SetCellValue(0, 0, west);
            occupancyGrid.SetCellValue(0, 1, any);
            occupancyGrid.SetCellValue(1, 1, north);
            return occupancyGrid;
        }
    }
}
