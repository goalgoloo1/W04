using System;
using System.Collections.Generic;
using UnityEngine;


public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class AngelLaser : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public GameObject startObject;
    private int maxReflections = 50;
    public float maxDistance = 100f;
    public Direction startDirection = Direction.Right;
    public LayerMask reflectLayer;
    public Action StartGameAction;
    [SerializeField]
    private bool isClear = false;


    private void Start()
    {
        isClear = false;
    }


    public void GameStart()
    {
        StartGameAction?.Invoke();
        isClear = false;
    }
    
    
    private void Update()
    {
        if (!isClear)
            DrawLaser();
    }
    
    
    private void DrawLaser()
    {
        List<Vector3> points = new List<Vector3>();
        Vector3 direction = transform.right;
        if(startDirection == Direction.Right)
            direction = transform.right;
        else if(startDirection == Direction.Left)
            direction = transform.right* -1;
        else if(startDirection == Direction.Up)
            direction = transform.up;
        else if(startDirection == Direction.Down)
            direction = transform.up * -1;
        
        Vector3 origin = startObject.transform.localPosition ;
        points.Add(origin);
        
        for (int i = 0; i < maxReflections; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance, reflectLayer);
            if (hit.collider != null)
            {
                points.Add(hit.point);

                direction = Vector2.Reflect(direction, hit.normal);
                origin = hit.point + (Vector2)direction.normalized * 0.01f; 

                if (hit.collider.CompareTag("Goal"))
                {
                    Debug.Log("goal");
                    isClear = true;
                    MonsterManager.Instance.SetCommon(4);
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
