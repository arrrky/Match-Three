using System;
using System.Collections;
using System.Collections.Generic;
using MiscTools;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class FieldManager : MonoBehaviour
{
    public static FieldManager Instance { get; private set; }
    [SerializeField] private ScoreManager scoreManager;

    #region TilesPrefabs

    [SerializeField] private GameObject circle;
    [SerializeField] private GameObject triangle;
    [SerializeField] private GameObject diamond;
    [SerializeField] private GameObject square;
    [SerializeField] private GameObject star;

    #endregion

    [SerializeField] private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
    [SerializeField] private List<Sprite> spritesOfTiles = new List<Sprite>();
    [SerializeField] private GameObject defaultTilePrefab;
    [SerializeField] private GameObject tilesParent;

    [SerializeField] [Range(2, 10)] private int height = 10;
    [SerializeField] [Range(2, 10)] private int width = 10;

    private GameObject[,] field;
    private Vector2 bottomLeftPositionOfTheField;
    private Vector2 spriteShift;

    private float timeForSwapAnimation = 0.3f;
    private bool isShifting;

    public event Action<int> MatchesFound;
    public static event Action TilesSwapped;

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
        FillPrefabsDictionary();
        field = new GameObject[width, height];
        bottomLeftPositionOfTheField = new Vector2(-width / 2.0f, -height / 2.0f);
        spriteShift = Tools.GetSpriteShift(spritesOfTiles[0]);
        FieldInit();
    }

    private void FillPrefabsDictionary()
    {
        prefabs.Add("star", star);
        prefabs.Add("triangle", triangle);
        prefabs.Add("square", square);
        prefabs.Add("circle", circle);
        prefabs.Add("diamond", diamond);
    }

    private void FieldInit()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject newTile = Instantiate(defaultTilePrefab, tilesParent.transform);
                SpriteRenderer newTileRenderer = newTile.GetComponent<SpriteRenderer>();

                List<Sprite> validSprites = GetValidSpritesList(x, y);

                newTileRenderer.sprite = validSprites[Random.Range(0, validSprites.Count)];

                newTile.transform.position = new Vector3(
                    bottomLeftPositionOfTheField.x + spriteShift.x + x,
                    bottomLeftPositionOfTheField.y + spriteShift.y + y);

                field[x, y] = newTile;

                TileController tileController = newTile.AddComponent<TileController>();
                tileController.TileIndex = new Vector2Int(x, y);
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

    private SpriteRenderer GetSpriteRendererAt(int x, int y)
    {
        // Проверка, чтобы не выходить за границы массива, при обращении к его элементам
        if (x < 0 || x > width - 1 || y < 0 || y > height - 1)
            return null;

        GameObject tile = field[x, y];
        SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();

        return sr;
    }

    public void SwapTiles(Vector2Int tile0Position, Vector2Int tile1Position)
    {
        GameObject tile0 = field[tile0Position.x, tile0Position.y];
        SpriteRenderer tile0Renderer = tile0.GetComponent<SpriteRenderer>();

        GameObject tile1 = field[tile1Position.x, tile1Position.y];
        SpriteRenderer tile1Renderer = tile1.GetComponent<SpriteRenderer>();

        StartCoroutine(SwapAnimation(tile0Position, tile1Position));

        Sprite tempSprite = tile0Renderer.sprite;
        tile0Renderer.sprite = tile1Renderer.sprite;
        tile1Renderer.sprite = tempSprite;

        bool isSwapValid = GetMatchesCount() > 0;

        if (!isSwapValid)
        {
            StartCoroutine(SwapAnimation(tile0Position, tile1Position));
            tempSprite = tile0Renderer.sprite;
            tile0Renderer.sprite = tile1Renderer.sprite;
            tile1Renderer.sprite = tempSprite;
        }
        else
        {
            do
            {
                if (scoreManager.Score >= scoreManager.Goal)
                    return;   
                
                OnMatchesFound(GetMatchesCount());
                UpdateField();
            } while (GetMatchesCount() > 0);
        }

        OnTilesSwapped();
    }

    private void OnMatchesFound(int matchesCount)
    {
        MatchesFound?.Invoke(matchesCount);
    }

    private void OnTilesSwapped()
    {
        TilesSwapped?.Invoke();
    }

    private int GetMatchesCount()
    {
        HashSet<SpriteRenderer> matchedTiles = new HashSet<SpriteRenderer>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SpriteRenderer currentSpriteRenderer = GetSpriteRendererAt(x, y);

                List<SpriteRenderer> rowsMatches = GetRowsMatches(x, y, currentSpriteRenderer.sprite);
                if (rowsMatches.Count >= 2)
                {
                    matchedTiles.UnionWith(rowsMatches);
                    matchedTiles.Add(currentSpriteRenderer);
                }

                List<SpriteRenderer> columnsMatches = GetColumnsMatches(x, y, currentSpriteRenderer.sprite);
                if (columnsMatches.Count >= 2)
                {
                    matchedTiles.UnionWith(columnsMatches);
                    matchedTiles.Add(currentSpriteRenderer);
                }
            }
        }

        foreach (SpriteRenderer spriteRenderer in matchedTiles)
        {
            spriteRenderer.sprite = null;
        }
        
        return matchedTiles.Count;
    }

    private List<SpriteRenderer> GetColumnsMatches(int x, int y, Sprite currentSprite)
    {
        List<SpriteRenderer> listOfMatches = new List<SpriteRenderer>();

        for (int i = x + 1; i < width; i++)
        {
            SpriteRenderer nextColumnRenderer = GetSpriteRendererAt(i, y);
            if (nextColumnRenderer.sprite != currentSprite)
            {
                break;
            }

            listOfMatches.Add(nextColumnRenderer);
        }

        return listOfMatches;
    }

    private List<SpriteRenderer> GetRowsMatches(int x, int y, Sprite currentSprite)
    {
        List<SpriteRenderer> listOfMatches = new List<SpriteRenderer>();

        for (int i = y + 1; i < height; i++)
        {
            SpriteRenderer nextColumnRenderer = GetSpriteRendererAt(x, i);
            if (nextColumnRenderer.sprite != currentSprite)
            {
                break;
            }

            listOfMatches.Add(nextColumnRenderer);
        }

        return listOfMatches;
    }

    private void UpdateField()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                while (GetSpriteRendererAt(x, y).sprite == null)
                {
                    for (int i = y; i < height - 1; i++)
                    {
                        SpriteRenderer currentTileSpriteRenderer = GetSpriteRendererAt(x, i);
                        SpriteRenderer nextTileSpriteRenderer = GetSpriteRendererAt(x, i + 1);
                        currentTileSpriteRenderer.sprite = nextTileSpriteRenderer.sprite;
                    }

                    SpriteRenderer lastTileSpriteRenderer = GetSpriteRendererAt(x, height - 1);
                    lastTileSpriteRenderer.sprite = spritesOfTiles[Random.Range(0, spritesOfTiles.Count)];
                }
            }
        }
    }

    public void EraseField()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GetSpriteRendererAt(x, y).sprite = null;
            }
        }
    }

    private IEnumerator SwapAnimation(Vector2Int tile0Index, Vector2Int tile1Index)
    {
        SpriteRenderer tile0SpriteRenderer = GetSpriteRendererAt(tile0Index.x, tile0Index.y);
        SpriteRenderer tile1SpriteRenderer = GetSpriteRendererAt(tile1Index.x, tile1Index.y);

        Vector3 tile0Position = tile0SpriteRenderer.gameObject.transform.position;
        Vector3 tile1Position = tile1SpriteRenderer.gameObject.transform.position;

        tile0SpriteRenderer.gameObject.SetActive(false);
        tile1SpriteRenderer.gameObject.SetActive(false);

        GameObject prefab0ToInstall = prefabs[tile0SpriteRenderer.sprite.name];
        GameObject prefab1ToInstall = prefabs[tile1SpriteRenderer.sprite.name];

        GameObject tempTile0 = Instantiate(prefab0ToInstall, tile0Position, Quaternion.identity, this.transform);
        GameObject tempTile1 = Instantiate(prefab1ToInstall, tile1Position, Quaternion.identity, this.transform);
      
        float currentTime = 0;

        while (currentTime < timeForSwapAnimation)
        {
            currentTime += Time.deltaTime;

            tempTile0.transform.position =
                Vector3.MoveTowards(tile0Position, tile1Position, (currentTime / timeForSwapAnimation));

            tempTile1.transform.position =
                Vector3.MoveTowards(tile1Position, tile0Position, (currentTime / timeForSwapAnimation));
            
            yield return new WaitForEndOfFrame();
        }

        Destroy(tempTile0);
        Destroy(tempTile1);

        tile0SpriteRenderer.gameObject.SetActive(true);
        tile1SpriteRenderer.gameObject.SetActive(true);
    }
}