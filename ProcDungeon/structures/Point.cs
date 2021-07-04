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
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override string ToString()
        {
            return $"Point({this.X}, {this.Y})";
        }

        public static Point operator+ (Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);
        public static bool operator< (Point a, Point b)
        {
            int dx = b.X - a.X;
            int dy = b.Y - a.Y;
            int d = (int)Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
            return d < 0;
        }
        public static bool operator> (Point a, Point b)
        {
            int dx = b.X - a.X;
            int dy = b.Y - a.Y;
            int d = (int)Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
            return d > 0;
        }
        public static bool operator== (Point a, Point b) => (a.X == b.X && a.Y == b.Y);
        public static bool operator!= (Point a, Point b) => !(a==b);
    
        public bool Equals(Point p)
        {   
            if (p is null || GetType() != p.GetType()) return false;
            return p == this;
        }

        public override bool Equals(object obj) => this.Equals(obj);
        
        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();

        public int CompareTo([AllowNull] Point other)
        {
            if (other is null) return -1;
            return (this.X + this.Y) - (other.X + other.Y);
        }
    }
}