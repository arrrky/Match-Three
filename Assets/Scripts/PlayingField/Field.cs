using UnityEngine;

public class Field
{
    private int width;
    private int height;
    private Type[,] matrixOfTypes;
    private GameObject[,] matrixOfTilesPrefabs;

    public int Width { get => width; set => width = value; }
    public int Height { get => height; set => height = value; }
    public Type[,] MatrixOfTypes { get => matrixOfTypes; set => matrixOfTypes = value; }
    public GameObject[,] MatrixOfTilesPrefabs { get => matrixOfTilesPrefabs; set => matrixOfTilesPrefabs = value; }

    public Field(int width, int height)
    {
        this.width = width;
        this.height = height;

        matrixOfTypes = new Type[width, height];
        matrixOfTilesPrefabs = new GameObject[width, height];

        CreateRandomField();
    }

    private void CreateRandomField()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                // TODO - добавить проверки, чтобы не создавать поле, где уже есть совпадения
                MatrixOfTypes[y, x] = GetRandomType();
            }
        }
    }

    private Type GetRandomType()
    {
        // TODO - придумать более правильный способ получать рандомный тип из enum'а
        return (Type)Random.Range(0, 4);
    }
   
}


