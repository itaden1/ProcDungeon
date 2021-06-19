
using ProcDungeon.Interfaces;

namespace ProcDungeon.Structures
{
    //<summary>Class <c>Tile</c> 
    //Holds data related to a map region</summary>
    public class Tile: INode
    {
        public string NodeType { get; }
        public Point Coords { get; set; }
        public string Colour { get; set; }

        public Tile(int x, int y, string ntype, string col)
        {
            Coords = new Point(x,y);
            NodeType = ntype;
            Colour = col;
        }
        public string GetData() => ToString();
        public override string ToString()
        {
            return NodeType;
        }
    }
}