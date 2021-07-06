using System;
using ProcDungeon.Structures;
using Xunit;
using Xunit.Abstractions;

namespace ProcDungeon.Tests
{
    public class TestBSPTree
    {
        public TestBSPTree(ITestOutputHelper output)
        {
            Converter converter = new Converter(output);
            Console.SetOut(converter);
        }
        [Fact]
        public void TestSingleIterationCreates1Leave()
        {
            var BSPTree = new BSPNode(0, 10, 0, 10);
            BSPTree.Partition(1);
            Assert.Equal(1, BSPTree.Leaves.Count);
        }

        [Fact]
        public void Test2IterationsReturns2Leaves()
        {
            var BSPTree = new BSPNode(0, 50, 0, 50);
            BSPTree.Partition(2);
            Assert.Equal(2, BSPTree.Leaves.Count);
        }

        [Fact]
        public void Test3IterationsReturns3Leaves()
        {
            var BSPTree = new BSPNode(0, 50, 0, 50);
            BSPTree.Partition(3);
            Assert.Equal(3, BSPTree.Leaves.Count);
        }
        
        [Fact]
        public void Test5IterationsReturns5Leaves()
        {
            var BSPTree = new BSPNode(0, 50, 0, 50);
            BSPTree.Partition(5);
            Assert.Equal(5, BSPTree.Leaves.Count);
        }
        [Fact]
        public void TestLeafCountGreatorEqualGraphSize()
        {
            var graph = new DungeonGraph();
            for (int i = 0; i <= 8; i++)
            {
                var n = new DNode(i);
                graph.AddNode(n);
            }

            var BSPTree = new BSPNode(0, 50, 0, 50);
            BSPTree.Partition(graph.NodeCount);
            Assert.Equal(graph.NodeCount, BSPTree.Leaves.Count);
        }
    }
}