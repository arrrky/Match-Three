using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using MiscTools;
using UnityEngine.Serialization;

public class PlayingFieldController : MonoBehaviour
{
    public static PlayingFieldController Instance;

    [SerializeField] private GameObject tilesParentPrefab;
    [SerializeField] private List<GameObject> tilesPrefabsList = new List<GameObject>();

    private readonly Dictionary<string, GameObject> tilesPrefabsDictionary = new Dictionary<string, GameObject>();

    private readonly int width = 10;
    private readonly int height = 10;
    private Vector2 topLeftPointOfTheField;

    #region TilesPrefabsDicInit

    private void TilesPrefabDicInit()
    {
        foreach (GameObject tilePrefab in tilesPrefabsList)
        {
            tilesPrefabsDictionary.Add(tilePrefab.name, tilePrefab);
        }
    }

    #endregion

    #region EnumValuesInit

    private System.Type typeOfEnum;
    private Array valuesOfEnum;

    private void TypeEnumValuesInit()
    {
        typeOfEnum = typeof(TypeOfTile);
        valuesOfEnum = typeOfEnum.GetEnumValues();
    }

    #endregion

    #region PROPERTIES

    public Field PlayingField { get; set; }
    public Vector2 SpriteShift { get; set; }

    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        TilesPrefabDicInit();
        TypeEnumValuesInit();
        PlayingField = new Field(width, height);
        topLeftPointOfTheField = new Vector2(-width / 2.0f, height / 2.0f);
        SpriteShift =
            Tools.GetSpriteShift(tilesPrefabsDictionary["Default"]); // все спрайты префабов одинаковые по размеру
        FillTheField();
    }

    #region FieldInit

    private void FillTheField()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                PlayingField.MatrixOfTiles[x, y] = new Tile(GetValidRandomTypeOfTile(x, y));
                TypeOfTile currentTypeOfTile = PlayingField.MatrixOfTiles[x, y].TypeOfTile;

                GameObject prefabToInstall = tilesPrefabsDictionary[currentTypeOfTile.ToString()];

                PlayingField.MatrixOfTiles[x, y].TilePrefab = Instantiate(prefabToInstall,
                    new Vector3(topLeftPointOfTheField.x + x + SpriteShift.x,
                        topLeftPointOfTheField.y - y - SpriteShift.y, 0),
                    Quaternion.identity,
                    tilesParentPrefab.transform);

                PlayingField.MatrixOfTiles[x, y].TilePrefab.GetComponent<TileController>().TileIndex =
                    new Vector2Int(x, y);
            }
        }
    }

    private TypeOfTile GetValidRandomTypeOfTile(int x, int y)
    {
        // Доработать, есть случаи, когда отрабатывает неправильно
        // (при проверке столбца может поставить элемент, который был исключен при проверке строки)
        TypeOfTile randomType = GetRandomTypeOfTile();
        TypeOfTile excludedType = TypeOfTile.Default;

        if (x >= 2)
        {
            while (true)
            {
                if (!AreRowsStacked(randomType, x, y))
                    break;
                else
                {
                    excludedType = randomType;
                    randomType = GetRandomTypeOfTile();
                }
            }
        }

        if (y >= 2)
        {
            while (true)
            {
                if (!AreColumnsStacked(randomType, x, y) && excludedType != randomType)
                    break;
                else
                {
                    randomType = GetRandomTypeOfTile();
                }
            }
        }

        return randomType;
    }

    private bool AreRowsStacked(TypeOfTile typeOfTile, int x, int y)
    {
        return (typeOfTile == PlayingField.MatrixOfTiles[x - 1, y].TypeOfTile) &&
               (typeOfTile == PlayingField.MatrixOfTiles[x - 2, y].TypeOfTile);
    }

    private bool AreColumnsStacked(TypeOfTile typeOfTile, int x, int y)
    {
        return (typeOfTile == PlayingField.MatrixOfTiles[x, y - 1].TypeOfTile) &&
               (typeOfTile == PlayingField.MatrixOfTiles[x, y - 2].TypeOfTile);
    }

    #endregion

    private TypeOfTile GetRandomTypeOfTile()
    {
        return (TypeOfTile) valuesOfEnum.GetValue(UnityEngine.Random.Range(1, valuesOfEnum.Length)); // 0 - default
    }

    public bool IsSwapValid(Vector2Int tile0Index, Vector2Int tile1Index)
    {
        return false;
    }

    public void SwapTilesInMatrix(Vector2Int tile0Index, Vector2Int tile1Index)
    {
        Tile tempTile = PlayingField.MatrixOfTiles[tile0Index.x, tile0Index.y];

        PlayingField.MatrixOfTiles[tile0Index.x, tile0Index.y] = PlayingField.MatrixOfTiles[tile1Index.x, tile1Index.y];
        PlayingField.MatrixOfTiles[tile1Index.x, tile1Index.y] = tempTile;
    }

    public void DeleteMatches(Dictionary<Vector2Int, TypeOfTile> tileToDelete)
    {
        List<Vector2Int> temp = new List<Vector2Int>();

        foreach (var tile in tileToDelete)
        {
            temp.Add(tile.Key);
        }

        foreach (var tileIndex in temp)
        {
            Destroy(PlayingField.MatrixOfTiles[tileIndex.x, tileIndex.y].TilePrefab); //удаляем на сцене
            PlayingField.MatrixOfTiles[tileIndex.x, tileIndex.y] = null; // в матрице
        }

        MoveEmptyTiles();
    }

    private void MoveEmptyTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height - 1; y++)
            {
                if (PlayingField.MatrixOfTiles[x, y + 1] == null)
                {
                    Tile temp = PlayingField.MatrixOfTiles[x, y];

                    PlayingField.MatrixOfTiles[x, y].TilePrefab.transform.position += Vector3.down;
                    PlayingField.MatrixOfTiles[x, y] = PlayingField.MatrixOfTiles[x, y + 1];
                    PlayingField.MatrixOfTiles[x, y + 1] = temp;
                }
            }
        }
    }
}