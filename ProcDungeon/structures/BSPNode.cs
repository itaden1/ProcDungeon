using System;
using System.Collections.Generic;

namespace ProcDungeon.Structures
{
	public class BSPNode
	{
		private enum Orientation {Vertical, Horizontal};
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

		public List<BSPNode> Leaves { 
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
			if (IsLeaf) leaves.Add(this);
			else 
			{
				leaves.AddRange(Branch1.GetAllLeaves());
				leaves.AddRange(Branch2.GetAllLeaves());
			}
			return leaves;
		}
		public void Partition(int iterations, int minSize)
		{
			if (iterations > 0)
			{
				Orientation _orientation = GetOrientation();
				if (_orientation == Orientation.Vertical)
				{
					Branch1 = new BSPNode(
						LeftEdge, 
						_random.Next(LeftEdge + minSize, RightEdge - minSize), 
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
				else
				{
					Branch1 = new BSPNode(
						LeftEdge, 
						RightEdge, 
						TopEdge, 
						_random.Next(TopEdge + minSize, BottomEdge - minSize)
					);
					Branch2 = new BSPNode(
						LeftEdge, 
						RightEdge, 
						Branch1.BottomEdge, 
						BottomEdge
					);
				}
				int remainingIterations = iterations - 1;
				Branch1.Partition(remainingIterations, minSize);
				Branch2.Partition(remainingIterations, minSize);
			}
			else
			{
				IsLeaf = true;
			}
		}
		private Orientation GetOrientation()
		{
			if (_height / _width * 100 < 10) return Orientation.Vertical;
			if (_width / _height * 100 < 10) return Orientation.Horizontal;

			return _random.Next(0, 2) == 1 ? Orientation.Horizontal : Orientation.Vertical;

		}
	}
}
