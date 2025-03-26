using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class WiringPuzzleManager : MonoBehaviour
{
    private float leftX = 31f;
    private float rightX = 19f;
    private float spacingZ = 2f;  // Changed from spacingY to spacingZ for 90-degree rotation
    private float lineWidth = 0.1f;
    private float yPosition = 1f; // Fixed Y position for all points
    private float zOffset = 4f;   // Z축 방향으로 +4 이동을 위한 오프셋

    [SerializeField] private Material lineMaterial;
    [SerializeField] private Text solvedText;

    // 퍼즐 완료 이벤트 클리어하면 클리어 디버그 뱉음 (외부 클래스에서 구독해서 사용)
    public event Action OnPuzzleCompleted;

    // Internal class: Endpoint information
    private class Endpoint
    {
        public GameObject obj;
        public string wireName;
        public Color color;
        public bool connected;

        public Endpoint(GameObject obj, string wireName, Color color)
        {
            this.obj = obj;
            this.wireName = wireName;
            this.color = color;
            this.connected = false;
        }
    }

    private class WireConnection
    {
        public Vector3 startPos;
        public Vector3 endPos;
        public Color color;
        public GameObject lineObj;

        public WireConnection(Vector3 start, Vector3 end, Color col, GameObject lineObj)
        {
            this.startPos = start;
            this.endPos = end;
            this.color = col;
            this.lineObj = lineObj;
        }
    }

    private List<Endpoint> leftEndpoints = new List<Endpoint>();
    private List<Endpoint> rightEndpoints = new List<Endpoint>();
    private List<WireConnection> connections = new List<WireConnection>();

    private Endpoint currentDraggingEndpoint = null;
    private LineRenderer tempLineRenderer;

    void Start()
    {
        CreateWireEndpoints();
        SetupDraggingLine();
    }

    void Update()
    {
        HandleMouseInput();
    }

    private void CreateWireEndpoints()
    {
        // 와이어 색상 및 식별자 설정
        var wireColors = GetWireColors();
        int numWires = wireColors.Count;

        // 왼쪽 엔드포인트 생성
        CreateLeftEndpoints(wireColors, numWires);

        // 오른쪽 엔드포인트 생성 (색상 순서 무작위)
        CreateRightEndpoints(wireColors, numWires);
    }

    private List<(string, Color)> GetWireColors()
    {
        return new List<(string, Color)>
        {
            ("red",    Color.red),
            ("blue",   Color.blue),
            ("green",  Color.green),
            ("yellow", Color.yellow),
        };
    }

    private void CreateLeftEndpoints(List<(string, Color)> wireColors, int numWires)
    {
        for (int i = 0; i < numWires; i++)
        {
            float z = CalculateEndpointZ(i, numWires) + zOffset;  // zOffset 추가
            Vector3 pos = new Vector3(leftX, yPosition, z);
            GameObject epObj = CreateEndpoint($"Left_{wireColors[i].Item1}", pos, wireColors[i].Item2);
            leftEndpoints.Add(new Endpoint(epObj, wireColors[i].Item1, wireColors[i].Item2));
        }
    }

    private void CreateRightEndpoints(List<(string, Color)> wireColors, int numWires)
    {
        List<(string, Color)> shuffled = new List<(string, Color)>(wireColors);
        ShuffleList(shuffled);

        for (int i = 0; i < numWires; i++)
        {
            float z = CalculateEndpointZ(i, numWires) + zOffset;  // zOffset 추가
            Vector3 pos = new Vector3(rightX, yPosition, z);
            GameObject epObj = CreateEndpoint($"Right_{shuffled[i].Item1}", pos, shuffled[i].Item2);
            rightEndpoints.Add(new Endpoint(epObj, shuffled[i].Item1, shuffled[i].Item2));
        }
    }

    private float CalculateEndpointZ(int index, int totalCount)
    {
        return (index - (totalCount - 1) / 2f) * spacingZ;
    }

    private void SetupDraggingLine()
    {
        GameObject tempLineObj = new GameObject("TempLine");
        tempLineRenderer = tempLineObj.AddComponent<LineRenderer>();
        tempLineRenderer.material = lineMaterial;
        tempLineRenderer.startWidth = lineWidth;
        tempLineRenderer.endWidth = lineWidth;
        tempLineRenderer.positionCount = 2;
        tempLineRenderer.enabled = false;
    }


    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseDown();
        }
        else if (Input.GetMouseButton(0) && currentDraggingEndpoint != null)
        {
            HandleMouseDrag();
        }
        else if (Input.GetMouseButtonUp(0) && currentDraggingEndpoint != null)
        {
            HandleMouseUp();
        }
    }

    private void HandleMouseDown()
    {
        Vector3 worldPos = GetMouseWorldPosition();

        foreach (var endpoint in leftEndpoints)
        {
            if (!endpoint.connected && IsNearEndpoint(worldPos, endpoint.obj.transform.position))
            {
                StartDragging(endpoint, worldPos);
                break;
            }
        }
    }

    private void HandleMouseDrag()
    {
        Vector3 worldPos = GetMouseWorldPosition();
        UpdateDragLine(worldPos);
    }

    private void HandleMouseUp()
    {
        Vector3 worldPos = GetMouseWorldPosition();

        foreach (var endpoint in rightEndpoints)
        {
            if (!endpoint.connected && IsNearEndpoint(worldPos, endpoint.obj.transform.position))
            {
                TryConnect(endpoint);
                break;
            }
        }

        StopDragging();
        CheckPuzzleCompletion();
    }

    private Vector3 GetMouseWorldPosition()
    {
        // Create a ray from the camera through the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Define a plane at the Y position (with Z offset)
        Plane plane = new Plane(Vector3.up, new Vector3(0, yPosition, zOffset));

        float distance;
        if (plane.Raycast(ray, out distance))
        {
            // Return the point where the ray intersects the plane
            return ray.GetPoint(distance);
        }

        // Fallback if no intersection (shouldn't happen if camera is properly positioned)
        return Vector3.zero;
    }

    private bool IsNearEndpoint(Vector3 position, Vector3 endpointPosition)
    {
        return Vector3.Distance(position, endpointPosition) < 0.5f;
    }

    private void StartDragging(Endpoint endpoint, Vector3 position)
    {
        currentDraggingEndpoint = endpoint;
        tempLineRenderer.enabled = true;
        tempLineRenderer.SetPosition(0, endpoint.obj.transform.position);
        tempLineRenderer.SetPosition(1, position);
    }

    private void UpdateDragLine(Vector3 position)
    {
        tempLineRenderer.SetPosition(1, position);
    }

    private void TryConnect(Endpoint rightEndpoint)
    {
        if (rightEndpoint.wireName == currentDraggingEndpoint.wireName)
        {
            ConnectEndpoints(currentDraggingEndpoint, rightEndpoint);
        }
    }

    private void ConnectEndpoints(Endpoint leftEndpoint, Endpoint rightEndpoint)
    {
        leftEndpoint.connected = true;
        rightEndpoint.connected = true;

        CreatePermanentLine(
            leftEndpoint.obj.transform.position,
            rightEndpoint.obj.transform.position,
            leftEndpoint.color,
            leftEndpoint.wireName
        );
    }

    private void StopDragging()
    {
        currentDraggingEndpoint = null;
        tempLineRenderer.enabled = false;
    }

    private void CheckPuzzleCompletion()
    {
        if (AllConnected())
        {
            CompletePuzzle();
        }
    }

    private void CompletePuzzle()
    {
        Debug.Log("Puzzle completed!");
        if (solvedText != null)
        {
            solvedText.gameObject.SetActive(true);
        }

        // 퍼즐 완료 이벤트 발생
        OnPuzzleCompleted?.Invoke();
    }

    GameObject CreateEndpoint(string endpointName, Vector3 pos, Color color)
    {
        GameObject ep = new GameObject(endpointName);
        ep.transform.position = pos;

        // Sprite renderer
        SpriteRenderer sr = ep.AddComponent<SpriteRenderer>();
        sr.sprite = GenerateCircleSprite(32, color);
        sr.color = Color.white;

        // Make the sprite face upward (rotated 90 degrees around X axis)
        ep.transform.rotation = Quaternion.Euler(90, 0, 0);

        // Add BoxCollider instead of CircleCollider2D for 3D space
        BoxCollider col = ep.AddComponent<BoxCollider>();
        col.isTrigger = true;
        col.size = new Vector3(1f, 0.1f, 1f);

        return ep;
    }

    Sprite GenerateCircleSprite(int size, Color color)
    {
        Texture2D tex = new Texture2D(size, size, TextureFormat.ARGB32, false);
        tex.filterMode = FilterMode.Point;

        float r = size / 2f;
        Vector2 center = new Vector2(r, r);

        // Set pixels: color inside circle, transparent outside
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dist = Vector2.Distance(center, new Vector2(x, y));
                if (dist <= r)
                    tex.SetPixel(x, y, color);
                else
                    tex.SetPixel(x, y, Color.clear);
            }
        }
        tex.Apply();

        // Create sprite with centered pivot
        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
    }

    void CreatePermanentLine(Vector3 startPos, Vector3 endPos, Color color, string wireName)
    {
        GameObject lineObj = new GameObject($"Line_{wireName}");
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();

        lr.material = lineMaterial;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.positionCount = 2;
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, endPos);
        lr.startColor = color;
        lr.endColor = color;

        connections.Add(new WireConnection(startPos, endPos, color, lineObj));
    }

    bool AllConnected()
    {
        return !HasDisconnectedEndpoint(leftEndpoints) && !HasDisconnectedEndpoint(rightEndpoints);
    }

    bool HasDisconnectedEndpoint(List<Endpoint> endpoints)
    {
        foreach (var ep in endpoints)
        {
            if (!ep.connected) return true;
        }
        return false;
    }

    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randIndex = UnityEngine.Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randIndex];
            list[randIndex] = temp;
        }
    }
}

