
using ProcDungeon.Interfaces;

namespace ProcDungeon.Structures
{
    //<summary>Class <c>Tile</c> 
    //Holds data related to a map region</summary>
    public class Tile: ITileNode
    {
        public string TileType { get; }
        public string Colour { get; set; }
        public bool Blocking { get; set; }
        public Tile()
        {
            Blocking = true;
        }
    }
}