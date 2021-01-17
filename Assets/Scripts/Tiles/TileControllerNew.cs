using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class TileControllerNew : MonoBehaviour
{
    public Vector2Int TileIndex { get; set; }
    private SpriteRenderer tileRenderer;
    
    private static TileControllerNew _selectedTile;
    private static bool _isHighlighted;

    private readonly Dictionary<string, Color> tileColorsDictionary = new Dictionary<string, Color>
    {
        {"defaultColor", Color.white},
        {"selectColor", Color.yellow},
        {"errorColor", Color.red},
        {"highlightColor", Color.green},
    };
    
    private const float BlinkTime = 0.3f;
    
    private readonly List<Vector2> rayDirection = new List<Vector2>
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    private List<GameObject> nearTiles = new List<GameObject>();

    private void Start()
    {
        tileRenderer = GetComponent<SpriteRenderer>();
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
            HighlightNearTiles();

            if (IsChosenTileNear())
            {
                PlayingFieldControllerNew.Instance.SwapTiles(_selectedTile.TileIndex, this.TileIndex);
            }
            else
            {
                StartErrorColorBlinkRoutine(this);
            }

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
        SetColor(tileColorsDictionary["selectColor"]);
        _selectedTile = this;
    }

    private void Deselect()
    {
        SetColor(tileColorsDictionary["defaultColor"]);
        _selectedTile = null;
    }

    private void SetColor(Color colorOtTile)
    {
        tileRenderer.color = colorOtTile;
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
    
    private void StartErrorColorBlinkRoutine(TileControllerNew tileController)
    {
        StartCoroutine(ErrorColorBlinkRoutine(tileController));
    }
    
    private IEnumerator ErrorColorBlinkRoutine(TileControllerNew tileController)
    {
        tileController.tileRenderer.color = tileColorsDictionary["errorColor"];
        yield return  new WaitForSeconds(BlinkTime);
        tileController.tileRenderer.color = tileColorsDictionary["defaultColor"];
    }
    
    private void HighlightNearTiles()
    {
        Color color = _isHighlighted ? tileColorsDictionary["defaultColor"] : tileColorsDictionary["highlightColor"];

        foreach (GameObject tile in _selectedTile.nearTiles)
        {
            tile.gameObject.GetComponent<TileControllerNew>().SetColor(color);
        }
        _isHighlighted = !_isHighlighted;
    }

}
