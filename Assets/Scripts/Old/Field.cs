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
    }       
}
