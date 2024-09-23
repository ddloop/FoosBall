using System.Collections.Generic;

public static class Helper
{    
    public static void SetRegionToTrue(bool[,] array, int startRow, int startCol, int endRow, int endCol)
    {
        for (int i = startRow; i <= endRow; i++)
        {
            for (int j = startCol; j <= endCol; j++)
            {
                array[i, j] = true;
            }
        }
    }

    public static void FillRegionConnections() 
    { 

    }
}
