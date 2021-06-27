using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcDungeon.Structures
{
    public class BSPNode
    {
        private enum Orientation { Vertical, Horizontal };
        public int LeftEdge;
        public int TopEdge;
        public int RightEdge;
        public int BottomEdge;
        private int _width;
        public int Width => _width;
        private int _height;
        public int Height => _height;
        private Random _random = new Random();

        public BSPNode Branch1;
        public BSPNode Branch2;
        private List<BSPNode> _leaves = new List<BSPNode>();
        private int _margin;

        public List<BSPNode> Leaves
        {
            get
            {
                if (_leaves.Count <= 0)
                {
                    _leaves = GetAllLeaves();
                }
                return _leaves;
            }
        }

        public BSPNode(int startX, int endX, int startY, int endY)
        {
            LeftEdge = startX;
            RightEdge = endX;
            TopEdge = startY;
            BottomEdge = endY;

            _height = endY - startY;
            _width = endX - startX;
        }
        private List<BSPNode> GetAllLeaves()
        {
            List<BSPNode> leaves = new List<BSPNode>();
            if (Branch1 is null && Branch2 is null)
            {
                leaves.Add(this);
                return leaves;
            }
            if (Branch1 != null)
            {
                leaves.AddRange(Branch1.GetAllLeaves());
            }
            if (Branch2 != null)
            {
                leaves.AddRange(Branch2.GetAllLeaves());
            }
            return leaves;
        }
        public BSPNode GetLeafeFromPoint(Point p)
        {
            // Select an edge node from the BSPTree based on somecoordinates
            IEnumerable<BSPNode> query = from n in Leaves
                                         where n.TopEdge <= p.y
                                         where n.BottomEdge >= p.y
                                         where n.LeftEdge <= p.x
                                         where n.RightEdge >= p.x
                                         select n;
            return query.First();
        }
        public void Partition(int iterations)
        {
            if (iterations > 1)
            {
                Orientation _orientation = GetOrientation();
                if (_orientation == Orientation.Vertical)
                {
                    CreateVerticalPartition();
                }
                else
                {
                    CreateHorizontalPartition();
                }
                iterations = iterations--;
                if (iterations > 0)
                {
                    if (iterations == 1)
                    {
                        Branch1.Partition(2);
                    }
                    else if (iterations % 2 == 0)
                    {
                        Branch1.Partition(iterations / 2);
                        Branch2.Partition(iterations / 2);
                    }
                    else
                    {
                        Branch1.Partition(iterations / 2 + iterations % 2);
                        Branch2.Partition(iterations / 2);
                    }
                }
            }
        }

        internal List<BSPNode> GetNeighbouringLeaves(BSPNode leaf)
        {
            // get a list of leaves that are neighbours of the supplied leaf
            IEnumerable<BSPNode> query = from ln in Leaves
                                         where ln.RightEdge == leaf.LeftEdge
                                             || ln.TopEdge == leaf.BottomEdge
                                             || ln.LeftEdge == leaf.RightEdge
                                             || ln.BottomEdge == leaf.TopEdge
                                         select ln;

            return query.ToList();
        }

        private void CreateHorizontalPartition()
        {
            int split = _random.Next(TopEdge + _height / 3, BottomEdge - _height / 3);
            Branch1 = new BSPNode(
                LeftEdge,
                RightEdge,
                TopEdge,
                split
            );
            Branch2 = new BSPNode(
                LeftEdge,
                RightEdge,
                Branch1.BottomEdge,
                BottomEdge
            );
        }

        private void CreateVerticalPartition()
        {
            int split = _random.Next(LeftEdge + _width / 3, RightEdge - _width / 3);
            Branch1 = new BSPNode(
                LeftEdge,
                split,
                TopEdge,
                BottomEdge
            );
            Branch2 = new BSPNode(
                Branch1.RightEdge,
                RightEdge,
                TopEdge,
                BottomEdge
            );
        }
        private Orientation GetOrientation()
        {
            if (_height / _width * 100 < 10) return Orientation.Vertical;
            if (_width / _height * 100 < 10) return Orientation.Horizontal;

            return _random.Next(0, 2) == 1 ? Orientation.Horizontal : Orientation.Vertical;

        }

        public static Point GetLeafOverlapPoint(BSPNode l1, BSPNode l2)
        {
            // get the area of overlap, should only occure on a singe axis
            var areaOverlapX = Math.Min(l1.RightEdge, l2.RightEdge) - Math.Max(l1.LeftEdge, l2.LeftEdge);
            var areaOverlapY = Math.Min(l1.BottomEdge, l2.BottomEdge) - Math.Max(l1.TopEdge, l2.TopEdge);

            // get half of the ovelrap while avoiding division by zero error
            int offsetX = 0;
            int offsetY = 0;
            if (areaOverlapX > 0) offsetX = areaOverlapX / 2;
            if (areaOverlapY > 0) offsetY = areaOverlapY / 2;

            // add the offset to the leaf with the highest axis
            int centerX = Math.Max(l1.LeftEdge, l2.LeftEdge) + offsetX;
            int centerY = Math.Max(l1.TopEdge, l2.TopEdge) + offsetY;            

            return new Point(centerX, centerY);
        }
    }
}
