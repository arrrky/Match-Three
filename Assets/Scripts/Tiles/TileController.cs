using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    private enum Axis
    {
        X = 0,
        Y = 1,
    }
   
    private Axis chosenAxis;
    private float mouseZpos;   

    private void OnMouseDown()
    {
        mouseZpos = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        chosenAxis = Mathf.Abs(Input.GetAxis("Mouse X")) > Mathf.Abs(Input.GetAxis("Mouse Y")) ? Axis.X : Axis.Y;
    }

    private void OnMouseDrag()
    {   
        MoveTile(chosenAxis);     
    }    
    
    private void MoveTile(Axis chosenAxis)
    {
        Vector3 mouseWorldPosition = GetMouseAsWorldPoint();        

        switch (chosenAxis)
        {
            case Axis.X:
                transform.position = new Vector3(mouseWorldPosition.x, transform.position.y, 0);
                break;
            case Axis.Y:
                transform.position = new Vector3(transform.position.x, mouseWorldPosition.y, 0);
                break;
        }       
    }

    private Vector3 GetMouseAsWorldPoint()
    {        
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mouseZpos;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
