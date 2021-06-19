using System.Collections.Generic;
using ProcDungeon.Structures;

namespace ProcDungeon
{
    public static class ExtensionMethods
    {
        public static Point CoordinatesOf<T>(this T[,] matrix, T value)
        {
            int width = matrix.GetLength(0);
            int height = matrix.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (matrix[x, y].Equals(value)) return new Point(x, y);
                }
            }
            return new Point(-1, -1);
        }
    }

}