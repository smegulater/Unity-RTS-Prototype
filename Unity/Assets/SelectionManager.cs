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

    public List<SelectionController> selectedObjects;
    public SelectionController[] selectableObjects;
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

        selectedObjects = new List<SelectionController>();

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
        GetSelectableObjects();
        GetSelectedObjects();

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
                DeselectObjects();
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

    
    private void GetSelectableObjects()
    {
        selectableObjects = FindObjectsOfType<SelectionController>();
    }
    private void GetSelectedObjects()
    {
        //DeselectUnits(); 
        selectedObjects.Clear();
        foreach (var selectableObject in selectableObjects)
        {
            if (selectableObject.Selected)
            {
                selectedObjects.Add(selectableObject);
            }

        }
    }

    private void Select(bool multiSelect)
    {
        // TODO - Add GUI check to nto select units if GUI selected

        RaycastHit rayHit;

        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out rayHit, Mathf.Infinity, SelectableUnit))
        {
            SelectionController selectedObject = rayHit.collider.GetComponent<SelectionController>();
            if(selectedObject == null)
            {
                Debug.Log(RTSCore.BuildString("Could not find Selection Controller on clicked object. Object Name:", rayHit.collider.name));
            }

            if (multiSelect)
            {
                if (selectedObject.CompareTag("Unit"))
                {
                    selectedObject.ToggleSelection();
                }
                
            }
            else
            {
                SelectObjects(selectedObject);
            }

        }
        else if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit, Mathf.Infinity, SelectableBuilding))
        {
            Debug.Log("We Clicked a building!");

            SelectionController selectedObject = rayHit.collider.GetComponent<SelectionController>();

            if (selectedObject == null)
            {
                Debug.Log(RTSCore.BuildString("Could not find Selection Controller on clicked object. Object Name:", rayHit.collider.name));
            }

            if (multiSelect)
            {
                Debug.Log("We clicked with Ctrl");
                if(selectedObjects.Count == 0)
                {
                    Debug.Log("Nothing else selected, selecting Building"); ;
                    SelectObjects(selectedObject);
                }
            }
            else
            {
                Debug.Log("Selecting Building");
                SelectObjects(selectedObject);
            }

        }
        //else
        //{
        //    Debug.Log("We did not hit anything!");
        //}



    }

    private void SelectObjects(params SelectionController[] selectionController)
    {
        foreach (SelectionController sc in selectionController)
        {
            sc.SetSelected();
        }
    }

    private void MultiSelect(bool addSelect)
    {

        if (addSelect)
        {
            foreach (var selectableObject in selectableObjects)
            {
                if(IsWithinSelectionBounds(selectableObject))
                {
                    if (selectableObject.MultiSelectable)
                    {
                        SelectObjects(selectableObject);
                    }
                }
            }
        }
        else
        {
            DeselectObjects();

            foreach (var selectableObject in selectableObjects)
            {
                if (IsWithinSelectionBounds(selectableObject))
                {
                    if (selectableObject.MultiSelectable)
                    {
                        SelectObjects(selectableObject);
                    }
                }
            }
        }
    }

    private void MultiSelectRemove()
    {
        foreach (var selectableObject in selectableObjects)
        {
            if (IsWithinSelectionBounds(selectableObject))
            {
                if (selectableObject.MultiSelectable)
                {
                    DeselectObject(selectableObject);
                }
            }
        }
    }

    private void DeselectObject(SelectionController sc)
    {
        sc.SetNotSelected();
    }
    private void DeselectObjects()
    {
        foreach (SelectionController sc in selectedObjects)
        {
            sc.SetNotSelected();
        }
    }

    private bool IsWithinSelectionBounds(SelectionController selectionController)
    {
        //if (!dragging)
        //    return false;

        var camera = Camera.main;
        var viewportBounds = RTSCore.GetViewportBounds(camera, mouseStartPosition, mouseCurrentPosition);
        return viewportBounds.Contains(camera.WorldToViewportPoint(selectionController.transform.position));
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
