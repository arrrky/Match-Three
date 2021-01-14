using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiscTools;

public class PlayingFieldController : MonoBehaviour
{
    [SerializeField] GameObject tilesParentPrefab;

    [SerializeField] GameObject defaultTilePrefab;
    [SerializeField] GameObject circleTilePrefab;
    [SerializeField] GameObject diamondTilePrefab;
    [SerializeField] GameObject starTilePrefab;
    [SerializeField] GameObject squareTilePrefab;
    [SerializeField] GameObject triangleTilePrefab;

    private Field playingField;
    private int width = 10;
    private int height = 10;
    private Vector2 topLeftPointOfTheField;
    private Vector2 spriteShift;

    #region EnumValuesInit
    private System.Type typeOfEnum;
    private Array valuesOfEnum;
    private void TypeEnumValuesInit()
    {
        typeOfEnum = typeof(Type);
        valuesOfEnum = typeOfEnum.GetEnumValues();
    }
    #endregion

    public Field PlayingField { get => playingField; set => playingField = value; }
    public Vector2 SpriteShift { get => spriteShift; set => spriteShift = value; }

    void Start()
    {
        TypeEnumValuesInit();
        PlayingField = new Field(width, height);
        topLeftPointOfTheField = new Vector2(-width / 2, height / 2);
        SpriteShift = Tools.GetSpriteShift(defaultTilePrefab); // все спрайты префабов одинаковые
        FillTheField();
    }

    private void FillTheField()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                PlayingField.MatrixOfTiles[y, x] = new Tile(GetValidRandomTypeOfTile(x, y));

                GameObject prefabToInstall;

                switch (PlayingField.MatrixOfTiles[y, x].TypeOfTile)
                {
                    case Type.Circle:
                        prefabToInstall = circleTilePrefab;
                        break;
                    case Type.Diamond:
                        prefabToInstall = diamondTilePrefab;
                        break;
                    case Type.Square:
                        prefabToInstall = squareTilePrefab;
                        break;
                    case Type.Star:
                        prefabToInstall = starTilePrefab;
                        break;
                    case Type.Triangle:
                        prefabToInstall = triangleTilePrefab;
                        break;
                    default:
                        prefabToInstall = defaultTilePrefab;
                        break;
                }

                PlayingField.MatrixOfTiles[y, x].TilePrefab = Instantiate(prefabToInstall,
                    new Vector3(topLeftPointOfTheField.x + x + SpriteShift.x, topLeftPointOfTheField.y - y - SpriteShift.y, 0),
                    Quaternion.identity,
                    tilesParentPrefab.transform);
            }
        }
    }   

    private Type GetValidRandomTypeOfTile(int x, int y)
    {
        // Доработать, есть случаи, когда отрабатывает неправильно
        // (при проверке столбца может поставить элемент, который был исключен при проверке строки)
        Type randomType = GetRandomTypeOfTile();
        Type excludedType = Type.Default;

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
                Debug.Log($"Excluded: {excludedType}");
                Debug.Log($"Tile index : {x}-{y}");
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

    public bool AreRowsStacked(Type typeOfTile, int x, int y)
    {
        return (typeOfTile == PlayingField.MatrixOfTiles[y, x - 1].TypeOfTile) && (typeOfTile == PlayingField.MatrixOfTiles[y, x - 2].TypeOfTile);
    }

    public bool AreColumnsStacked(Type typeOfTile, int x, int y)
    {
        return (typeOfTile == PlayingField.MatrixOfTiles[y - 1, x].TypeOfTile) && (typeOfTile == PlayingField.MatrixOfTiles[y - 2, x].TypeOfTile);
    }

    private Type GetRandomTypeOfTile()
    {
        return (Type)valuesOfEnum.GetValue(UnityEngine.Random.Range(1, valuesOfEnum.Length)); // 0 - default
    }
}
