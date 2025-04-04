using System;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;
    public GameObject[] cams;
    public GameObject[] puzzles;

    private void Awake()
    {
        Instance = this;
    }

    public void SetCamImage(int camNum)
    {
        Monster mon = MonsterManager.Instance.GetMonsterByCamNum(camNum);

        if (MonsterManager.Instance.GetMonster(6).state == MonsterState.Anomalous) // 6번의 예외 처리
        {
            mon.isCheckAnomalous = true;
            cams[camNum - 1].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite =
                MonsterManager.Instance.GetMonster(6).anomalousSprite;
            return;
        }
        
        if(mon.state == MonsterState.Common)
            cams[camNum - 1].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = mon.commonSprite;
        if (mon.state == MonsterState.Anomalous)
        {
            cams[camNum - 1].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = mon.anomalousSprite;
            mon.isCheckAnomalous = true;
        }
        if (mon.state == MonsterState.Critical)
            cams[camNum - 1].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = mon.criticalSprite;
    }
}
