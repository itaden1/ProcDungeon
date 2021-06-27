namespace ProcDungeon.Structures
{
    public struct Rectangle
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

		public override string ToString() => $"({x},{y})";
    }
}