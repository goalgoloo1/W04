using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RadioFrequencyPuzzle : MonoBehaviour
{
    //한성->민석 : ui를 생성하는 코드라서 유니티에서 수정하기보단 에디터로 코드를 수정하는 것이 좋을듯합니다.
    //한성: ui새로 만들어서 옮기기엔 시간이 부족하니 코드 수정으로 디자인 부탁드리겠습니다!~!!
    private float minFrequency = 88.0f;
    private float maxFrequency = 108.0f;
    private float targetFrequency;
    private float tolerance = 1.0f;

    private Slider frequencySlider;
    private Text frequencyText;
    private Text statusText;

    private float stayTime = 2.0f;           // 정답 주파수 유지 시간
    private float currentStayTime = 0f;      // 현재 유지 시간
    private bool puzzleSolved = false;       // 퍼즐 중복 해결 방지

    void Start()
    {
        CreateUI();

        targetFrequency = Random.Range(minFrequency, maxFrequency);
        Debug.Log("Target Frequency (hidden): " + targetFrequency);

        frequencySlider.minValue = minFrequency;
        frequencySlider.maxValue = maxFrequency;
        frequencySlider.value = (minFrequency + maxFrequency) / 2;
        frequencySlider.onValueChanged.AddListener(OnFrequencyChanged);

        frequencyText.text = "Frequency: " + frequencySlider.value.ToString("F1") + " MHz";
        statusText.text = "Adjust the radio frequency to find the signal.";
    }

    void Update()
    {
        if (puzzleSolved || frequencySlider == null) return;

        float currentFreq = frequencySlider.value;

        if (Mathf.Abs(currentFreq - targetFrequency) <= tolerance)
        {
            currentStayTime += Time.deltaTime;

            if (currentStayTime >= stayTime)
            {
                puzzleSolved = true;
                statusText.text = "Puzzle Solved!";
                Debug.Log("Puzzle Solved!");
            }
            else
            {
                float remain = stayTime - currentStayTime;
                statusText.text = $"Hold steady... ({remain:F1}s)";
            }
        }
        else
        {
            currentStayTime = 0f;
            statusText.text = "Adjust the radio frequency to find the signal.";
        }
    }

    void OnFrequencyChanged(float value)
    {
        frequencyText.text = "Frequency: " + value.ToString("F1") + " MHz";
        // 상태 메시지는 Update에서 처리
    }

    void CreateUI()
    {
        
        //한성: 디자인 수정을 위해 여기 코드를 참고하시면됩니다.
        Canvas canvas = FindObjectOfType<Canvas>();

        GameObject sliderObj = new GameObject("FrequencySlider");
        sliderObj.transform.SetParent(canvas.transform, false);
        frequencySlider = sliderObj.AddComponent<Slider>();

        // 회색 배경(슬라이더)
        GameObject sliderBackground = new GameObject("Background");
        sliderBackground.transform.SetParent(sliderObj.transform, false);
        Image bgImage = sliderBackground.AddComponent<Image>();
        bgImage.color = Color.black;
        RectTransform bgRect = sliderBackground.GetComponent<RectTransform>();
        bgRect.anchorMin = new Vector2(0, 0.25f);
        bgRect.anchorMax = new Vector2(1, 0.75f);
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;

        // 색을 채우는 과정
        GameObject fillArea = new GameObject("Fill Area");
        fillArea.transform.SetParent(sliderObj.transform, false);
        RectTransform fillAreaRect = fillArea.AddComponent<RectTransform>();
        fillAreaRect.anchorMin = new Vector2(0, 0);
        fillAreaRect.anchorMax = new Vector2(1, 1);
        fillAreaRect.offsetMin = new Vector2(10, 0);
        fillAreaRect.offsetMax = new Vector2(-10, 0);

        //녹색 슬라이더 
        GameObject fill = new GameObject("Fill");
        fill.transform.SetParent(fillArea.transform, false);
        Image fillImage = fill.AddComponent<Image>();
        fillImage.color = Color.grey;
        RectTransform fillRect = fill.GetComponent<RectTransform>();
        fillRect.anchorMin = new Vector2(0, 0);
        fillRect.anchorMax = new Vector2(1, 1);
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;
        frequencySlider.fillRect = fillRect;

        // 흰색 스크롤 하는 친구
        GameObject handleArea = new GameObject("Handle Slide Area");
        handleArea.transform.SetParent(sliderObj.transform, false);
        RectTransform handleAreaRect = handleArea.AddComponent<RectTransform>();
        handleAreaRect.anchorMin = new Vector2(0, 0);
        handleAreaRect.anchorMax = new Vector2(1, 1);
        handleAreaRect.offsetMin = Vector2.zero;
        handleAreaRect.offsetMax = Vector2.zero;

        //실제 잡고 움직이는 핸들
        GameObject handle = new GameObject("Handle");
        handle.transform.SetParent(handleArea.transform, false);
        Image handleImage = handle.AddComponent<Image>();
        handleImage.color = Color.white;
        RectTransform handleRect = handle.GetComponent<RectTransform>();
        handleRect.sizeDelta = new Vector2(20, 20);
        frequencySlider.targetGraphic = handleImage;
        frequencySlider.handleRect = handleRect;

        RectTransform sliderRect = sliderObj.GetComponent<RectTransform>();
        sliderRect.sizeDelta = new Vector2(1000, 80);
        sliderRect.anchoredPosition = new Vector2(0, 60);

        //검은색 글귀
        GameObject freqTextObj = new GameObject("FrequencyText");
        freqTextObj.transform.SetParent(canvas.transform, false);
        frequencyText = freqTextObj.AddComponent<Text>();
        frequencyText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        frequencyText.fontSize = 80;
        frequencyText.fontStyle = FontStyle.Bold;
        frequencyText.alignment = TextAnchor.MiddleCenter;
        RectTransform freqTextRect = frequencyText.GetComponent<RectTransform>();
        freqTextRect.sizeDelta = new Vector2(800, 100);
        freqTextRect.anchoredPosition = new Vector2(0, 200);
        frequencyText.color = Color.black;

        //파란색 글귀
        GameObject statusTextObj = new GameObject("StatusText");
        statusTextObj.transform.SetParent(canvas.transform, false);
        statusText = statusTextObj.AddComponent<Text>();
        statusText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        statusText.fontSize = 50;
        statusText.fontStyle = FontStyle.Bold;
        statusText.alignment = TextAnchor.MiddleCenter;
        RectTransform statusTextRect = statusText.GetComponent<RectTransform>();
        statusTextRect.sizeDelta = new Vector2(800, 100);
        statusTextRect.anchoredPosition = new Vector2(0, -55);
        statusText.color = Color.black;
    }
}