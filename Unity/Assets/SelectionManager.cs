using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityRTSCore;

public class SelectionManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static SelectionManager instance;

    public LayerMask SelectableUnit;
    public LayerMask SelectableBuilding;
    public LayerMask Ground;

    public Color SelectionColor = Color.white;
    public float BorderThickness = 2f;

    public List<UnitController> selectedUnits;
    public UnitController[] selectableUnits;
    private Vector3 mouseStartPosition;
    private Vector3 mouseCurrentPosition;
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

    }

    #region Debug Metrics
    private void UpdateDebugConsole()
    {

        DebugOverlayManager.instance.AddMetric("Mouse Start Position", mouseStartPosition);
        DebugOverlayManager.instance.AddMetric("Mouse Current Position", mouseCurrentPosition);
        DebugOverlayManager.instance.AddMetric("Dragging?", dragging);

    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        GetSelectableUnits();
        GetSelectedUnits();

        if (Input.GetMouseButtonDown(0))
        {
            if(Input.GetKey(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.LeftControl))
            {

                mouseStartPosition = Input.mousePosition;
                Select(true);
            }
            else
            {
                mouseStartPosition = Input.mousePosition;
                DeselectUnits();
                Select(false);
            }
            
        }

        if (Input.GetMouseButton(0))
        {
            mouseCurrentPosition = Input.mousePosition;

            if(mouseStartPosition == mouseCurrentPosition)
            {
                dragging = false;
                return;
                
            }

            if(Input.GetKey(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.LeftControl))
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftShift))
                {
                    dragging = true;
                    mouseCurrentPosition = Input.mousePosition;
                    MultiSelectRemove();
                }
                else
                {
                    dragging = true;
                    mouseCurrentPosition = Input.mousePosition;
                    MultiSelect(true);
                }
                    
            }
            else
            {
                dragging = true;
                mouseCurrentPosition = Input.mousePosition;
                MultiSelect(false);
            }
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (mouseStartPosition == mouseCurrentPosition)
            {
                return;
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.LeftControl))
                {
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftShift))
                    {
                        dragging = false;
                        MultiSelectRemove();
                    }
                    else
                    {
                        dragging = false;
                        MultiSelect(true);
                    }
                    
                }
                else
                {
                    dragging = false;
                    MultiSelect(false);
                }
            }
        }

        UpdateDebugConsole();
    }

    
    private void GetSelectableUnits()
    {
        selectableUnits = FindObjectsOfType<UnitController>();
    }
    private void GetSelectedUnits()
    {
        //DeselectUnits(); 
        selectedUnits.Clear();
        foreach (var selectableUnit in selectableUnits)
        {
            if (selectableUnit.Selected)
            {
                selectedUnits.Add(selectableUnit);
            }

        }
    }

    private void Select(bool multiSelect)
    {
        // TODO - Add GUI check to nto select units if GUI selected

        RaycastHit rayHit;

        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out rayHit, Mathf.Infinity, SelectableUnit))
        {
            UnitController selectedUnit = rayHit.collider.GetComponent<UnitController>();
            if (multiSelect)
            {
                selectedUnit.Selected = !selectedUnit.Selected;
            }
            else
            {
                SelectUnits(selectedUnit);
            }

        }

        
    }

    private void SelectUnits(params UnitController[] units)
    {
        foreach (UnitController unit in units)
        {
            unit.Selected = true;
        }
    }
    private void MultiSelectRemove()
    {
        foreach (var selectableUnit in selectableUnits)
        {
            if (IsWithinSelectionBounds(selectableUnit))
            {
                DeselectUnit(selectableUnit);
            }
        }
    }
    private void MultiSelect(bool addSelect)
    {

        if (addSelect)
        {
            foreach (var selectableUnit in selectableUnits)
            {
                if(IsWithinSelectionBounds(selectableUnit))
                {
                    SelectUnits(selectableUnit);
                }
            }
        }
        else
        {
            DeselectUnits();

            foreach (var selectableUnit in selectableUnits)
            {
                if (IsWithinSelectionBounds(selectableUnit))
                {
                    SelectUnits(selectableUnit);
                }
                    
            }
        }



    }
    private void DeselectUnits()
    {
        foreach (UnitController unit in selectedUnits)
        {
            unit.Selected = false;
        }
    }
    private void DeselectUnit(UnitController uc)
    {
        uc.Selected = false;
    }

    private bool IsWithinSelectionBounds(UnitController unitController)
    {
        //if (!dragging)
        //    return false;

        var camera = Camera.main;
        var viewportBounds = RTSCore.GetViewportBounds(camera, mouseStartPosition, mouseCurrentPosition);
        return viewportBounds.Contains(camera.WorldToViewportPoint(unitController.transform.position));
    }
    private void OnGUI()
    {
        if(dragging)
        {
            Rect rect = RTSCore.GetScreenRect(mouseStartPosition, mouseCurrentPosition);
            RTSCore.DrawScreenRectBorderShaded(rect, BorderThickness, SelectionColor);
        }

    }







}
