using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    public bool Selected = false;

    public bool MultiSelectable = false;
    //public bool Selected;

    public void ToggleSelection()
    {
        Selected = !Selected;
    }
    public void SetSelected()
    {
        Selected = true;
    }
    public void SetNotSelected()
    {
        Selected = false;
    }
}
