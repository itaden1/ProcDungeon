namespace ProcDungeon.Structures
{
    public class Rectangle
    {
		public int X;
		public int Y;
		public int Width;
		public int Height;

		public int  EndX => X + Width;
		public int EndY => Y + Height;
		public Point Center => new Point(EndX - Width / 2, EndY - Height / 2);
		public bool OverlapsWith(Rectangle other)
		{
			return !(other.X > X + Width || other.Y > Y + Height || other.X + other.Width < X || other.Y + other.Height < Y);   
		}

		public override string ToString() => $"x:{X}, y:{Y}, w:{Width}, h:{Height}";
		public static explicit operator Rectangle(BSPNode n)
		{
			return new Rectangle(){
				X = n.LeftEdge,
				Y = n.TopEdge,
				Width = n.RightEdge - n.LeftEdge,
				Height = n.BottomEdge - n.TopEdge
			};
		}
		public static implicit operator BSPNode(Rectangle r)
		{
			return new BSPNode(r.X, r.X + r.Width, r.Y, r.Y + r.Height);
		}
    }

}