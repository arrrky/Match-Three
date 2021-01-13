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
    
    void Start()
    {
        playingField = new Field(width, height);
        topLeftPointOfTheField = new Vector2(-width / 2, height / 2);
        spriteShift = Tools.GetSpriteShift(defaultTilePrefab); // все спрайты префабов одинаковые
        FillTheField();
    } 

    private void FillTheField()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject prefabToInstall;

                switch(playingField.MatrixOfTiles[y,x].TypeOfTile)
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
                
                playingField.MatrixOfTiles[y, x].TilePrefab = Instantiate (prefabToInstall,
                    new Vector3(topLeftPointOfTheField.x + x + spriteShift.x, topLeftPointOfTheField.y - y - spriteShift.y, 0),
                    Quaternion.identity,
                    tilesParentPrefab.transform);
            }
        }
    }

    // Для теста
    private void RecreateAndFillTheField()
    {
        playingField = new Field(width, height);       
        FillTheField();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            RecreateAndFillTheField();
        }
    }
}
