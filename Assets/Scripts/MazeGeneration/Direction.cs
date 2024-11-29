using System;
using System.Collections.Generic;

namespace ShareefSoftware
{
    [Flags]
    public enum Direction {  Blank = 0, North = 1, South = 2, West = 4, East = 8 };
    
    public static class Directions
    {
        public static readonly List<Direction> DeadEnds = new List<Direction>() { Direction.North, Direction.East, Direction.South, Direction.West };
        public static readonly List<Direction> Turns = new List<Direction>() { Direction.North | Direction.West, Direction.North | Direction.East, Direction.East | Direction.South, Direction.South | Direction.West };
        public static readonly Direction CrossSection = Direction.North | Direction.West | Direction.East | Direction.South;
        public static readonly List<Direction> Straights = new List<Direction>() { Direction.North | Direction.South, Direction.East | Direction.West };
        public static readonly List<Direction> TJunctions = new List<Direction>()
        {
            Direction.North | Direction.West | Direction.South,
            Direction.East | Direction.North | Direction.West,
            Direction.South | Direction.East | Direction.North,
            Direction.West | Direction.South | Direction.East
        };

        public static bool IsCrossSection(this Direction direction) => direction == CrossSection;
        public static bool IsDeadEnd(this Direction direction) => DeadEnds.Contains(direction);
        public static bool IsStraight(this Direction direction) => Straights.Contains(direction);
        public static bool IsTurn(this Direction direction) => Turns.Contains(direction);
        public static bool IsTJunction(this Direction direction) => TJunctions.Contains(direction);
    }
    
}