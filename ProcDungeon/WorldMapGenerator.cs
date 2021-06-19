using System.Collections.Generic;
using System.Text;
using ProcDungeon.Interfaces;
using ProcDungeon.Structures;

namespace ProcDungeon
{

	public class DWorld
	{
		public Tile[,] Map {get; set; }
		public DungeonGraph Graph {get; set; }

		public DWorld(IWorldGenerator generator)
		{
			generator.Create();
			Map = generator.GetGridMap();
			Graph = generator.GetGraph(); 
		}
	}

	//<summary>Class <c>GridGenerator</c>
	//Genrate a simple grid of width and height
	public class StandardMapGenerator : IWorldGenerator
	{
		private int _mapSize;
		public DungeonGraph Graph;
		public Tile[,] TileMap;
		public StandardMapGenerator(int size) : base()
		{
			_mapSize = size;
		}
		public void Create()
		{
			var gridGen = new GridGenerator();
			var graphGen = new MapGraphGenerator();

			DNode[,] nodes = gridGen.GenerateNodeGrid(_mapSize);
			DungeonGraph graph = graphGen.GenerateGraphFromGrid(10, nodes);
			Tile[,] worldMap = gridGen.GenerateMapGrid(graph, 100, new char[]{'N','S'});

			Graph = graph;
			TileMap = worldMap;
		}
		public Tile[,] GetGridMap() => TileMap;
		public DungeonGraph GetGraph() => Graph;
	}
}
