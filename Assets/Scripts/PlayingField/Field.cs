using UnityEngine;

public class Field
{
    private int width;
    private int height;    
    private Tile[,] matrixOfTiles;

    #region PROPERTIES
    public int Width { get => width; set => width = value; }
    public int Height { get => height; set => height = value; }    
    public Tile[,] MatrixOfTiles { get => matrixOfTiles; set => matrixOfTiles = value; }
    #endregion

    public Field(int width, int height)
    {
        this.width = width;
        this.height = height;

        MatrixOfTiles = new Tile[width, height];     
        
        CreateRandomField();
    }    

    private void CreateRandomField()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Type randomType = GetValidRandomTypeOfTile(x,y);  
                MatrixOfTiles[y, x] = new Tile(randomType);
            }
        }
    }

    private bool AreRowsStacked(Type randomType, int x, int y)
    {
        return (randomType == MatrixOfTiles[y, x - 1].TypeOfTile) && (randomType == MatrixOfTiles[y, x - 2].TypeOfTile);
    }

    private bool AreColumnsStacked(Type randomType, int x, int y)
    {
        return (randomType == MatrixOfTiles[y - 1, x].TypeOfTile) && (randomType == MatrixOfTiles[y - 2, x].TypeOfTile);
    }

    private Type GetRandomTypeOfTile()
    {
        // TODO - придумать более правильный способ получать рандомный тип из enum'а
        return (Type)Random.Range(1, 6);
    }


    // TODO - есть сомнения в правильности работы - следить
    private Type GetValidRandomTypeOfTile(int x, int y)
    {
        Type randomType = GetRandomTypeOfTile();
        //Type excludedType = Type.Default;

        if (x >= 2)
        {
            while (true)
            {
                if(!AreRowsStacked(randomType, x ,y))
                {
                    break;
                }
                else
                {
                    //excludedType = randomType;
                    randomType = GetRandomTypeOfTile();
                    //Debug.Log(excludedType.ToString());
                }
            }
        }

        if (y >= 2)
        {
            while (true)
            {
                if (!AreColumnsStacked(randomType, x, y))
                {
                    break;
                }
                else
                {
                    //excludedType = randomType;
                    randomType = GetRandomTypeOfTile();
                    Debug.Log(randomType.ToString());                    
                }
            }
        }
        return randomType;
    }
}
