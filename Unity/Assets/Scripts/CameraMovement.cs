using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityRTSCore;

public class CameraMovement : MonoBehaviour
{
    

    public float ScrollSpeed = 0.125f;
    public float ScreenEdgeBuffer = 150f;

    public float ZoomSpeed = 2.125f;
    public float ZoomSmooth = 100f;
    private float MaxZoom = 0f;
    private float MinZoom = -7f;

    public bool freeMove = false;


    public Text DebugText;

    private float screenHeight = Screen.height / 2;
    private float screenWidth = Screen.width / 2;
    private ScreenEdgeDirection MousePos;

    private float mouseWheel = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            freeMove = !freeMove;
        }

        

        CheckMouseState();
        MoveCamera();
        ZoomCamera();


    }

    private void ZoomCamera()
    {
        //if(mouseWheel == 0)
        //{
        //    return;
        //}

        Vector3 currentPos = transform.position;

        Vector3 targetPos = new Vector3(currentPos.x, Mathf.Clamp(currentPos.y + (mouseWheel * ZoomSpeed),MinZoom,MaxZoom),currentPos.z);

        transform.position = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * ZoomSmooth);







    }

    private void MoveCamera()
    {
        if (!freeMove)
        {
            return;
        }

        switch (MousePos)
        {
            case ScreenEdgeDirection.Up:
                transform.Translate(new Vector3(0, 0, ScrollSpeed));
                break;
            case ScreenEdgeDirection.Down:
                transform.Translate(new Vector3(0, 0, -ScrollSpeed));
                break;
            case ScreenEdgeDirection.Left:
                transform.Translate(new Vector3( -ScrollSpeed, 0, 0));
                break;
            case ScreenEdgeDirection.Right:
                transform.Translate(new Vector3(ScrollSpeed, 0, 0));
                break;
            case ScreenEdgeDirection.LeftUp:
                transform.Translate(new Vector3( -(ScrollSpeed/2), 0, ScrollSpeed / 2));
                break;
            case ScreenEdgeDirection.RightUp:
                transform.Translate(new Vector3(ScrollSpeed / 2, 0, ScrollSpeed / 2));
                break;
            case ScreenEdgeDirection.RightDown:
                transform.Translate(new Vector3(ScrollSpeed / 2, 0, -(ScrollSpeed / 2)) );
                break;
            case ScreenEdgeDirection.LeftDown:
                transform.Translate(new Vector3( -(ScrollSpeed / 2), 0, -ScrollSpeed / 2));
                break;
            case ScreenEdgeDirection.Neutral:
                break;
            default:
                break;
        }
    }

    private void CheckMouseState()
    {
        Vector2 mousePos = Input.mousePosition;
        mousePos.x -= screenWidth;
        mousePos.y -= screenHeight;

        #region ScreenEdgeDetection
        if ((mousePos.x < -(screenWidth - ScreenEdgeBuffer)) && (mousePos.y < (screenHeight - ScreenEdgeBuffer)) && (mousePos.y > -(screenHeight - ScreenEdgeBuffer)))
        {
            MousePos = ScreenEdgeDirection.Left;
        }
        else if ((mousePos.x > (screenWidth - ScreenEdgeBuffer)) && (mousePos.y < (screenHeight - ScreenEdgeBuffer)) && (mousePos.y > -(screenHeight - ScreenEdgeBuffer)))
        {
            MousePos = ScreenEdgeDirection.Right;
        }
        else if ((mousePos.y > (screenHeight - ScreenEdgeBuffer)) && (mousePos.x > -(screenWidth - ScreenEdgeBuffer)) && (mousePos.x < (screenWidth - ScreenEdgeBuffer)))
        {
            MousePos = ScreenEdgeDirection.Up;
        }
        else if ((mousePos.y < -(screenHeight - ScreenEdgeBuffer)) && (mousePos.x > -(screenWidth - ScreenEdgeBuffer)) && (mousePos.x < (screenWidth - ScreenEdgeBuffer)))
        {
            MousePos = ScreenEdgeDirection.Down;
        }
        else if ((mousePos.x < -(screenWidth - ScreenEdgeBuffer)) && (mousePos.y > (screenHeight - ScreenEdgeBuffer)))
        {
            MousePos = ScreenEdgeDirection.LeftUp;
        }
        else if ((mousePos.x > (screenWidth - ScreenEdgeBuffer)) && (mousePos.y > (screenHeight - ScreenEdgeBuffer)))
        {
            MousePos = ScreenEdgeDirection.RightUp;
        }
        else if ((mousePos.x > (screenWidth - ScreenEdgeBuffer)) && (mousePos.y < -(screenHeight - ScreenEdgeBuffer)))
        {
            MousePos = ScreenEdgeDirection.RightDown;
        }
        else if ((mousePos.x < -(screenWidth - ScreenEdgeBuffer)) && (mousePos.y < -(screenHeight - ScreenEdgeBuffer)))
        {
            MousePos = ScreenEdgeDirection.LeftDown;
        }
        else
        {
            MousePos = ScreenEdgeDirection.Neutral;
        }
        #endregion


        mouseWheel = Mathf.Clamp(Input.GetAxisRaw("Mouse ScrollWheel")*100,-1,1);

        DebugOverlayManager.instance.AddMetric("Mouse cords", mousePos);
        DebugOverlayManager.instance.AddMetric("Screen Width", Screen.width);
        DebugOverlayManager.instance.AddMetric("Screen Height", Screen.height);
        DebugOverlayManager.instance.AddMetric("Mouse Position", MousePos);
        DebugOverlayManager.instance.AddMetric("Mouse Wheel", mouseWheel);
        DebugOverlayManager.instance.AddMetric("Free Move Enabled", freeMove);

      


    }
}
