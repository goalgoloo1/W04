using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using static CameraManager;

public class WiringPuzzleManager : MonoBehaviour
{
  
    private float leftX = 71f;
    private float rightX = 79f;
    private float spacingY = 1.5f;  // Back to Y-axis for 2D
    private float lineWidth = 0.1f;

    [SerializeField] private Material lineMaterial;
    [SerializeField] private Text solvedText;
    [SerializeField] private float resetDelay = 2.0f; // Time before resetting the puzzle after completion

    public event Action OnPuzzleCompleted;

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
    private bool puzzleSolved = false;

    void Start()
    {
        InitPuzzle();
    }

    // Initialize (or reset) the puzzle
    public void InitPuzzle()
    {
        // Clear existing objects if any
        CleanupPuzzle();

        // Reset state
        puzzleSolved = false;
        if (solvedText != null)
        {
            solvedText.gameObject.SetActive(false);
        }

        // Create new wire endpoints
        CreateWireEndpoints();
        SetupDraggingLine();
    }

    // Clean up all objects before resetting
    private void CleanupPuzzle()
    {
        // Destroy all left endpoints
        foreach (var endpoint in leftEndpoints)
        {
            if (endpoint.obj != null)
            {
                Destroy(endpoint.obj);
            }
        }
        leftEndpoints.Clear();

        // Destroy all right endpoints
        foreach (var endpoint in rightEndpoints)
        {
            if (endpoint.obj != null)
            {
                Destroy(endpoint.obj);
            }
        }
        rightEndpoints.Clear();

        // Destroy all connections
        foreach (var connection in connections)
        {
            if (connection.lineObj != null)
            {
                Destroy(connection.lineObj);
            }
        }
        connections.Clear();

        // Destroy temp line renderer if it exists
        if (tempLineRenderer != null)
        {
            Destroy(tempLineRenderer.gameObject);
            tempLineRenderer = null;
        }

        currentDraggingEndpoint = null;
    }

    void Update()
    {
        if (!puzzleSolved)
        {
            HandleMouseInput();
        }
    }

    private void CreateWireEndpoints()
    {
        var wireColors = GetWireColors();
        int numWires = wireColors.Count;

        CreateLeftEndpoints(wireColors, numWires);

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
            float y = CalculateEndpointY(i, numWires);
            Vector3 pos = new Vector3(leftX, y, 0); // Using 2D coordinates with Z=0
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
            float y = CalculateEndpointY(i, numWires);
            Vector3 pos = new Vector3(rightX, y, 0); // Using 2D coordinates with Z=0
            GameObject epObj = CreateEndpoint($"Right_{shuffled[i].Item1}", pos, shuffled[i].Item2);
            rightEndpoints.Add(new Endpoint(epObj, shuffled[i].Item1, shuffled[i].Item2));
        }
    }

    private float CalculateEndpointY(int index, int totalCount)
    {
        return (index - (totalCount - 1) / 2f) * spacingY;
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
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0; // Ensure we're in the 2D plane
        return worldPos;
    }

    private bool IsNearEndpoint(Vector3 position, Vector3 endpointPosition)
    {
        // Use 2D distance calculation
        position.z = 0;
        endpointPosition.z = 0;
        return Vector2.Distance(position, endpointPosition) < 0.5f;
    }

    private void StartDragging(Endpoint endpoint, Vector3 position)
    {
        currentDraggingEndpoint = endpoint;
        tempLineRenderer.enabled = true;

        // Ensure Z=0 for 2D
        Vector3 startPos = endpoint.obj.transform.position;
        startPos.z = 0;
        position.z = 0;

        tempLineRenderer.SetPosition(0, startPos);
        tempLineRenderer.SetPosition(1, position);
    }

    private void UpdateDragLine(Vector3 position)
    {
        position.z = 0; // Ensure Z=0 for 2D
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

        // Ensure Z=0 for 2D
        Vector3 startPos = leftEndpoint.obj.transform.position;
        Vector3 endPos = rightEndpoint.obj.transform.position;
        startPos.z = 0;
        endPos.z = 0;

        CreatePermanentLine(startPos, endPos, leftEndpoint.color, leftEndpoint.wireName);
    }

    private void StopDragging()
    {
        currentDraggingEndpoint = null;
        tempLineRenderer.enabled = false;
    }

    private void CheckPuzzleCompletion()
    {
        if (!puzzleSolved && AllConnected())
        {
            CompletePuzzle();
        }
    }

    private void CompletePuzzle()
    {
        puzzleSolved = true;

        if (solvedText != null)
        {
            solvedText.gameObject.SetActive(true);
        }

        Debug.Log("Puzzle completed! 땅콩 Set Common 상태.");
        CutChangeManager.Instance.PlayCutCoroutine();
        MonsterManager.Instance.SetCommon(1);

        // Trigger the completion event
        OnPuzzleCompleted?.Invoke();

        // Schedule reset after delay
        StartCoroutine(ResetAfterDelay());
    }

    private IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(resetDelay);
        InitPuzzle();
    }

    GameObject CreateEndpoint(string endpointName, Vector3 pos, Color color)
    {
        GameObject ep = new GameObject(endpointName);
        ep.transform.position = pos; // Position is already set with Z=0

        // Sprite renderer
        SpriteRenderer sr = ep.AddComponent<SpriteRenderer>();
        sr.sprite = GenerateCircleSprite(32, color);
        sr.color = Color.white;

        // Add CircleCollider2D for 2D space
        CircleCollider2D col = ep.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = 0.5f;

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

    // Public method to manually reset the puzzle
    public void ResetPuzzle()
    {
        StopAllCoroutines();
        InitPuzzle();
    }
}
