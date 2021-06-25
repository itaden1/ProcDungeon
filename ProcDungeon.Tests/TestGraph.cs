using System;
using ProcDungeon;
using ProcDungeon.Structures;
using Xunit;
using Xunit.Abstractions;

namespace ProcDungeon.Tests
{
    public class TestGraph
    {
        public TestGraph(ITestOutputHelper output)
        {
            Converter converter = new Converter(output);
            Console.SetOut(converter);
        }
        [Fact]
        public void TestAddNodeToGraph()
        {
            DungeonGraph graph = new DungeonGraph();
            DNode node = new DNode(1);
            graph.AddNode(node);
            Assert.Contains(node, graph.Nodes);
        }
        [Fact]
        public void TestAddEdgeToNode()
        {
            DNode node1 = new DNode(1);
            DNode node2 = new DNode(2);
            DEdge edge = new DEdge(node1, node2);
            node1.AddEdge(edge);
            Assert.True(node1.HasEdgeToNode(node2));
            Assert.False(node2.HasEdgeToNode(node1));
        }
    }
}