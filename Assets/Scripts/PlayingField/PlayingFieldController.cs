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

    private readonly int width = 10;
    private readonly int height = 10;
    private Vector2 topLeftPointOfTheField;

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

    void Start()
    {
        TypeEnumValuesInit();
        PlayingField = new Field(width, height);
        topLeftPointOfTheField = new Vector2(-width / 2.0f, height / 2.0f);
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
                    case TypeOfTile.Circle:
                        prefabToInstall = circleTilePrefab;
                        break;
                    case TypeOfTile.Diamond:
                        prefabToInstall = diamondTilePrefab;
                        break;
                    case TypeOfTile.Square:
                        prefabToInstall = squareTilePrefab;
                        break;
                    case TypeOfTile.Star:
                        prefabToInstall = starTilePrefab;
                        break;
                    case TypeOfTile.Triangle:
                        prefabToInstall = triangleTilePrefab;
                        break;
                    default:
                        prefabToInstall = defaultTilePrefab;
                        break;
                }

                PlayingField.MatrixOfTiles[y, x].TilePrefab = Instantiate(prefabToInstall,
                    new Vector3(topLeftPointOfTheField.x + x + SpriteShift.x,
                        topLeftPointOfTheField.y - y - SpriteShift.y, 0),
                    Quaternion.identity,
                    tilesParentPrefab.transform);
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
        return (typeOfTile == PlayingField.MatrixOfTiles[y, x - 1].TypeOfTile) &&
               (typeOfTile == PlayingField.MatrixOfTiles[y, x - 2].TypeOfTile);
    }

    private bool AreColumnsStacked(TypeOfTile typeOfTile, int x, int y)
    {
        return (typeOfTile == PlayingField.MatrixOfTiles[y - 1, x].TypeOfTile) &&
               (typeOfTile == PlayingField.MatrixOfTiles[y - 2, x].TypeOfTile);
    }

    private TypeOfTile GetRandomTypeOfTile()
    {
        return (TypeOfTile) valuesOfEnum.GetValue(UnityEngine.Random.Range(1, valuesOfEnum.Length)); // 0 - default
    }
}