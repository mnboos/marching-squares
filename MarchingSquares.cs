using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.Remoting.Messaging;

namespace MarchingSquares
{
    public class MarchingSquares
    {
        private Bitmap _bmp;
        private int[,] _cellStates;
        private Point? _startPoint = null;

        public void LoadImage()
        {
            //var test = new Bitmap(@"D:\Projects\MarchingSquares\MarchingSquares\rgb.png");

            //var _bmp = new Bitmap(@"D:\Projects\MarchingSquares\MarchingSquares\large.png");
            //_bmp = new Bitmap(@"D:\Projects\MarchingSquares\MarchingSquares\sprite.png");
            _bmp = new Bitmap(@"D:\Projects\MarchingSquares\MarchingSquares\test.png");

            #region Testdata
            //_bmp = new Bitmap(@"D:\Projects\MarchingSquares\MarchingSquares\p0.png");
            //CalculateStates();

            //_bmp = new Bitmap(@"D:\Projects\MarchingSquares\MarchingSquares\p1.png");
            //CalculateStates();

            //_bmp = new Bitmap(@"D:\Projects\MarchingSquares\MarchingSquares\p2.png");
            //CalculateStates();

            //_bmp = new Bitmap(@"D:\Projects\MarchingSquares\MarchingSquares\p3.png");
            //CalculateStates();

            //_bmp = new Bitmap(@"D:\Projects\MarchingSquares\MarchingSquares\p4.png");
            //CalculateStates();

            //_bmp = new Bitmap(@"D:\Projects\MarchingSquares\MarchingSquares\p5.png");
            //CalculateStates();

            //_bmp = new Bitmap(@"D:\Projects\MarchingSquares\MarchingSquares\p6.png");
            //CalculateStates();

            //_bmp = new Bitmap(@"D:\Projects\MarchingSquares\MarchingSquares\p7.png");
            //CalculateStates();

            //_bmp = new Bitmap(@"D:\Projects\MarchingSquares\MarchingSquares\p8.png");
            //CalculateStates();

            //_bmp = new Bitmap(@"D:\Projects\MarchingSquares\MarchingSquares\p9.png");
            //CalculateStates();

            //_bmp = new Bitmap(@"D:\Projects\MarchingSquares\MarchingSquares\p10.png");
            //CalculateStates();

            //_bmp = new Bitmap(@"D:\Projects\MarchingSquares\MarchingSquares\p11.png");
            //CalculateStates();

            //_bmp = new Bitmap(@"D:\Projects\MarchingSquares\MarchingSquares\p12.png");
            //CalculateStates();

            //_bmp = new Bitmap(@"D:\Projects\MarchingSquares\MarchingSquares\p13.png");
            //CalculateStates();

            //_bmp = new Bitmap(@"D:\Projects\MarchingSquares\MarchingSquares\p14.png");
            //CalculateStates();

            //_bmp = new Bitmap(@"D:\Projects\MarchingSquares\MarchingSquares\p15.png");
            //CalculateStates();
            #endregion

            CalculateStates();
            March();
        }

        private Direction GetNextDirection(int state, Direction previousDirection)
        {
            switch (state)
            {
                case 1: return Direction.Down;
                case 2: return Direction.Right;
                case 3: return Direction.Right;
                case 4: return Direction.Up;
                case 5: return Direction.Down;
                case 6: return Direction.Up;
                case 7: return Direction.Up;
                case 8: return Direction.Left;
                case 9: return Direction.Down;
                case 10: return Direction.Up;
                case 11: return Direction.Right;
                case 12: return Direction.Left;
                case 13: return Direction.Down;
                case 14: return Direction.Left;

                default: throw new Exception("Illegal state: "+state);
            }
        }

        private void CalculateStates()
        {
            _cellStates = new int[_bmp.Width - 1, _bmp.Height - 1];
            for (int y = 0; y < _bmp.Height - 1; y++)
            {
                for (int x = 0; x < _bmp.Width - 1; x++)
                {
                    int topLeft = _bmp.GetPixel(x, y).R == 0 ? 1 : 0;
                    int topRight = _bmp.GetPixel(x + 1, y).R == 0 ? 1 : 0;
                    int bottomRight = _bmp.GetPixel(x + 1, y + 1).R == 0 ? 1 : 0;
                    int bottomLeft = _bmp.GetPixel(x, y + 1).R == 0 ? 1 : 0;

                    int cellState = (topLeft << 3) | (topRight << 2) | (bottomRight << 1) | bottomLeft;

                    _cellStates[x, y] = cellState;
                    //Console.Write(_cellStates[x,y].ToString("D2"));
                    //Debug.Write(_cellStates[x,y].ToString("D2"));

                    if (_startPoint == null && cellState > 0 && cellState < 15)
                    {
                        _startPoint = new Point(x, y);
                    }
                }
                //Console.WriteLine("");
                //Debug.WriteLine("");
            }
        }

        private void March()
        {
            if (!_startPoint.HasValue)
                throw new Exception("Nothing to contour");

            var points = new List<Point>();
            Point currentPoint = _startPoint.Value;
            Direction direction = GetNextDirection(GetState(currentPoint), null);
            do
            {
                points.Add(new Point(currentPoint.X, currentPoint.Y));

                currentPoint.X += direction.X;
                currentPoint.Y += direction.Y;

                var state = GetState(currentPoint);
                direction = GetNextDirection(state, direction);
            } while (currentPoint.X != _startPoint.Value.X || currentPoint.Y != _startPoint.Value.Y);

            CreateVectors(points);
        }

        private void CreateVectors(IList<Point> points)
        {
            var vectors = new List<Point>();

            while (points.Any())
            {
                var tempPoints = points
                    .TakeWhile((p, i) => i == 0 
                    || points.Take(i).All(precedingPoint => precedingPoint.X == p.X)
                    || points.Take(i).All(precedingPoint => precedingPoint.Y == p.Y))
                    .ToList();
                tempPoints.ForEach(p => points.Remove(p));

                Debug.Assert(tempPoints.Any());

                if (tempPoints.Count == 1)
                {
                    vectors.Add(tempPoints[0]);
                }
                else
                {
                    vectors.Add(tempPoints.First());
                    vectors.Add(tempPoints.Last());
                }
            }
        }

        private int GetState(Point point)
        {
            return _cellStates[point.X, point.Y];
        }
    }
}
