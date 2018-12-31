using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUIController : MonoBehaviour
{
    public bool UIFaceCamera = true;

    private Transform target;

    private Camera cam;
    private Vector2 LookAtPoint;

    private void Start()
    {
        if(target == null)
        {
            target = Camera.main.transform;
        }
        if(cam == null)
        {
            cam = Camera.main;
        }

        LookAtPoint =  new Vector2(cam.pixelWidth / 2, 0);
    }

    private void LateUpdate()
    {
        if (UIFaceCamera)
        {
            LookAtCamera();
            

        }
        




    }

    private void LookAtCamera()
    {
        Vector3 point = new Vector3();

        point = cam.ScreenToWorldPoint(new Vector3(LookAtPoint.x, LookAtPoint.y, cam.nearClipPlane));

        Vector3 direction = new Vector3(
            point.x - transform.position.x,
            point.y - transform.position.y,
            point.z - transform.position.z
            );

        transform.forward = -direction;



    }



}
