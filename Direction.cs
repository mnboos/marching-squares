using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarchingSquares
{
    public class Direction
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public string Description { get; private set; }

        public override string ToString() => Description;
        

        public static Direction Up = new Direction { X = 0, Y = -1, Description = "Up"};
        public static Direction Right = new Direction { X = 1, Y = 0, Description = "Right"};
        public static Direction Down = new Direction { X = 0, Y = 1, Description = "Down"};
        public static Direction Left = new Direction { X = -1, Y = 0, Description = "Left"};
    }
}
