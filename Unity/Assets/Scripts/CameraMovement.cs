using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityRTSCore;

public class CameraMovement : MonoBehaviour
{
    

    public double ScrollSpeed = 5;
    public double ScreenEdgeBuffer = 10;

    public ScreenEdgeDirection MousePos;
    



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // Get mouse position 
        CheckMousePos();

        // Check if mouse is near screen boundry
        // if it is move camera in the direction of the mouse in world space

    }

    private void CheckMousePos()
    {
        Vector2 mousePos = Input.mousePosition;
        mousePos.x -= Screen.width / 2;
        mousePos.y -= Screen.height / 2;



        
    }
}
