using System;
using System.Collections;
using System.Collections.Generic;
using MiscTools;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayingFieldControllerNew : MonoBehaviour
{
    public static PlayingFieldControllerNew Instance { get; set; }
    
    [SerializeField] private List<Sprite> spritesOfTiles = new List<Sprite>();
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject tilesParent;
        
    [SerializeField] [Range(2,10)] private int height = 10;
    [SerializeField] [Range(2,10)] private int width = 10;

    private GameObject[,] field;
   
    private Vector2 topLeftPositionOfTheField;
    private Vector2 spriteShift;

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

    private void Start()
    {
        field = new GameObject[width,height];
        topLeftPositionOfTheField = new Vector2(-width / 2.0f, height / 2.0f);
        spriteShift = Tools.GetSpriteShift(spritesOfTiles[0]);
        FieldInit();
    }

    private void FieldInit()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject newTile = Instantiate(tilePrefab, tilesParent.transform);
                SpriteRenderer newTileRenderer = newTile.GetComponent<SpriteRenderer>();

                List<Sprite> validSprites = GetValidSpritesList(x, y);
                
                newTileRenderer.sprite = validSprites[Random.Range(0, validSprites.Count)];
                
                newTile.transform.position = new Vector3(
                    topLeftPositionOfTheField.x + spriteShift.x + x,
                    topLeftPositionOfTheField.y - spriteShift.y - y);

                field[x, y] = newTile;

                TileControllerNew tileControllerNew = newTile.AddComponent<TileControllerNew>();
                tileControllerNew.TileIndex = new Vector2Int(x,y);
            }
        }
    }
    
    private List<Sprite> GetValidSpritesList(int x, int y)
    {
        List<Sprite> validSprites = new List<Sprite>(spritesOfTiles);

        Sprite left1 = GetSpriteAt(x - 1, y);
        Sprite left2 = GetSpriteAt(x - 2, y);

        if (left2 != null || left1 == left2)
        {
            validSprites.Remove(left1);
        }
        
        Sprite up1 = GetSpriteAt(x, y - 1);
        Sprite up2 = GetSpriteAt(x, y - 2);

        if (up2 != null || up1 == up2)
        {
            validSprites.Remove(up1);
        }

        return validSprites;
    }

    private Sprite GetSpriteAt(int x, int y)    
    {
        // Проверка, чтобы не выходить за границы массива, при обращении к его элементам
        if (x < 0 || x > width - 1 || y < 0 || y > height - 1)
            return null;
        
        GameObject tile = field[x, y];
        SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();

        return sr.sprite;
    }

    public void SwapTiles(Vector2Int tile0Position, Vector2Int tile1Position)
    {
        GameObject tile0 = field[tile0Position.x, tile0Position.y];
        SpriteRenderer tile0Renderer = tile0.GetComponent<SpriteRenderer>();
        
        GameObject tile1 = field[tile1Position.x, tile1Position.y];
        SpriteRenderer tile1Renderer = tile1.GetComponent<SpriteRenderer>();

        Sprite tempSprite = tile0Renderer.sprite;
        tile0Renderer.sprite = tile1Renderer.sprite;
        tile1Renderer.sprite = tempSprite;
    }
}
