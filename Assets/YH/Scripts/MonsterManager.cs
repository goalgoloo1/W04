using System;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState
{
    Common,
    Anomalous,
    Critical,
    
}

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;

    public MonsterData[] monsterData;
    [SerializeField] private Monster[] monsters;
    [SerializeField]
    private bool isStop = false;
    private bool isSlenderTimeUpdate = false;

    
    [Header("예외 이상현상 들")]
    [SerializeField]
    private float slenderTime = 2;
    private float slenderTimer = 0;
    public Sprite boySprite;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetMonsterData();
        isStop = false;
    }


    private void Update()
    {
        if (isStop) return;
        MonsterTimeUpdate();
        if (CameraManager.Instance.currentCamera == 4 )
        {
            if (GetMonster(5) != null)
            {
                if (GetMonster(5).state == MonsterState.Anomalous)
                {
                    slenderTimer += Time.deltaTime;
                    if (slenderTimer >= slenderTime)
                    {
                        monsters[3].state = MonsterState.Critical;
                        slenderTimer = 0;
                    }
                }
            }
        }
    }


    private void MonsterTimeUpdate()
    {
        for (int i = 0; i < monsters.Length; i++)
        {
            monsters[i].time += Time.deltaTime;
            ToAnomalous(monsters[i]);
            ToCritical(monsters[i]);
        }
    }

    private void ToAnomalous(Monster monster)
    {
        if (monster.state == MonsterState.Common)
        {
            if (monster.time >= monster.timeToAnomalous)
            {
                monster.state = MonsterState.Anomalous;
                monster.time = 0;
            }
        }
    }

    private void ToCritical(Monster monster)
    {
        if(monster.isSlender) return;
        if (monster.state == MonsterState.Anomalous)
        {
            if (monster.time >= monster.timeToCritical)
            {
                monster.state = MonsterState.Critical;
                //Debug.Log("YouDie" + monster.name);
                //monster.time = 0;
                //isStop = true;
            }
        }
    }


    private void SetMonsterData()
    {
        monsters = new Monster[monsterData.Length];
        for (int i = 0; i < monsterData.Length; i++)
        {
            monsters[i] = new Monster();
            monsters[i].name = monsterData[i].name;
            monsters[i].num = monsterData[i].num;
            monsters[i].state = monsterData[i].state;
            monsters[i].time = 0;
            monsters[i].timeToAnomalous = monsterData[i].timeToAnomalous;
            monsters[i].timeToCritical = monsterData[i].timeToCritical;
            monsters[i].commonSprite = monsterData[i].commonSprite;
            monsters[i].anomalousSprite = monsterData[i].anomalousSprite;
            monsters[i].criticalSprite = monsterData[i].criticalSprite;
            monsters[i].isCheckAnomalous = monsterData[i].isCheckAnomalous;
            monsters[i].camNum = monsterData[i].camNum;
            monsters[i].isSlender = monsterData[i].isSlender;
            monsters[i].isGirl = monsterData[i].isGirl;
        }
    }

    public void SetCommon(int num) //몬스터의 번호를 입력 받습니다 1 ~ 7
    {
        Monster mon = GetMonster(num);
        if ((mon.state == MonsterState.Anomalous && mon.isCheckAnomalous) || (mon.num == 6 && mon.state == MonsterState.Anomalous))
        {
            mon.state = MonsterState.Common;
            mon.time = 0;
            mon.isCheckAnomalous = false;
        }

    }

    public Monster GetMonster(int num)
    {
        foreach (var data in monsters)
        {
            if (data.num == num)
                return data;
        }
        Debug.LogWarning("Monster not found"); 
        return null;
    }
    
    public Monster GetMonsterByCamNum(int num)
    {
        foreach (var data in monsters)
        {
            //Debug.Log(data.camNum + " "+ num);
            if (data.camNum == num)
                return data;
        }
        Debug.LogWarning("Monster not found"); 
        return null;
    }
    
    
    
}

[Serializable]
public class Monster
{
    public string name;
    public int num;
    public MonsterState state;
    public float time;
    public float timeToAnomalous;
    public float timeToCritical;
    public Sprite commonSprite;
    public Sprite anomalousSprite;
    public Sprite criticalSprite;
    //이상현상 관측 을 구분하는 bool
    public bool isCheckAnomalous;
    //cam 넘버
    public int camNum;
    //예외처리
    public bool isSlender;
    public bool isGirl;
}
