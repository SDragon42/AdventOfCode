using System.Collections.Generic;
using System.Drawing;

namespace AdventOfCode.CSharp.Common
{
    public interface IGrid<TCell>
    {
        int Count { get; }
        IEnumerable<TCell> Grid { get; }

        bool IsInBounds(Point point);

        Point IndexToPoint(int index);
        int PointToIndex(Point point);

        TCell GetCell(int index);
        TCell GetCell(Point point);
    }
}
