using System.Collections.Generic;
using System.Linq;

namespace ShareefSoftware
{
    public static class MazeQuery
    {
        public static IEnumerable<(int Row, int Column)> DeadEndsNoLinq(this Maze maze)
        {
            for (int row = 0; row < maze.NumberOfRows; row++)
            {
                for (int column = 0; column < maze.NumberOfColumns; column++)
                {
                    Direction direction = maze.GetCellValue(row, column);
                    if (direction == Direction.North || direction == Direction.South || direction == Direction.East || direction == Direction.West)
                    {
                        yield return (row, column);
                    }
                }
            }
        }

        public static IEnumerable<(int Row, int Column)> DeadEnds(this Maze maze)
        {
            //var query = from cell in maze
            //            where (cell.NodeValue == Direction.North || cell.NodeValue == Direction.South || cell.NodeValue == Direction.East || cell.NodeValue == Direction.West)
            //            select (cell.Row, cell.Column);
            var query = from cell in maze
                        where cell.NodeValue.IsDeadEnd()
                        select (cell.Row, cell.Column);
            return query;
        }

        public static IEnumerable<(int Row, int Column)> TJunctions(this Maze maze)
        {
            var query = from cell in maze
                        where cell.NodeValue.IsTJunction()
                        select (cell.Row, cell.Column);
            return query;
        }

        public static IEnumerable<(int Row, int Column)> Straights(this Maze maze)
        {
            var query = from cell in maze
                        where cell.NodeValue.IsStraight()
                        select (cell.Row, cell.Column);
            return query;
        }

        public static IEnumerable<(int Row, int Column)> Turns(this Maze maze)
        {
            var query = from cell in maze
                        where cell.NodeValue.IsTurn()
                        select (cell.Row, cell.Column);
            return query;
        }
        
        public static IEnumerable<(int Row, int Column)> CrossSections(this Maze maze)
        {
            var query = from cell in maze
                        where cell.NodeValue.IsCrossSection()
                        select (cell.Row, cell.Column);
            return query;
        }
    }
}