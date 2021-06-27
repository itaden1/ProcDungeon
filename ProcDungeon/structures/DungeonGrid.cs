using System;
using System.Collections.Generic;
using ProcDungeon.Algorythms;
using ProcDungeon.Interfaces;
using ProcDungeon.Structures;

namespace ProcDungeon
{
	public class DungeonGrid<T> where T : ITileNode, new()
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

        internal void ClearArea(Rectangle rect)
        {
			for (int y = rect.y; y < rect.y + rect.height; y++)
			{
				for (int x = rect.x; x < rect.x + rect.width; x++)
				{
					Grid[y, x].Blocking = false;
				}
			}
        }
    } 
}
