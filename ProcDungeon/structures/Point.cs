using System;
using System.Diagnostics.CodeAnalysis;

namespace ProcDungeon.Structures
{   
    /*<summary>Class <c>Point</c>
    represents a single point or location
    <param name="x">the x coordinate</param>
    <param name="y">the y coordinate</param>
    </summary>
    */
    public class Point: IEquatable<Point>, IComparable<Point>
    {
        public int x { get; }
        public int y { get; }

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"Point({this.x}, {this.y})";
        }

        public static Point operator+ (Point a, Point b) => new Point(a.x + b.x, a.y + b.y);
        public static bool operator< (Point a, Point b)
        {
            int dx = b.x - a.x;
            int dy = b.y - a.y;
            int d = (int)Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
            return d < 0;
        }
        public static bool operator> (Point a, Point b)
        {
            int dx = b.x - a.x;
            int dy = b.y - a.y;
            int d = (int)Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
            return d > 0;
        }
        public static bool operator== (Point a, Point b) => (a.x == b.x && a.y == b.y);
        public static bool operator!= (Point a, Point b) => !(a==b);
    
        public bool Equals(Point p)
        {   
            if (p is null || GetType() != p.GetType()) return false;
            return p == this;
        }

        public override bool Equals(object obj) => this.Equals(obj);
        
        public override int GetHashCode() => x.GetHashCode() ^ y.GetHashCode();

        public int CompareTo([AllowNull] Point other)
        {
            if (other is null) return -1;
            return (this.x + this.y) - (other.x + other.y);
        }
    }
}