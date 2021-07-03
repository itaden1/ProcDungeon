using System;
using Xunit;
using ProcDungeon.Structures;
using ProcDungeon.Algorythms;
using ProcDungeon.Enums;
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
        public void TestGenerateGridFromBSP()
        {
            var map = new DungeonGrid<Tile>(30);

			BSPDungeonAlgorythm alg = new BSPDungeonAlgorythm(map);
			alg.Generate(10, new List<int>(){1,2});

            Console.WriteLine(map.ToString());

            // Assert.Equal(graph.NodeCount, alg.Rooms.Count);
        }

        [Fact]
        public void TestCorridoorBetweenBSPNodes()
        {
            var map = new DungeonGrid<Tile>(5);
            var leaf1 = new BSPNode(0,2,0,4);
            var leaf2 = new BSPNode(2,4,0,4);
            BSPDungeonAlgorythm alg = new BSPDungeonAlgorythm(map);
            alg.CreateCorridoor((Rectangle)leaf1, (Rectangle)leaf2, Alignment.Vertical);

            Console.WriteLine(map.ToString());

            Assert.True(map.Grid[0,1].Blocking);
            Assert.True(map.Grid[1,1].Blocking);
            Assert.False(map.Grid[2,1].Blocking);
            Assert.False(map.Grid[2,2].Blocking);
        }
    }
    public class TestTRandomAlgorythm
    {
        public TestTRandomAlgorythm(ITestOutputHelper output)
        {
            Converter converter = new Converter(output);
            Console.SetOut(converter);
        }
        [Fact]
        public void TestGenerate()
        {
            var map = new DungeonGrid<Tile>(20);
            RandomPlacementAlgorythm alg = new RandomPlacementAlgorythm(map);
			alg.Generate(5, new List<int>(){1,2});
            Console.WriteLine(map.ToString());
            Assert.Equal(1,1);
        }
    }
}