using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static SelectionManager instance;

    public LayerMask SelectableUnit;
    public LayerMask SelectableBuilding;
    public LayerMask Ground;



    public List<UnitController> selectedUnits;

    public GameObject RightAngleMarker;
    public GameObject SelectionBox;
    private GameObject selectionBox;

    private Vector3 mouseDragStart;
    private Vector3 mouseDragStartScreen;
    private Vector3 mouseDragEnd;
    private Vector3 mouseDragEndScreen;
    private bool dragging = false;

    private void Start()
    {
        #region Singleton Check
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Debug.LogError("Attempted to create multiples instances of the DebugOverlayManager Script");
            Destroy(this);
        }
        #endregion

        selectedUnits = new List<UnitController>();
        mouseDragStart = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {

            mouseDragStart = GetMousePoint();
            mouseDragStartScreen = Input.mousePosition;
            DebugOverlayManager.instance.AddMetric("Mouse Drag Start", mouseDragStart);
            DebugOverlayManager.instance.AddMetric("Mouse Drag Start pos", Input.mousePosition);
            Select();
        }

        if (Input.GetMouseButton(0))
        {
            dragging = true;

            mouseDragEnd = GetMousePoint();
            mouseDragEndScreen = Input.mousePosition;
            DebugOverlayManager.instance.AddMetric("Mouse Drag End", mouseDragEnd);
            DebugOverlayManager.instance.AddMetric("Mouse Drag Start End", Input.mousePosition);

            DrawSelectionBox();
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
            if(selectionBox != null)
            {
                //Destroy(selectionBox);
            }
            //MultiSelect();
        }

        
        


    }

    

    private void DrawSelectionBox()
    {
        GetRightAngleScreenPoint();

        Vector3 midPoint = Vector3.Lerp(mouseDragStart, mouseDragEnd, 0.5f);
        midPoint.y = 1.125f;


        Vector3 thirdPoint = GetRightAngleScreenPoint();
        Instantiate(RightAngleMarker, thirdPoint, Quaternion.identity);

        float height = Vector3.Distance(mouseDragStart, thirdPoint);
        float width = Vector3.Distance(mouseDragEnd, thirdPoint);

        DebugOverlayManager.instance.AddMetric("width", width);
        DebugOverlayManager.instance.AddMetric("Height", height);
        

        

        DebugOverlayManager.instance.AddMetric("Centre Point", midPoint); ;

        if (selectionBox == null)
        {
            selectionBox = Instantiate(SelectionBox, midPoint, Quaternion.identity);
        }

        SelectionBoxController sb = selectionBox.GetComponent<SelectionBoxController>();

        sb.SetPosSize(midPoint, width, height);

        



    }

    private Vector3 GetRightAngleScreenPoint()
    {
        float hypo = Vector3.Distance(mouseDragStartScreen, mouseDragEndScreen);
        
        float height = hypo * Mathf.Sin(32);
        //float width = Mathf.Sqrt(Mathf.Pow(hypo, 2) - Mathf.Pow(height, 2));

        Vector3 point = new Vector3(mouseDragStartScreen.x, mouseDragStartScreen.z + height);

        RaycastHit rayInfo;
        Physics.Raycast(Camera.main.ScreenPointToRay(point), out rayInfo, Mathf.Infinity, Ground);


        return new Vector3(rayInfo.point.x, 1.125f, rayInfo.point.z);


    }
    private Vector3 GetMousePoint()
    {
        RaycastHit rayInfo;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayInfo, Mathf.Infinity, Ground);

        Vector3 rv = new Vector3(rayInfo.point.x, 1.125f, rayInfo.point.z);

        return rv;
    }


    private void Select()
    {
        // TODO - Add GUI check to nto select units if GUI selected

        RaycastHit rayHit;

        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out rayHit, Mathf.Infinity, SelectableUnit))
        {
            UnitController selectedUnit = rayHit.collider.GetComponent<UnitController>();

            SelectUnits(selectedUnit);

        }
        else
        {
            DeselectUnits();
        }

        
    }

    private void DeselectUnits()
    {
        foreach (UnitController unit in selectedUnits)
        {
            unit.Selected = false;
        }

        selectedUnits.Clear();
    }

    private void SelectUnits(params UnitController[] units)
    {
        foreach (UnitController unit in units)
        {
            unit.Selected = true;
            selectedUnits.Add(unit);
        }
    }


}
