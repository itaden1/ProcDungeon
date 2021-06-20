using System;
using System.Collections.Generic;
using System.Linq;
using ProcDungeon.Interfaces;
using ProcDungeon.Structures;

namespace ProcDungeon.Algorythms
{
    public class BSPDungeonAlgorythm<T> : IGenerationAlgorythm<T>
    {
        private Random _random = new Random();
        private List<Rectangle> _rooms = new List<Rectangle>();
        public List<Rectangle> Rooms  => _rooms;

        public T[,] Generate<T>(T[,] canvas, DungeonGraph graph) 
            where T : ITileNode
        {
            BSPNode BSPTree = new BSPNode(0, canvas.GetLength(0), 0,  canvas.GetLength(1));
			BSPTree.Partition(graph.NodeCount/2, 4);
            var rects = new List<Rectangle>();

            var start = new Point(canvas.GetLength(1) / 2, 0);

            // Select an edge node from the BSPTree
            // TODO: first node is always on the top... change this to be dynamic
			List<BSPNode> leafNodes = BSPTree.Leaves;
            IEnumerable<BSPNode> query = from n in leafNodes
                                        where n.TopEdge == 0 
                                        where n.LeftEdge <= start.x
                                        where n.RightEdge >= start.x
                                        select n;
            var firstLeaf = query.First();

            var leafQueue = new Queue<BSPNode>();
            leafQueue.Enqueue(firstLeaf);

            var processedLeaves = new List<BSPNode>();


            while (processedLeaves.Count > 0)
			{
                
                var leaf = leafQueue.Dequeue(); 
                var rect = new Rectangle(){
                    x = leaf.LeftEdge + 1,
                    y = leaf.TopEdge + 1,
                    width = (leaf.RightEdge - leaf.LeftEdge) - 1,
                    height = (leaf.BottomEdge - leaf.TopEdge) - 1
                };
		
				// place it on the grid
				for (int y = rect.y; y < rect.y + rect.height; y++)
				{
					for (int x = rect.x; x < rect.x + rect.width; x++)
					{
						canvas[y,x].Blocking = false;
					}
				}
				rects.Add(rect);

                // Get neighbouring leaves from BSPTree
                IEnumerable<BSPNode> leafQuery = from n in leafNodes
                                    where n.RightEdge == leaf.LeftEdge ||
                                    n.TopEdge == leaf.BottomEdge ||
                                    n.LeftEdge == leaf.RightEdge ||
                                    n.BottomEdge == leaf.TopEdge &&
                                    !processedLeaves.Contains(n) 
                                    select n;
                foreach(BSPNode n in leafQuery)
                {
                    leafQueue.Enqueue(n);
                }
                processedLeaves.Add(leaf);
			}
            _rooms.AddRange(rects);
			return canvas;
        }
    }
}