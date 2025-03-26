using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("Timer Settings")]
    private float m_seconds = 95f;
    private int m_hours = 0;
    private bool m_isTimePaused = false;
    private int secondsPerHour = 10;  // Adjustable seconds per hour

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI timeText;  // Reference to the TextMeshPro UI element

    // Hour name strings
    private readonly string[] hourNames = { "12 AM", "1 AM", "2 AM", "3 AM", "4 AM", "5 AM", "6 AM" };

    void Start()
    {
        // Initial time setup
        UpdateTimeDisplay();
    }

    void Update()
    {
        CheckMonsterState();
        UpdateHours();
        StageClear();
        UpdateTimer();
        UpdateTimeDisplay();
    }

    void UpdateTimer()
    {
        if (!m_isTimePaused)
        {
            m_seconds += Time.deltaTime;
        }
    }

    void UpdateHours()
    {
        int previousHour = m_hours;
        m_hours = (int)m_seconds / secondsPerHour;

        // Log only when hour changes to reduce console spam
        if (previousHour != m_hours)
        {
            Debug.Log($"Seconds: {m_seconds:F2}, Hour: {m_hours}");
        }
    }

    void UpdateTimeDisplay()
    {
        if (timeText != null)
        {
            // Clamp hours to valid range (0-6)
            int displayHour = Mathf.Clamp(m_hours, 0, 6);

            // Set the text to the appropriate hour name
            timeText.text = hourNames[displayHour];

            // Optional: Animate the text when hour changes
            if (m_hours > 0 && m_seconds % secondsPerHour < 1.0f)
            {
                StartCoroutine(PulseText());
            }
        }
    }

    System.Collections.IEnumerator PulseText()
    {
        // Simple pulse animation
        Vector3 originalScale = timeText.transform.localScale;
        Vector3 targetScale = originalScale * 1.2f;

        // Scale up
        float duration = 0.2f;
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            timeText.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / duration);
            yield return null;
        }

        // Scale down
        elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            timeText.transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / duration);
            yield return null;
        }

        timeText.transform.localScale = originalScale;
    }

    void StageClear()
    {
        if (m_hours == 6)
        {
            m_isTimePaused = true;
            Debug.Log("Stage Clear! , Clear.Scene 로드");

            // You can add scene loading code here:
            // SceneManager.LoadScene("Clear");
        }
    }

    void CheckMonsterState()
    {
        for (int i = 1; i < 8; i++)
        {
            if (i == 2) continue; // Skip monster 2, 자판기 괴물 피쳐 삭제.
            if (MonsterManager.Instance.GetMonster(i).state == MonsterState.Anomalous)
            {
                m_isTimePaused = true;
                return;
            }
        }
        m_isTimePaused = false;
    }

    // Public method to get the current hour (for other scripts)
    public int GetCurrentHour()
    {
        return m_hours;
    }

    // Public method to check if it's currently 6AM
    public bool IsStageClear()
    {
        return m_hours >= 6;
    }
}

