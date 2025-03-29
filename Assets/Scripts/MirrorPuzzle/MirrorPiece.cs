using System;
using UnityEngine;

public class MirrorPiece : MonoBehaviour
{
    [HideInInspector]
    public GameObject goalColl;
    private Vector3 offset;
    [HideInInspector]
    public bool isDragging = false;
    [HideInInspector]
    public bool isCompleted = false;
    
    
    private void OnMouseDown()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        offset = transform.position - mousePos;
        isDragging = true;
    }
    
    private void OnMouseDrag()
    {
        if (isDragging && !isCompleted)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            transform.position = mousePos + offset;
        }
    }
    
    private void OnMouseUp()
    {
        isDragging = false;
    }


    
}
