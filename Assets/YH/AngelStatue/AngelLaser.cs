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
    public LineRenderer lineRenderer;           // 레이저 라인
    public GameObject startObject;              // 레이저 시작 오브젝트
    private int maxReflections = 50;            // 최대 반사 횟수
    public float maxDistance = 100f;            // 레이저 최대 거리
    public Direction startDirection = Direction.Right; // 시작 방향
    public LayerMask reflectLayer;              // 반사 대상 레이어
    public Action StartGameAction;              // 게임 시작 시 실행할 델리게이트

    [SerializeField]
    private bool isClear = false;               // 클리어 여부

    private void Start()
    {
        isClear = false;

        if (lineRenderer != null)
        {
            lineRenderer.useWorldSpace = true; // 라인렌더러가 월드 좌표계 사용
        }
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

        // 2D 기준 방향 설정
        Vector2 direction = Vector2.right;
        switch (startDirection)
        {
            case Direction.Right:
                direction = Vector2.right;
                break;
            case Direction.Left:
                direction = Vector2.left;
                break;
            case Direction.Up:
                direction = Vector2.up;
                break;
            case Direction.Down:
                direction = Vector2.down;
                break;
        }

        // 월드 좌표 기준 시작점 설정
        Vector2 origin = startObject.transform.position;
        points.Add(origin);

        for (int i = 0; i < maxReflections; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance, reflectLayer);

            if (hit.collider != null)
            {
                points.Add(hit.point);

                // 방향 반사
                direction = Vector2.Reflect(direction, hit.normal);
                origin = hit.point + direction.normalized * 0.01f; // 다음 origin 조금 이동

                // 목표 도달 체크
                if (hit.collider.CompareTag("Goal"))
                {
                    Debug.Log("goal");
                    isClear = true;

                    // 몬스터 상태 업데이트
                    MonsterManager.Instance.SetCommon(4);
                    break;
                }
            }
            else
            {
                // 더 이상 맞는 게 없으면 끝 방향까지 그리기
                points.Add(origin + direction * maxDistance);
                break;
            }
        }

        // 라인 렌더러 갱신
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ConvertAll(p => (Vector3)p).ToArray());
    }
}
