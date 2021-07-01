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
            var map = new DungeonGrid<Tile>(50);

			BSPDungeonAlgorythm alg = new BSPDungeonAlgorythm(map);
			alg.Generate(4, new List<int>(){1,2});

            Console.WriteLine(map.ToString());

            // Assert.Equal(graph.NodeCount, alg.Rooms.Count);
        }

        [Fact]
        public void TestGenerateFromComplexGraph()
        {
            var map = new DungeonGrid<Tile>(10);
  
            BSPDungeonAlgorythm alg = new BSPDungeonAlgorythm(map);
            alg.Generate(3, new List<int>{1,2});
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
            var map = new DungeonGrid<Tile>(30);
            RandomPlacementAlgorythm alg = new RandomPlacementAlgorythm(map);
			alg.Generate(5, new List<int>(){1,2});
            Console.WriteLine(map.ToString());
            Assert.Equal(1,1);
        }
    }
}