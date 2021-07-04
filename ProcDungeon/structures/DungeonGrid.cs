using System;
using System.Collections.Generic;
using System.Text;
using ProcDungeon.Algorythms;
using ProcDungeon.Interfaces;
using ProcDungeon.Structures;

namespace ProcDungeon
{
	public class DungeonGrid<T> where T : BaseTile, new()
	{
		private T[,] _grid;
		public T[,] Grid => _grid;
		public DungeonGrid(int size)
		{
			_grid = new T[size,size];
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					_grid[j,i] = new T();
				}
			}
		}

        internal void ClearArea(Rectangle rect, byte symbol)
        {
			for (int y = rect.Y; y < rect.Y + rect.Height; y++)
			{
				for (int x = rect.X; x < rect.X + rect.Width; x++)
				{
					Grid[y, x].Blocking = false;
					Grid[y, x].BitSymbol = symbol;
				}
			}
        }

		public DNode[,] GenerateBasicNodeGrid(int size)
		{
			var grid = new DNode[size,size];
			for (int y = 0; y < size; y++)
			{
				for (int x = 0; x < size; x++)
				{
					grid[y,x] = new DNode(y >> x);
				}
			}
			return grid;
		}

		public override string ToString()
		{
			// Output the grid where # is bloking and ' ' is not
			var stringBuilder = new StringBuilder("\n");
			for (int y = 0; y < Grid.GetLength(0); y++)
			{
				for (int x = 0; x < Grid.GetLength(1); x++)
				{
					if (Grid[y,x].BitSymbol == 0) stringBuilder.Append("# ");
					else if (Grid[y,x].BitSymbol == 2) stringBuilder.Append(". ");
					else stringBuilder.Append("  ");
				}
				stringBuilder.Append("\n");
			}
			return stringBuilder.ToString();
		}
    } 
}
