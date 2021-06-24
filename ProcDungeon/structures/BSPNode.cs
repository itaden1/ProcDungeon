using System;
using System.Collections.Generic;
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
        private int _height;
        private Random _random = new Random();

        public BSPNode Branch1;
        public BSPNode Branch2;
        public bool IsLeaf = false;
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
                Console.WriteLine("Found leaf");
                return leaves;
            }
            if (Branch1 != null)
            {
                leaves.AddRange(Branch1.GetAllLeaves());
                Console.WriteLine("looking deeper b1");
            }
            if (Branch2 != null)
            {
                leaves.AddRange(Branch2.GetAllLeaves());
                Console.WriteLine("looking deeper b2");
            }
            return leaves;
        }
        public void Partition(int iterations)
        {
            if (iterations > 1)
            {
                Console.WriteLine("*");
                Orientation _orientation = GetOrientation();
                if (_orientation == Orientation.Vertical)
                {
                    CreateVerticalPartition();
                }
                else
                {
                    CreateHorizontalPartition();
                }
                iterations = iterations - 2;

                var remaining = iterations / 2;
                var odd = iterations % 2;

                Console.WriteLine($"iterations{iterations} b1={remaining + odd} : b2={remaining}");
                Branch1.Partition(remaining + odd);
                Branch2.Partition(remaining);

            }
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

        public override string ToString()
        {
            var str  = new StringBuilder();
            if (Branch2 is null) str.Append(">");
            else str.Append(Branch2.ToString());
            if (Branch1 is null) str.Append("<");
            else str.Append(Branch1.ToString());

            return str.ToString();
        }
    }
}
