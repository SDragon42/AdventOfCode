using System.Collections.Generic;
using System.Drawing;

namespace AdventOfCode.CSharp.Common
{
    public interface IGrid<TCell>
    {
        int Count { get; }
        IList<TCell> Grid { get; }

        bool IsInBounds(Point point);

        Point IndexToPoint(int index);
        int PointToIndex(Point point);

        TCell this[int i] {  get; set; }
        TCell this[Point pt] {  get; set; }
    }
}
