

namespace ProcDungeon.Structures
{
    public class BaseTile
    {
        public bool Blocking { get; set; }
        public byte BitSymbol = 0; // placeholder for tile data 255 types of tile

    }

    //<summary>Class <c>Tile</c> 
    //Holds data related to a map region</summary>
    public class Tile: BaseTile
    {
        bool Corridoor = false;

        public Tile()
        {
            Blocking = true;
        }
    }
}