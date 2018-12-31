using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionBoxController : MonoBehaviour
{
    public Image selectionBoxImage;
    public BoxCollider selectionBoxCollider;
    public Canvas Canvas;

    private void Start()
    {
        Canvas.worldCamera = Camera.main;
    }

    public void SetPosSize(Vector3 Center, float Width, float Height)
    {

        transform.position = Center;

        selectionBoxImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Width);
        selectionBoxImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Height);
        
        Width = Mathf.Round(Width);
        Height = Mathf.Round(Height);

        
       selectionBoxCollider.size = new Vector3(Width, Height, 0.1f);


    }
}
