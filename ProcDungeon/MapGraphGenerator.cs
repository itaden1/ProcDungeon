using System;
using System.Collections.Generic;
using System.Linq;
using ProcDungeon.Structures;

namespace ProcDungeon
{
    //<summary>Class <c>MapGraph</c>
    // Holds the graph structure of the level / world</summary>
    public class MapGraphGenerator
    {
        public Dictionary<int, List<int>> nodes;

        private Random _random = new Random();

        private Dictionary<string, Point> directions = new Dictionary<string, Point>(){
            { "left", new Point(-1, 0) },
            { "right", new Point(1, 0) },
            { "up", new Point(0, -1) },
            { "down", new Point(0, 1) }
        };

        public Dictionary<int, List<int>> GenerateGraphFromGrid(int iterations, List<List<Tile>> grid)
        {
            //find center of grid
            int w = grid[0].Count;
            int h = grid.Count;
            Point center = new Point(w/2, h/2);

            return nodes;
        }
        public DungeonGraph GenerateGraphFromGrid(int iterations, DNode[,] grid)
        {
            var _graph = new DungeonGraph();

            //find center of grid
            int center_y = grid.GetLength(0)/2;
            int center_x = grid.GetLength(1)/2;
            DNode center = grid[center_x, center_y];


            var processed = new List<DNode>();
            var unprocessed = new Queue<DNode>();
            unprocessed.Enqueue(center);

            //start from the center and walk out in random directions
            // for every iteration
            for (int i = 0; i < iterations; i++)
            {
                // 1 choose an unprocessed tile in the grid
                DNode currentNode = unprocessed.Dequeue();

                var choices = new List<string>()
                {
                    "up", "down", "left", "right"
                };

                Point currentPosition = grid.CoordinatesOf(currentNode);
                DNode newNode;

                while(true)
                {
                    if (choices.Count == 0)
                    {
                        newNode = getRandomNode(processed);
                        break;
                    };

                    // 2 choose a random direction
                    string choice = choices[_random.Next(0, choices.Count - 1)];
                    Point direction = directions[choice];

                    // 3 if direction is not within bounds try again
                    Point newPosition = currentPosition + direction;
                    int gridDimension = grid.GetLength(0);
                    if (newPosition.x < 0 || newPosition.y < 0 
                        || newPosition.x > gridDimension - 1 || newPosition.y > gridDimension - 1) 
                    {
                        choices.Remove(choice);
                        continue;
                    }
                
                    newNode = grid[
                        currentPosition.y + direction.y,
                        currentPosition.x + direction.x
                        ];

                    // 4 if node in direction exists and already has an edge pick a new direction
                    if (processed.Contains(newNode) && newNode.HasEdgeToNode(currentNode))
                    {
                        choices.Remove(choice);
                        continue;
                    }
                    // If the node in direction allready has maximum number of edges pick a new node
                    if (newNode.Edges.Count >= 4) 
                    {
                        choices.Remove(choice);
                        continue;
                    }
                    // If for some random reason new Node is null try again
                    if (newNode is null)
                    {
                        choices.Remove(choice);
                        continue;
                    };
                    break;
 
                }

                // 5 create an edge on both nodes pointing to eachother
                var edgeTo = new DEdge(currentNode, newNode);
                var edgeFrom = new DEdge(newNode, currentNode);

                currentNode.AddEdge(edgeTo);
                newNode.AddEdge(edgeFrom);
                unprocessed.Enqueue(newNode);
                processed.Add(currentNode);
                
                _graph.AddNode(currentNode);
            }
            return _graph;
        }

        private DNode getRandomNode(List<DNode> nodeList)
        {
            var validNodesQ = from n in nodeList
                where n.Edges.Count < 3
                select n;

            List<DNode> nodes = validNodesQ.ToList();
            return nodes[_random.Next(0, nodes.Count)];
        }
    }
}