using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    private SpriteRenderer tileColor;
    private static TileController selectedTile;


    void Start()
    {
        tileColor = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if (selectedTile!= null)
        {
            selectedTile.Deselect();
        }
        selectedTile = this;
        Select();
    }

    protected void Select()
    {
        tileColor.color = Color.yellow;     
    }

    protected void Deselect()
    {
        tileColor.color = Color.white;    
    }

    private void SwapTiles()
    {

    }

}
