using System;
using ProcDungeon;
using ProcDungeon.Structures;
using Xunit;
using Xunit.Abstractions;

namespace ProcDungeon.Tests
{
    public class TestBSPTree
    {
        private readonly ITestOutputHelper output;

        [Fact]
        public void TestSinglePartitioningCreatesLeaves()
        {
            var BSPTree = new BSPNode(0, 10, 0, 10);
            BSPTree.Partition(1, 4);
            Assert.True(BSPTree.Branch1.IsLeaf);
            Assert.True(BSPTree.Branch2.IsLeaf);
        }
        [Fact]
        public void Test2PartitionsReturns4Leaves()
        {
            var BSPTree = new BSPNode(0, 50, 0, 50);
            BSPTree.Partition(2, 10);
            Assert.Equal(4, BSPTree.Leaves.Count);
        }
        [Fact]
        public void TestLeafCountGreatorEqualGraphSize()
        {
            var graph = new DungeonGraph();
            for (int i = 0; i <= 5; i++)
            {
                var n = new DNode(i);
                graph.AddNode(n);
            }

            var BSPTree = new BSPNode(0, 50, 0, 50);
            BSPTree.Partition(graph.NodeCount/2, 10);
            Assert.True(BSPTree.Leaves.Count >= graph.NodeCount);
        }
    }
}