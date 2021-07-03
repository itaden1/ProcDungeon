namespace ProcDungeon.Structures
{
    public class Rectangle
    {
		public int x;
		public int y;
		public int width;
		public int height;

		public int  ex => x + width;
		public int ey => y + height;
		public bool OverlapsWith(Rectangle other)
		{
			return !(other.x > x + width || other.y > y + height || other.x + other.width < x || other.y + other.height < y);   
		}

		public override string ToString() => $"x:{x}, y:{y}, w:{width}, h:{height}";
		public static explicit operator Rectangle(BSPNode n)
		{
			return new Rectangle(){
				x = n.LeftEdge,
				y = n.TopEdge,
				width = n.RightEdge - n.LeftEdge,
				height = n.BottomEdge - n.TopEdge
			};
		}
		public static implicit operator BSPNode(Rectangle r)
		{
			return new BSPNode(r.x, r.x + r.width, r.y, r.y + r.height);
		}
    }

}