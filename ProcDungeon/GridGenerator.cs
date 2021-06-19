using System;
using System.Collections.Generic;
using ProcDungeon.Interfaces;
using ProcDungeon.Structures;

namespace ProcDungeon
{

	public struct Rectangle
	{
		public int x;
		public int y;
		public int width;
		public int height;

		public bool OverlapsWith(Rectangle other)
		{
			return !(other.x > x + width || other.y > y + height || other.x + other.width < x || other.y + other.height < y);   
		}

		public override string ToString() => $"({x},{y})";
	} 

	 public class GridGenerator
	{
		private Random _random = new Random();

		public T[,] GenerateGrid<T>(T ob, int size) where T : INode, new()
		{
			T[,] grid = new T[size,size];
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					grid[j,i] = new T();
				}
			}
			return grid;
		}
		public Tile[,] GenerateTileGrid(int size)
		{
			Tile[,] grid = new Tile[size, size];
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					grid[j,i] = new Tile(i, j, "void", "#222222");
				}
			}
			return grid;
		}
		public DNode[,] GenerateNodeGrid(int size)
		{
			DNode[,] grid = new DNode[size, size];
			for (int i = 0; i < size * size; i++)
			{
				grid[i / size , i % size] = new DNode(i + 1);
			}
			return grid;
		}
		public Tile[,] GenerateMapGrid(DungeonGraph graph, int size, char[] exits)
		{

			Tile[,] map = GenerateTileGrid(size);

			BSPNode BSPTree = new BSPNode(0, map.GetLength(0), 0,  map.GetLength(1));
			BSPTree.Partition(graph.NodeCount/2, 4);

			List<BSPNode> leafNodes = BSPTree.Leaves;
			var rects = new List<Rectangle>();

			foreach (DNode node in graph.Nodes)
			{
				Rectangle rect;
				while (true)
				{
					var p = new Point(_random.Next(1, size - 6), _random.Next(1, size - 6));
					int w = _random.Next(2, 6);
					int h = _random.Next(2, 6);
					
					// Create a rect
					rect = new Rectangle(){x=p.x, y=p.y, width=w, height=h};

					// make sure its inside bounds and does not overlap with previouse rects
					bool validPlacement = true;
					if (rects.Count > 0)
					{
						foreach(Rectangle r in rects)
						{
							if (rect.OverlapsWith(r)) validPlacement = false;
						}
					}
					if (validPlacement) break;
					else continue;
				}
		
				// place it on the grid
				for (int y = rect.y; y < rect.y + rect.height; y++)
				{
					for (int x = rect.x; x < rect.x + rect.width; x++)
					{
						map[y,x].Colour = "#00dd00";
					}
				}
				rects.Add(rect);
			}

			return map;
		}
	}   
}
