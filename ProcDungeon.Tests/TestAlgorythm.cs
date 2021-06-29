using System;
using Xunit;
using ProcDungeon.Structures;
using ProcDungeon.Algorythms;
using Xunit.Abstractions;
using System.Collections.Generic;

namespace ProcDungeon.Tests
{

    public class TestBSPAlgorythm
    {
        public TestBSPAlgorythm(ITestOutputHelper output)
        {
            Converter converter = new Converter(output);
            Console.SetOut(converter);
        }

        [Fact]
        public void TestGenerateFromBasicGraph()
        {
            var map = new DungeonGrid<Tile>(80);
            var graph = new DungeonGraph();
            var n1 = new DNode(1);
            var n2 = new DNode(2);
            var n3 = new DNode(3);
            var n4 = new DNode(4);
            var n5 = new DNode(5);
            var n6 = new DNode(6);


            var e1 = new DEdge(n1, n2);
            var e2 = new DEdge(n2, n1);
            var e3 = new DEdge(n2, n3);
            var e4 = new DEdge(n3, n4);
            var e5 = new DEdge(n4, n5);
            var e6 = new DEdge(n5, n6);

            n1.AddEdge(e1);
            // n2.AddEdge(e2);
            n2.AddEdge(e3);
            n3.AddEdge(e4);
            n4.AddEdge(e5);
            n5.AddEdge(e6);

            graph.AddNodes(new List<DNode>{n1, n2, n3, n4, n5, n6});

			BSPDungeonAlgorythm alg = new BSPDungeonAlgorythm(map);
			alg.Generate(graph);

            Console.WriteLine(map.ToString());

            Assert.Equal(graph.NodeCount, alg.Rooms.Count);
        }

        [Fact]
        public void TestGenerateFromComplexGraph()
        {
            var map = new DungeonGrid<Tile>(10);
            var grid = map.GenerateBasicNodeGrid(5);
            var graphGen = new MapGraphGenerator();
            var graph = graphGen.GenerateGraphFromGrid(5, grid);
            BSPDungeonAlgorythm alg = new BSPDungeonAlgorythm(map);
            alg.Generate(graph);
            Console.WriteLine(map);
        }

        [Fact]
        public void TestCorridoorBetweenBSPNodes()
        {
            var map = new DungeonGrid<Tile>(5);
            var leaf1 = new BSPNode(0,2,0,4);
            var leaf2 = new BSPNode(2,4,0,4);
            BSPDungeonAlgorythm alg = new BSPDungeonAlgorythm(map);
            alg.CreateCorridoor(leaf1, leaf2);

            Assert.True(map.Grid[0,1].Blocking);
            Assert.True(map.Grid[1,1].Blocking);
            Assert.False(map.Grid[2,1].Blocking);
            Assert.False(map.Grid[2,2].Blocking);
        }
    }
    public class TestTRandomAlgorythm
    {
        [Fact]
        public void TestGenerate()
        {
            var map = new DungeonGrid<Tile>(100);
            var graph = new DungeonGraph();
            for (int i = 0; i <= 3; i++)
            {
                var n = new DNode(i);
                graph.AddNode(n);
            }

            RandomPlacementAlgorythm alg = new RandomPlacementAlgorythm(map);
			alg.Generate(graph);
            Assert.Equal(graph.NodeCount, alg.Rooms.Count);
        }
    }
}