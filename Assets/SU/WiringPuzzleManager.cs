using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WiringPuzzleManager : MonoBehaviour
{
    [Header("엔드포인트 및 라인 기본 설정")]
    [SerializeField] private float leftX = -4f;
    [SerializeField] private float rightX = 4f;
    [SerializeField] private float spacingY = 2f;
    [SerializeField] private float lineWidth = 0.1f;

    // Inspector에서 비워 둬도 됨. (자동 생성)
    [SerializeField] private Material lineMaterial;
    [SerializeField] private Text solvedText;

    // 내부 클래스: 엔드포인트 정보
    private class Endpoint
    {
        public GameObject obj;
        public string wireName;  // 전선 식별자 (예: "red", "blue" 등)
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

    // 연결 정보 (필요시 참고용)
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
        // 1) 라인 재질이 없다면 "Sprites/Default"로 기본 할당
        if (lineMaterial == null)
        {
            lineMaterial = new Material(Shader.Find("Sprites/Default"));
        }

        // 2) "퍼즐 해결" 텍스트가 없으면 자동으로 생성
        if (solvedText == null)
        {
            CreateSolvedText();
        }
        solvedText.gameObject.SetActive(false); // 시작 시 비활성화

        // 3) 전선 색상 및 식별자 설정 (필요에 맞게 추가/삭제 가능)
        List<(string, Color)> wireColors = new List<(string, Color)>
        {
            ("red",    Color.red),
            ("blue",   Color.blue),
            ("green",  Color.green),
            ("yellow", Color.yellow),
        };

        int numWires = wireColors.Count;

        // 4) 왼쪽 엔드포인트 생성
        for (int i = 0; i < numWires; i++)
        {
            float y = (i - (numWires - 1) / 2f) * spacingY;
            Vector3 pos = new Vector3(leftX, y, 0);
            GameObject epObj = CreateEndpoint($"Left_{wireColors[i].Item1}", pos, wireColors[i].Item2);
            leftEndpoints.Add(new Endpoint(epObj, wireColors[i].Item1, wireColors[i].Item2));
        }

        // 5) 오른쪽 엔드포인트 생성 (색상 순서 무작위)
        List<(string, Color)> shuffled = new List<(string, Color)>(wireColors);
        ShuffleList(shuffled);

        for (int i = 0; i < numWires; i++)
        {
            float y = (i - (numWires - 1) / 2f) * spacingY;
            Vector3 pos = new Vector3(rightX, y, 0);
            GameObject epObj = CreateEndpoint($"Right_{shuffled[i].Item1}", pos, shuffled[i].Item2);
            rightEndpoints.Add(new Endpoint(epObj, shuffled[i].Item1, shuffled[i].Item2));
        }

        // 6) 드래그 중 표시할 임시 라인
        GameObject tempLineObj = new GameObject("TempLine");
        tempLineRenderer = tempLineObj.AddComponent<LineRenderer>();
        tempLineRenderer.material = lineMaterial;
        tempLineRenderer.startWidth = lineWidth;
        tempLineRenderer.endWidth = lineWidth;
        tempLineRenderer.positionCount = 2;
        tempLineRenderer.enabled = false;
    }

    void Update()
    {
        // 마우스 클릭 시작: 왼쪽 엔드포인트 선택
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0;

            foreach (var endpoint in leftEndpoints)
            {
                if (!endpoint.connected)
                {
                    float dist = Vector3.Distance(worldPos, endpoint.obj.transform.position);
                    if (dist < 0.5f)
                    {
                        currentDraggingEndpoint = endpoint;
                        tempLineRenderer.enabled = true;
                        tempLineRenderer.SetPosition(0, endpoint.obj.transform.position);
                        tempLineRenderer.SetPosition(1, worldPos);
                        break;
                    }
                }
            }
        }

        // 드래그 중: 임시 선 갱신
        if (Input.GetMouseButton(0) && currentDraggingEndpoint != null)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0;
            tempLineRenderer.SetPosition(1, worldPos);
        }

        // 마우스 버튼 뗄 때: 오른쪽 엔드포인트 연결 시도
        if (Input.GetMouseButtonUp(0) && currentDraggingEndpoint != null)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0;

            foreach (var endpoint in rightEndpoints)
            {
                if (!endpoint.connected)
                {
                    float dist = Vector3.Distance(worldPos, endpoint.obj.transform.position);
                    if (dist < 0.5f)
                    {
                        // 전선 식별자(예: "red")가 일치하면 연결
                        if (endpoint.wireName == currentDraggingEndpoint.wireName)
                        {
                            endpoint.connected = true;
                            currentDraggingEndpoint.connected = true;
                            CreatePermanentLine(currentDraggingEndpoint.obj.transform.position,
                                                endpoint.obj.transform.position,
                                                currentDraggingEndpoint.color,
                                                currentDraggingEndpoint.wireName);
                        }
                        break;
                    }
                }
            }

            currentDraggingEndpoint = null;
            tempLineRenderer.enabled = false;

            // 모든 연결 완료 시 UI 메시지 표시
            if (AllConnected())
            {
                solvedText.gameObject.SetActive(true);
            }
        }
    }

    // ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    // 여기서부터는 유틸리티 / 부가 메서드

    /// <summary>
    /// 엔드포인트(원형) 오브젝트를 동적으로 생성하여 반환
    /// </summary>
    GameObject CreateEndpoint(string endpointName, Vector3 pos, Color color)
    {
        GameObject ep = new GameObject(endpointName);
        ep.transform.position = pos;

        // 스프라이트 렌더러
        SpriteRenderer sr = ep.AddComponent<SpriteRenderer>();
        sr.sprite = GenerateCircleSprite(32, color);  // 32x32 짜리 원형 텍스처 생성
        sr.color = Color.white;                       // sprite 자체가 이미 색이 입혀져 있음

        // CircleCollider2D 추가 (클릭 판정)
        CircleCollider2D col = ep.AddComponent<CircleCollider2D>();
        col.isTrigger = true;  // 충돌 대신 클릭 판정만 필요하면 isTrigger 사용
        col.radius = 0.5f;

        return ep;
    }

    /// <summary>
    /// 동적으로 원형 텍스처를 만든 뒤, 해당 텍스처를 사용하는 Sprite 생성
    /// </summary>
    Sprite GenerateCircleSprite(int size, Color color)
    {
        Texture2D tex = new Texture2D(size, size, TextureFormat.ARGB32, false);
        tex.filterMode = FilterMode.Point;

        float r = size / 2f;
        Vector2 center = new Vector2(r, r);

        // 원 내부 픽셀에만 color, 나머지는 투명
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

        // pivot = (0.5, 0.5)로 중앙 정렬된 스프라이트
        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
    }

    /// <summary>
    /// 두 엔드포인트를 연결하는 영구 라인 생성
    /// </summary>
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

    /// <summary>
    /// 엔드포인트 연결 완료 여부 확인
    /// </summary>
    bool AllConnected()
    {
        foreach (var ep in leftEndpoints)
            if (!ep.connected) return false;

        foreach (var ep in rightEndpoints)
            if (!ep.connected) return false;

        return true;
    }

    /// <summary>
    /// 리스트 무작위 셔플
    /// </summary>
    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randIndex];
            list[randIndex] = temp;
        }
    }

    /// <summary>
    /// 퍼즐 완료 텍스트가 없으면, 자동으로 Canvas + Text를 생성
    /// </summary>
    void CreateSolvedText()
    {
        // 캔버스가 없다면 새로 생성
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        // Text 생성
        GameObject textObj = new GameObject("SolvedText");
        textObj.transform.SetParent(canvas.transform, false);

        solvedText = textObj.AddComponent<Text>();
        solvedText.text = "퍼즐 해결!";
        solvedText.fontSize = 36;
        solvedText.color = Color.white;
        solvedText.alignment = TextAnchor.MiddleCenter;

        // RectTransform 설정 (상단 중앙)
        RectTransform rect = solvedText.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 1);
        rect.anchorMax = new Vector2(0.5f, 1);
        rect.pivot = new Vector2(0.5f, 1);
        rect.anchoredPosition = new Vector2(0, -50);
        rect.sizeDelta = new Vector2(400, 100);
    }
}
