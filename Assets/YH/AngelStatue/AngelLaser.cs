using System;
using System.Collections.Generic;
using UnityEngine;

public class AngelLaser : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int maxReflections = 5;
    public float maxDistance = 100f;
    public LayerMask reflectLayer;
    


    private void Update()
    {
        DrawLaser();
    }
    
    
    private void DrawLaser()
    {
        List<Vector3> points = new List<Vector3>();
        Vector3 direction = transform.right;
        Vector3 origin = transform.localPosition;
        
        points.Add(origin);
        
        for (int i = 0; i < maxReflections; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance, reflectLayer);
            if (hit.collider != null)
            {
                points.Add(hit.point);

                direction = Vector2.Reflect(direction, hit.normal);
                origin = hit.point;

                if (hit.collider.CompareTag("Goal"))
                {
                    Debug.Log("goal");
                    break;
                }
            }
            else
            {
                points.Add(origin+direction*maxDistance);
                break;
            }
        }
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());

    }
}
