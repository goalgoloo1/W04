using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    float m_seconds = 46f;
    int m_hours = 0;
    //bool m_isTimePaused = false;

    [SerializeField] private TextMeshProUGUI timeDisplay;

    void Update()
    {
        //CheckMonsterState();
        PrintHours();
        StageClear();
        UpdateTimer();
        UpdateTimeDisplay();
    }
    void UpdateTimeDisplay()
    {
        timeDisplay.text = m_hours.ToString("00" + "AM");
    }
    void UpdateTimer()
    {
        //if (!m_isTimePaused)
        //{
        //    m_seconds += Time.deltaTime;
        //}
        m_seconds += Time.deltaTime;

    }
    void PrintHours()
    {
        const int secondsPerHour = 50;
        m_hours = (int)m_seconds / secondsPerHour;
        //Debug.Log(m_seconds);
        //Debug.Log(m_hours);
    }
    void StageClear()
    {
        if (m_hours == 6)
        {
            SceneManager.LoadScene("EndingScene");
            Debug.Log("Stage Clear! ,EndingScene 로드");
        }
    }
}

