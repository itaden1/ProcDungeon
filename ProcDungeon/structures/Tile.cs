

namespace ProcDungeon.Structures
{
    public class BaseTile
    {
        public bool Blocking { get; set; }
    }

    //<summary>Class <c>Tile</c> 
    //Holds data related to a map region</summary>
    public class Tile: BaseTile
    {
        public Tile()
        {
            Blocking = true;
        }
    }
}