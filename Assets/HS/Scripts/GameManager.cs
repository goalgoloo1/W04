using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject nutsJumpscareObject; // Reference to the 2D image object for Nuts jumpscare

    void Update()
    {
        if (MonsterManager.Instance.GetMonster(1).state == MonsterState.Critical) { NutsJumpscare(); }
        if (MonsterManager.Instance.GetMonster(3).state == MonsterState.Critical) { MaskJumpscare(); }
        if (MonsterManager.Instance.GetMonster(4).state == MonsterState.Critical) { AngelJumpscare(); }
        if (MonsterManager.Instance.GetMonster(5).state == MonsterState.Critical) { SlendermanJumpscare(); }
        if (MonsterManager.Instance.GetMonster(7).state == MonsterState.Critical) { BallerinaJumpscare(); }
    }
    void GameOver()
    {
        Debug.Log("메인화면 씬 로드");
    }
    void NutsJumpscare()
    {
        Debug.Log("Nuts한테 쥬금");
        nutsJumpscareObject.SetActive(true); 
        StartCoroutine(DelayedGameOver());
    }
    void MaskJumpscare()
    {
        Debug.Log("Mask한테 쥬금");
        StartCoroutine(DelayedGameOver());
    }
    void AngelJumpscare()
    {
        Debug.Log("Angel한테 쥬금");
        StartCoroutine(DelayedGameOver());
    }
    void SlendermanJumpscare()
    {
        Debug.Log("Slenderman한테 쥬금");
        StartCoroutine(DelayedGameOver());
    }
    void BallerinaJumpscare()
    {
        Debug.Log("Ballerina한테 쥬금");
        StartCoroutine(DelayedGameOver());
    }
    IEnumerator DelayedGameOver()
    {
        yield return new WaitForSeconds(2f);
        GameOver();
    }
}
