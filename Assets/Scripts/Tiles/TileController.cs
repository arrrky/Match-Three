using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class TileController : MonoBehaviour
{
    private SpriteRenderer tileColor;
    private static TileController _selectedTile;

    private void Start()
    {
        tileColor = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if (_selectedTile == this) return;

        if (_selectedTile != null)
        {
            SwapTiles(this, _selectedTile);
            _selectedTile.Deselect();
        }
        else
        {
            Select();
        }
    }

    private void SwapTiles(Component thisTile, Component selectedTile)
    {
        Debug.Log($"This tile: {thisTile.name}");
        Debug.Log($"Selected tile: {selectedTile.name}");

        var thisTileGameObject = thisTile.gameObject;
        var thisTileGameObjectTransformPosition = thisTileGameObject.transform.position;

        var selectedTileGameObject = selectedTile.gameObject;
        var selectedTileGameObjectTransformPosition = selectedTileGameObject.transform.position;

        selectedTileGameObject.transform.position = thisTileGameObjectTransformPosition;
        thisTileGameObject.transform.position = selectedTileGameObjectTransformPosition;
    }


    private void Select()
    {
        tileColor.color = Color.yellow;
        _selectedTile = this;
    }

    private void Deselect()
    {
        tileColor.color = Color.white;
        _selectedTile = null;
    }
}