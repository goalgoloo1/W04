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
    [SerializeField]
    private Monster[] monsters; 
    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        SetMonsterData();
    }
    
    
    private void Update()
    {
        MonsterTimeUpdate();
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
        if (monster.state == MonsterState.Anomalous)
        {
            if (monster.time >= monster.timeToCritical)
            {
                monster.state = MonsterState.Critical;
                monster.time = 0;
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
        }
    }
    
    public void SetCommon(int num) //몬스터의 번호를 입력 받습니다 1 ~ 7
    {
        num--;
        if (monsters[num].state == MonsterState.Anomalous && monsters[num].isCheckAnomalous)
        {
            monsters[num].state = MonsterState.Common;
            monsters[num].time = 0;
            monsters[num].isCheckAnomalous = false;
        }
        
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
}
