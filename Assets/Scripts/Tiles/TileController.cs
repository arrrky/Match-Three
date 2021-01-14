using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    private PlayingFieldController playingFieldController;

    private Vector2Int tileIndex;    
    private Vector2 defaultPosition;

    private enum Axis
    {
        X = 0,
        Y = 1,
    }

    private Axis chosenAxis;
    private float mouseZpos;
    private Vector3 startMousePosition;
    private Vector3 endMousePosition;

    public PlayingFieldController PlayingFieldController { get => playingFieldController; set => playingFieldController = value; }
    public Vector2Int TileIndex { get => tileIndex; set => tileIndex = value; }
    public Vector2 DefaultPosition { get => defaultPosition; set => defaultPosition = value; }

    public void Init(PlayingFieldController playingFieldController, Vector2Int tileIndex)
    {
        this.playingFieldController = playingFieldController;
        this.tileIndex = tileIndex;
    }

    private void Start()
    {
        DefaultPosition = transform.position;
    }

    private void OnMouseDown()
    {

        // mouseZpos = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        chosenAxis = Mathf.Abs(Input.GetAxis("Mouse X")) > Mathf.Abs(Input.GetAxis("Mouse Y")) ? Axis.X : Axis.Y;
        startMousePosition = GetMouseAsWorldPoint();
        //Debug.Log($"Start mouse pos: {startMousePosition}");

    }

    private void OnMouseUp()
    {
        endMousePosition = GetMouseAsWorldPoint();
        //Debug.Log($"End mouse pos: {endMousePosition}");
        Collider2D nearTile = DetectNearTile();
        if(nearTile != null && playingFieldController.IsSwapValid(tileIndex))
        {
            Debug.Log(nearTile.name);
            SwapTiles(gameObject, nearTile.gameObject);
        }
        else
        {
            ReturnTileInToDefaultPosition();
        }

    }

    private void OnMouseDrag()
    {
        MoveTile(chosenAxis);
    }

    private void MoveTile(Axis chosenAxis)
    {

        // TODO - пока что работает очень плохо, доработать
        Vector3 mouseWorldPosition = GetMouseAsWorldPoint();

        //switch (chosenAxis)
        //{
        //    case Axis.X:
        //        transform.position = new Vector3(mouseWorldPosition.x, transform.position.y, 0);
        //        break;
        //    case Axis.Y:
        //        transform.position = new Vector3(transform.position.x, mouseWorldPosition.y, 0);
        //        break;
        //}

        transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0);

    }

    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;
        //mousePoint.z = mouseZpos;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void SwapTiles(GameObject thisTile, GameObject tileToSwap)
    {
        //Vector3 tempTransform;
        //tempTransform = thisTile.position;
        
        thisTile.transform.position = tileToSwap.transform.position;
        tileToSwap.transform.position = DefaultPosition;
    }

    private Collider2D DetectNearTile()
    {
        Vector2 directionOfRay = Vector2.zero;

        if (Input.GetAxis("Mouse X") > 0)
        {
            directionOfRay = Vector2.right;
        }
        if (Input.GetAxis("Mouse X") < 0)
        {
            directionOfRay = Vector2.left;
        }
        if (Input.GetAxis("Mouse Y") > 0)
        {
            directionOfRay = Vector2.up;
        }
        if (Input.GetAxis("Mouse Y") < 0)
        {
            directionOfRay = Vector2.down;
        }
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionOfRay, playingFieldController.SpriteShift.x);


        return hit.collider;
    }

    private void ReturnTileInToDefaultPosition()
    {
        transform.position = DefaultPosition;
    }   
}
