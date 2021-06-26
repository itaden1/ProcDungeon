using System;
using System.Collections.Generic;
using System.Linq;
using ProcDungeon.Interfaces;
using ProcDungeon.Structures;

namespace ProcDungeon.Algorythms
{
    public class BSPDungeonAlgorythm : IGenerationAlgorythm
    {
        private Random _random = new Random();
        private List<Rectangle> _rooms = new List<Rectangle>();
        public List<Rectangle> Rooms => _rooms;

        public DungeonGrid<Tile> Generate(DungeonGrid<Tile> grid, DungeonGraph graph)
        {
            var canvas = grid.Grid;
            BSPNode BSPTree = new BSPNode(0, canvas.GetLength(0), 0, canvas.GetLength(1));
            BSPTree.Partition(graph.NodeCount);

            var rects = new List<Rectangle>();


            // create entrance point/'s
            var start = new Point(canvas.GetLength(1) / 2, 0);
            var leaf = BSPTree.GetLeafeFromPoint(start);
            var node = graph.Nodes[0];
            var rect = new Rectangle()
            {
                x = leaf.LeftEdge + 1,
                y = leaf.TopEdge + 1,
                width = (leaf.RightEdge - leaf.LeftEdge) - 2,
                height = (leaf.BottomEdge - leaf.TopEdge) - 2
            };
            grid.ClearArea(rect);
            rects.Add(rect);

            var leafQueue = new Queue<BSPNode>();
            var nodeQueue = new Queue<DNode>();
            foreach(BSPNode l in BSPTree.GetNeighbouringLeaves(leaf))
            {
                leafQueue.Enqueue(l);
            }

            foreach(DEdge e in node.Edges)
            {
                if (e.NodeFrom == node)
                {
                    nodeQueue.Enqueue(e.NodeTo);
                }
                else
                {
                    nodeQueue.Enqueue(e.NodeFrom);
                }   
            }


            var processedLeaves = new List<BSPNode>();
            var processedNodes = new List<DNode>();
            processedLeaves.Add(leaf);
            processedNodes.Add(node);

            var emergencyBreak = 100;
            while(nodeQueue.Count > 0)
            {   
                emergencyBreak--;
                if(emergencyBreak <= 0) break;

                leaf = leafQueue.Dequeue();
                node = nodeQueue.Dequeue();
                rect = new Rectangle()
                {
                    x = leaf.LeftEdge + 1,
                    y = leaf.TopEdge + 1,
                    width = (leaf.RightEdge - leaf.LeftEdge) - 2,
                    height = (leaf.BottomEdge - leaf.TopEdge) - 2
                };

                // place it on the grid
                grid.ClearArea(rect);

                rects.Add(rect);

                // Get neighbouring leaves from BSPTree
                var leafList = BSPTree.GetNeighbouringLeaves(leaf);
         
 
                foreach(DEdge e in node.Edges)
                {
           
                    leafQueue.Enqueue(leafList[0]);
                    leafList.RemoveAt(0);
                
                    Console.WriteLine($"node({node}) has {node.Edges.Count} edges");
                    Console.WriteLine($"{e.NodeFrom}--->{e.NodeTo}");
                    if (e.NodeFrom == node && !(processedNodes.Contains(e.NodeTo)))
                    {
                        Console.WriteLine($"queuing {e.NodeTo}");
                        nodeQueue.Enqueue(e.NodeTo);
                    }
                    else if (!(processedNodes.Contains(e.NodeFrom)))
                    {
                        Console.WriteLine($"queuing {e.NodeFrom}");
                        nodeQueue.Enqueue(e.NodeFrom);
                    }
         
                }
                processedLeaves.Add(leaf);
                processedNodes.Add(node);
            }
            _rooms.AddRange(rects);
            return grid;
        }
    }
}