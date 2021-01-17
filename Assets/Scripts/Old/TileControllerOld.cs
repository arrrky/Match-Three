using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TileControllerOld : MonoBehaviour
{
    private SpriteRenderer tileColor;
    
    private readonly Dictionary<string, Color> tileColorsDictionary = new Dictionary<string, Color>
    {
        {"defaultColor", Color.white},
        {"selectedColor", Color.yellow},
        {"errorColor", Color.red},
        {"highlightColor", Color.green},
    };
    
    private const float BlinkTime = 0.3f;
    
    private static TileControllerOld _selectedTile;
    private static bool _isHighlighted;

    private readonly List<Vector2> rayDirection = new List<Vector2>
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    private List<GameObject> nearTiles = new List<GameObject>();
    
    public Vector2Int TileIndex { get; set; }


    private void Start()
    {
        tileColor = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if (_selectedTile == this)
        {
            HighlightNearTiles();
            Deselect();
            return;
        }

        if (_selectedTile != null)
        {
            SwapTilesOnScene(this, _selectedTile);
            _selectedTile.Deselect();
        }
        else
        {
            Select();
            DetectNearTiles();
        }
    }

    private void Select()
    {
        SetColor(tileColorsDictionary["selectedColor"]);
        _selectedTile = this;
    }

    private void Deselect()
    {
        SetColor(tileColorsDictionary["defaultColor"]);
        _selectedTile = null;
    }

    private void SetColor(Color colorOtTile)
    {
        tileColor.color = colorOtTile;
    }

    private void DetectNearTiles()
    {
        foreach (Vector2 direction in rayDirection)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);
            if (hit.collider != null)
            {
                nearTiles.Add(hit.collider.gameObject);
            }
        }
        
        HighlightNearTiles();
    }
    
    private void SwapTilesOnScene(TileControllerOld thisTile, TileControllerOld selectedTile)
    {
        HighlightNearTiles();

        if (!IsChosenTileNear())
        {
            StartErrorColorBlinkRoutine(this);
            return;
        }
        
        PlayingFieldControllerOld.Instance.SwapTilesInMatrix(thisTile.TileIndex, selectedTile.TileIndex);

        // Сделать синглтон?
        MatchesChecker mc = new MatchesChecker();
        Dictionary<Vector2Int, TypeOfTile> tilesToDelete = mc.GetTilesToDelete();

        if (tilesToDelete.Count == 0)
        {
            //Если пришел пустой словарь - значит совпадений этот ход не вызвал - меняем обратно
            PlayingFieldControllerOld.Instance.SwapTilesInMatrix(selectedTile.TileIndex, thisTile.TileIndex);
            return;
        }

        foreach (var item in tilesToDelete)
        {
            Debug.Log($"Tile to delete {item.Value.ToString()} : {item.Key}");
        }

        var thisTileGameObject = thisTile.gameObject;
        var thisTileGameObjectTransformPosition = thisTileGameObject.transform.position;

        var selectedTileGameObject = selectedTile.gameObject;
        var selectedTileGameObjectTransformPosition = selectedTileGameObject.transform.position;

        selectedTileGameObject.transform.position = thisTileGameObjectTransformPosition;
        thisTileGameObject.transform.position = selectedTileGameObjectTransformPosition;
        
        PlayingFieldControllerOld.Instance.DeleteMatches(tilesToDelete);
    }

    private bool IsChosenTileNear()
    {
        bool result = false;
        for (int index = 0; index < _selectedTile.nearTiles.Count; index++)
        {
            if (this.gameObject == _selectedTile.nearTiles[index])
            {
                // Debug.Log($"This object name: {gameObject.name}");
                // Debug.Log($"Near tile name: {_selectedTile.nearTiles[index].name}");
                result = true;
            }
        }

        _selectedTile.nearTiles.Clear();

        return result;
    }

    private void StartErrorColorBlinkRoutine(TileControllerOld tileControllerOld)
    {
        StartCoroutine(ErrorColorBlinkRoutine(tileControllerOld));
    }
    
    private IEnumerator ErrorColorBlinkRoutine(TileControllerOld tileControllerOld)
    {
        tileControllerOld.tileColor.color = tileColorsDictionary["errorColor"];
        yield return  new WaitForSeconds(BlinkTime);
        tileControllerOld.tileColor.color = tileColorsDictionary["defaultColor"];
    }

    private void HighlightNearTiles()
    {
        Color color = _isHighlighted ? tileColorsDictionary["defaultColor"] : tileColorsDictionary["highlightColor"];

        foreach (GameObject tile in _selectedTile.nearTiles)
        {
            tile.gameObject.GetComponent<TileControllerOld>().SetColor(color);
        }
        _isHighlighted = !_isHighlighted;
    }
}