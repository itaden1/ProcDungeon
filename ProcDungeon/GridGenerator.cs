using System;
using System.Collections.Generic;
using ProcDungeon.Algorythms;
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

		public T[,] GenerateGrid<T>(T ob, int size) where T : ITileNode, new()
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
		public static Tile[,] GenerateTileGrid(int size)
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
			BSPDungeonAlgorythm<Tile> alg = new BSPDungeonAlgorythm<Tile>();
			Tile[,] tileMap = alg.Generate<Tile>(map, graph);

			return map;
		}
	}   
}
