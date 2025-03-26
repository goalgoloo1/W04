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
        
        if(mon.state == MonsterState.Common)
            cams[camNum - 1].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = mon.commonSprite;
        if (mon.state == MonsterState.Anomalous)
        {
            cams[camNum - 1].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = mon.anomalousSprite;
            mon.isCheckAnomalous = true;
        }
    }
}
