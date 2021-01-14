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

    public Field PlayingField { get => playingField; set => playingField = value; }
    public Vector2 SpriteShift { get => spriteShift; set => spriteShift = value; }

    void Start()
    {
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
                PlayingField.MatrixOfTiles[y, x] = new Tile(GetValidRandomTypeOfTile(x,y));                

                GameObject prefabToInstall;

                switch(PlayingField.MatrixOfTiles[y,x].TypeOfTile)
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
                
                PlayingField.MatrixOfTiles[y, x].TilePrefab = Instantiate (prefabToInstall,
                    new Vector3(topLeftPointOfTheField.x + x + SpriteShift.x, topLeftPointOfTheField.y - y - SpriteShift.y, 0),
                    Quaternion.identity,
                    tilesParentPrefab.transform);

                TileController tileController = PlayingField.MatrixOfTiles[y, x].TilePrefab.GetComponent<TileController>();
                tileController.Init(this, new Vector2Int(x, y));                
            }
        }
    }

    // Для теста
    private void RecreateAndFillTheField()
    {
        PlayingField = new Field(width, height);       
        FillTheField();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            RecreateAndFillTheField();
        }
    }

    //private void CreateRandomField()
    //{
    //    for (int y = 0; y < playingField.Height; y++)
    //    {
    //        for (int x = 0; x < playingField.Width; x++)
    //        {
    //            Type randomType = GetValidRandomTypeOfTile(x, y);
    //            MatrixOfTiles[y, x] = new Tile(randomType);
    //        }
    //    }
    //}

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
        // TODO - придумать более правильный способ получать рандомный тип из enum'а
        return (Type)UnityEngine.Random.Range(1, 6);
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
                if (!AreRowsStacked(randomType, x, y))
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
                }
            }
        }
        return randomType;
    }

    public bool IsSwapValid(Vector2Int tileIndex)
    {        
        return true;
    }    
}
