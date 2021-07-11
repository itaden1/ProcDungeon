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
            BSPDungeonAlgorythm alg = new BSPDungeonAlgorythm(100);
            var map = new DungeonGrid(30, alg);
            map.GenerateLayout();

            Console.WriteLine(map.ToString());
            Assert.True(1 == 2);
        }

        [Fact]
        public void TestCorridoorBetweenBSPNodes()
        {
            BSPDungeonAlgorythm alg = new BSPDungeonAlgorythm(2);

            var map = new DungeonGrid(5, alg);
            var leaf1 = new BSPNode(0,2,0,4);
            var leaf2 = new BSPNode(2,4,0,4);
            alg.GetWayPoints((Rectangle)leaf1, (Rectangle)leaf2, Alignment.Vertical);

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
            RandomPlacementAlgorythm alg = new RandomPlacementAlgorythm(5);
            var map = new DungeonGrid(20,  alg);
			map.GenerateLayout();
            Console.WriteLine(map.ToString());
            Assert.Equal(1,1);
        }
    }
}