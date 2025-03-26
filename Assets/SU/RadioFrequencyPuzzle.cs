using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RadioFrequencyPuzzle : MonoBehaviour
{
    // FM 라디오 주파수 범위 (예: 88.0 MHz ~ 108.0 MHz)
    private float minFrequency = 88.0f;
    private float maxFrequency = 108.0f;
    private float targetFrequency; // 정답 주파수 (랜덤 결정)
    private float tolerance = 1.0f; // ±1.0 MHz 오차 허용

    private Slider frequencySlider;
    private Text frequencyText;
    private Text statusText;

    void Start()
    {
        CreateUI();

        // 정답 주파수는 플레이 시 매번 랜덤으로 결정 (플레이어에게는 숨김)
        targetFrequency = Random.Range(minFrequency, maxFrequency);
        Debug.Log("Target Frequency (hidden): " + targetFrequency);

        // 슬라이더 초기 설정
        frequencySlider.minValue = minFrequency;
        frequencySlider.maxValue = maxFrequency;
        frequencySlider.value = (minFrequency + maxFrequency) / 2;
        frequencySlider.onValueChanged.AddListener(OnFrequencyChanged);

        // 초기 UI 업데이트
        frequencyText.text = "Frequency: " + frequencySlider.value.ToString("F1") + " MHz";
        statusText.text = "Adjust the radio frequency to find the signal.";
    }

    /// <summary>
    /// 슬라이더 값 변경 시 호출됨.
    /// 현재 주파수를 텍스트로 표시하고, 목표 주파수와 비교하여 퍼즐 해결 여부를 업데이트합니다.
    /// </summary>
    /// <param name="value">현재 슬라이더 값 (주파수)</param>
    void OnFrequencyChanged(float value)
    {
        frequencyText.text = "Frequency: " + value.ToString("F1") + " MHz";

        if (Mathf.Abs(value - targetFrequency) <= tolerance)
        {
            statusText.text = "Puzzle Solved!";
            Debug.Log("Puzzle Solved! Frequency matched: " + value);
        }
        else
        {
            statusText.text = "Adjust the radio frequency to find the signal.";
        }
    }

    /// <summary>
    /// UI 캔버스, 슬라이더, 그리고 텍스트들을 자동으로 생성합니다.
    /// Inspector에서 별도 할당할 필요 없이 코드만 붙이면 바로 동작합니다.
    /// </summary>
    void CreateUI()
    {
        // EventSystem이 없는 경우 자동 생성 (UI 입력 처리 필수)
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystemObj = new GameObject("EventSystem");
            eventSystemObj.AddComponent<EventSystem>();
            eventSystemObj.AddComponent<StandaloneInputModule>();
        }

        // Canvas가 있는지 확인, 없으면 생성
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        // 슬라이더 생성
        GameObject sliderObj = new GameObject("FrequencySlider");
        sliderObj.transform.SetParent(canvas.transform, false);
        frequencySlider = sliderObj.AddComponent<Slider>();

        // 슬라이더 배경 설정 (간단한 이미지 추가)
        GameObject sliderBackground = new GameObject("Background");
        sliderBackground.transform.SetParent(sliderObj.transform, false);
        Image bgImage = sliderBackground.AddComponent<Image>();
        bgImage.color = Color.gray;
        RectTransform bgRect = sliderBackground.GetComponent<RectTransform>();
        bgRect.anchorMin = new Vector2(0, 0.25f);
        bgRect.anchorMax = new Vector2(1, 0.75f);
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;

        // Fill Area 생성
        GameObject fillArea = new GameObject("Fill Area");
        fillArea.transform.SetParent(sliderObj.transform, false);
        RectTransform fillAreaRect = fillArea.AddComponent<RectTransform>();
        fillAreaRect.anchorMin = new Vector2(0, 0);
        fillAreaRect.anchorMax = new Vector2(1, 1);
        fillAreaRect.offsetMin = new Vector2(10, 0);
        fillAreaRect.offsetMax = new Vector2(-10, 0);

        // Fill 생성
        GameObject fill = new GameObject("Fill");
        fill.transform.SetParent(fillArea.transform, false);
        Image fillImage = fill.AddComponent<Image>();
        fillImage.color = Color.green;
        RectTransform fillRect = fill.GetComponent<RectTransform>();
        fillRect.anchorMin = new Vector2(0, 0);
        fillRect.anchorMax = new Vector2(1, 1);
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;
        frequencySlider.fillRect = fillRect;

        // Handle Slide Area 생성
        GameObject handleArea = new GameObject("Handle Slide Area");
        handleArea.transform.SetParent(sliderObj.transform, false);
        RectTransform handleAreaRect = handleArea.AddComponent<RectTransform>();
        handleAreaRect.anchorMin = new Vector2(0, 0);
        handleAreaRect.anchorMax = new Vector2(1, 1);
        handleAreaRect.offsetMin = Vector2.zero;
        handleAreaRect.offsetMax = Vector2.zero;

        // Handle 생성
        GameObject handle = new GameObject("Handle");
        handle.transform.SetParent(handleArea.transform, false);
        Image handleImage = handle.AddComponent<Image>();
        handleImage.color = Color.white;
        RectTransform handleRect = handle.GetComponent<RectTransform>();
        handleRect.sizeDelta = new Vector2(20, 20);
        frequencySlider.targetGraphic = handleImage;
        frequencySlider.handleRect = handleRect;

        // 슬라이더 RectTransform 설정
        RectTransform sliderRect = sliderObj.GetComponent<RectTransform>();
        sliderRect.sizeDelta = new Vector2(300, 20);
        sliderRect.anchoredPosition = new Vector2(0, -50);

        // 주파수를 표시할 텍스트 생성
        GameObject freqTextObj = new GameObject("FrequencyText");
        freqTextObj.transform.SetParent(canvas.transform, false);
        frequencyText = freqTextObj.AddComponent<Text>();
        frequencyText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        frequencyText.fontSize = 24;
        frequencyText.alignment = TextAnchor.MiddleCenter;
        RectTransform freqTextRect = frequencyText.GetComponent<RectTransform>();
        freqTextRect.sizeDelta = new Vector2(400, 50);
        freqTextRect.anchoredPosition = new Vector2(0, 0);
        frequencyText.color = Color.black;

        // 상태 메시지를 표시할 텍스트 생성
        GameObject statusTextObj = new GameObject("StatusText");
        statusTextObj.transform.SetParent(canvas.transform, false);
        statusText = statusTextObj.AddComponent<Text>();
        statusText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        statusText.fontSize = 24;
        statusText.alignment = TextAnchor.MiddleCenter;
        RectTransform statusTextRect = statusText.GetComponent<RectTransform>();
        statusTextRect.sizeDelta = new Vector2(400, 50);
        statusTextRect.anchoredPosition = new Vector2(0, -100);
        statusText.color = Color.blue;
    }
}
