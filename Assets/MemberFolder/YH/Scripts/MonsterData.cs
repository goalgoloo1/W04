using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Create MonsterData")]
public class MonsterData : ScriptableObject
{
    public string monsterName;
    public int num;
    public MonsterState state;
    public float timeToAnomalous;
    public float timeToCritical;
    //상태별 png
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
