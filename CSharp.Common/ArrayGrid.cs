using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode.CSharp.Common
{
    public class ArrayGrid<TCell> : IGrid<TCell>
    {
        //public static ArrayGrid<char> Create(IList<string> lines)
        //{
        //    var ySize = lines.Count;
        //    var xSize = lines.First().Length;
        //    var grid = lines.SelectMany(l => l.Select(c => c)).ToArray();

        //    return new ArrayGrid<char>(grid, xSize, ySize);
        //}

        //public static ArrayGrid<TCell> Create<TCellT>(IList<string> lines, Func<char, TCell> transformMethod)
        //{
        //    var ySize = lines.Count;
        //    var xSize = lines.First().Length;
        //    var grid = lines.SelectMany(l => l.Select(transformMethod)).ToArray();

        //    return new ArrayGrid<TCell>(grid, xSize, ySize);
        //}



        private TCell[] _grid;
        private int _xSize;
        private int _ySize;

        //public ArrayGrid(IList<string> lines) : this(lines, c => c)
        //{
        //    //_ySize = lines.Count;
        //    //_xSize = lines.First().Length;
        //    //_grid = lines.SelectMany(l => l.Select(c => c)).ToArray();
        //}
        public ArrayGrid(IList<string> lines, Func<char, TCell> transformMethod)
        {
            _ySize = lines.Count;
            _xSize = lines.First().Length;
            _grid = lines.SelectMany(l => l.Select(transformMethod)).ToArray();
        }
        private ArrayGrid(TCell[] grid, int xSize, int ySize)
        {
            _grid = grid;
            _xSize = xSize;
            _ySize = _grid.Length / xSize;
        }



        public int Count => _grid.Length;
        public IEnumerable<TCell> Grid => _grid;



        public bool IsInBounds(Point point)
        {
            return 0 <= point.X && point.X < _xSize
                && 0 <= point.Y && point.Y < _ySize;
        }

        //public (int x, int y) GetXY(int index)
        //{

        //}

        public Point IndexToPoint(int index)
        {
            var x = index % _xSize;
            var y = index / _xSize;

            return new Point(x, y);
        }
        public int PointToIndex(Point point)
        {
            var x = point.X;
            var y = point.Y;

            var index = y * _xSize + x;
            return index;
        }

        public TCell GetCell(int index)
        {
            var point = IndexToPoint(index);
            if (!IsInBounds(point))
                throw new IndexOutOfRangeException();
            return _grid[index];
        }
        public TCell GetCell(Point point)
        {
            if (!IsInBounds(point))
                throw new IndexOutOfRangeException();
            var index = PointToIndex(point);
            return GetCell(index);
        }

    }
}
