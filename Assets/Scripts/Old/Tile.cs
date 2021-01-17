using UnityEngine;

public class Tile
{
    private TypeOfTile typeOfTile;
    private GameObject tilePrefab;

    public TypeOfTile TypeOfTile { get => typeOfTile; set => typeOfTile = value; }
    public GameObject TilePrefab { get => tilePrefab; set => tilePrefab = value; }  
  
    public Tile(TypeOfTile typeOfTile)
    {
        this.typeOfTile = typeOfTile;        
        this.tilePrefab = null;
    }    
}