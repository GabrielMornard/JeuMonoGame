using System;

namespace Game_1
{
    internal class Direction
    {
        public static readonly Direction None = new Direction(0, 0, "None");
        public static readonly Direction Left = new Direction(-1, 0, "Left");
        public static readonly Direction Right = new Direction(1, 0, "Right");
        public static readonly Direction Up = new Direction(0, -1, "Up");
        public static readonly Direction Down = new Direction(0, 1, "Down");

        public int X { get; private set; }
        public int Y { get; private set; }
        public string Name { get; private set; }

        private Direction(int x, int y, string name)
        {
            X = x;
            Y = y;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
