using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TileController : MonoBehaviour
{
    public Vector2Int TileIndex { get; set; }

    private SpriteRenderer tileColor;
    private const float BlinkTime = 0.3f;
    
    private static TileController _selectedTile;
    private static bool _isHighlighted;

    private readonly List<Vector2> rayDirection = new List<Vector2>
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    private readonly Dictionary<string, Color> tileColorsDictionary = new Dictionary<string, Color>
    {
        {"defaultColor", Color.white},
        {"selectedColor", Color.yellow},
        {"errorColor", Color.red},
        {"highlightColor", Color.green},
    };

    private List<GameObject> nearTiles = new List<GameObject>();

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
    
    private void SwapTilesOnScene(TileController thisTile, TileController selectedTile)
    {
        HighlightNearTiles();

        if (!IsChosenTileNear())
        {
            StartErrorColorBlinkRoutine(this);
            return;
        }
        
        PlayingFieldController.Instance.SwapTilesInMatrix(thisTile.TileIndex, selectedTile.TileIndex);

        MatchesChecker mc = new MatchesChecker();
        Dictionary<Vector2Int, TypeOfTile> tilesToDelete = mc.GetTilesToDelete();

        if (tilesToDelete.Count == 0)
        {
            //Если пришел пустой словарь - значит совпадения этот ход не вызвал - меняем обратно
            PlayingFieldController.Instance.SwapTilesInMatrix(selectedTile.TileIndex, thisTile.TileIndex);
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

    private void StartErrorColorBlinkRoutine(TileController tileController)
    {
        StartCoroutine(ErrorColorBlinkRoutine(tileController));
    }
    
    private IEnumerator ErrorColorBlinkRoutine(TileController tileController)
    {
        tileController.tileColor.color = tileColorsDictionary["errorColor"];
        yield return  new WaitForSeconds(BlinkTime);
        tileController.tileColor.color = tileColorsDictionary["defaultColor"];
    }

    private void HighlightNearTiles()
    {
        Color color = _isHighlighted ? tileColorsDictionary["defaultColor"] : tileColorsDictionary["highlightColor"];

        foreach (GameObject tile in _selectedTile.nearTiles)
        {
            tile.gameObject.GetComponent<TileController>().SetColor(color);
        }
        _isHighlighted = !_isHighlighted;
    }
}