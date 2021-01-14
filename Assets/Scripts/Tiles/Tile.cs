using UnityEngine;

public enum Type
{
    Default  = 0,
    Circle   = 1,
    Diamond  = 2,
    Square   = 3,
    Star     = 4,
    Triangle = 5,
}

// TODO - подумать - нужен ли такой класс вообще

public class Tile
{
    private Type typeOfTile;
    private GameObject tilePrefab;

    public Type TypeOfTile { get => typeOfTile; set => typeOfTile = value; }
    public GameObject TilePrefab { get => tilePrefab; set => tilePrefab = value; }  
  
    public Tile(Type typeOfTile)
    {
        this.typeOfTile = typeOfTile;        
        this.tilePrefab = null;
    }    
}