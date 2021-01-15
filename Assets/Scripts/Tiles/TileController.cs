using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public Vector2Int TileIndex { get; set; }

    private SpriteRenderer tileColor;
    private static TileController _selectedTile;

    private readonly List<Vector2> rayDirection = new List<Vector2>();
    private List<GameObject> nearTiles = new List<GameObject>();

    private void Start()
    {
        tileColor = gameObject.GetComponent<SpriteRenderer>();
        RayDirectionListInit();
    }

    private void RayDirectionListInit()
    {
        rayDirection.Add(Vector2.up);
        rayDirection.Add(Vector2.down);
        rayDirection.Add(Vector2.left);
        rayDirection.Add(Vector2.right);
    }

    private void OnMouseDown()
    {
        if (_selectedTile == this)
        {
            Deselect();
            return;
        }

        if (_selectedTile != null)
        {
            SwapTiles(this, _selectedTile);
            _selectedTile.Deselect();
        }
        else
        {
            Select();
            DetectNearTiles();
        }
    }

    private void SwapTiles(Component thisTile, Component selectedTile)
    {
        if (!IsSwapValid())
            return;

        // Debug.Log($"This tile: {thisTile.name}");
        // Debug.Log($"Selected tile: {selectedTile.name}");

        var thisTileGameObject = thisTile.gameObject;
        var thisTileGameObjectTransformPosition = thisTileGameObject.transform.position;

        var selectedTileGameObject = selectedTile.gameObject;
        var selectedTileGameObjectTransformPosition = selectedTileGameObject.transform.position;

        selectedTileGameObject.transform.position = thisTileGameObjectTransformPosition;
        thisTileGameObject.transform.position = selectedTileGameObjectTransformPosition;
    }


    private void Select()
    {
        SetColor(Color.yellow);
        _selectedTile = this;
    }

    private void Deselect()
    {
        SetColor(Color.white);
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

        foreach (var tile in nearTiles)
        {
           Debug.Log($"Near tiles: {tile.name}");
        }
    }

    private bool IsSwapValid()
    {
        Debug.Log(_selectedTile.nearTiles.Count);
        bool result = false;
        for (int index = 0; index < _selectedTile.nearTiles.Count; index++)
        {
            if (this.gameObject == _selectedTile.nearTiles[index])
            {
                Debug.Log($"This object name: {gameObject.name}");
                Debug.Log($"Near tile name: {_selectedTile.nearTiles[index].name}");
                result = true;
            }
        }
        _selectedTile.nearTiles.Clear();

        return result;
    }
}