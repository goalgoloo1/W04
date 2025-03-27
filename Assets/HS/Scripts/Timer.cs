using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float m_seconds = 96f;
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
        const int secondsPerHour = 100;
        m_hours = (int)m_seconds / secondsPerHour;
        //Debug.Log(m_seconds);
        //Debug.Log(m_hours);
    }
    void StageClear()
    {
        if (m_hours == 6)
        {
            Debug.Log("Stage Clear! , Clear.Scene 로드");
        }
    }

    //To do: 메인 씬에 넣고 아노말리 일 때 시계 멈추는지 확인

    //void CheckMonsterState()
    //{
    //    //Debug.Log("아노말리 때 시간 멈추는 지 확인 " + m_timePaused);

    //    for (int i = 1; i < 8; i++)
    //    {
    //        //Debug.Log(MonsterManager.Instance.GetMonster(i).state);
    //        if (i == 2) continue; // Skip monster 2, 자판기 괴물 피쳐 삭제.
    //        if (MonsterManager.Instance.GetMonster(i).state == MonsterState.Anomalous)
    //        {
    //            m_isTimePaused = true;
    //            return;
    //        }
    //    }
    //    m_isTimePaused = false;
    //}
}

