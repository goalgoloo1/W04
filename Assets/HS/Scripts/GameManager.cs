using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using static CameraManager;

public class GameManager : MonoBehaviour
{
    [Header("Jumpscare UI Elements")]
    [SerializeField] private Canvas jumpscareCanvas;
    [SerializeField] private Image nutsJumpscareImage;
    [SerializeField] private Image maskJumpscareImage;
    [SerializeField] private Image angelJumpscareImage;
    [SerializeField] private Image slendermanJumpscareImage;
    [SerializeField] private Image ballerinaJumpscareImage;

    //private bool isJumpscareActive = false;

    void Start()
    {
        DeactivateAllJumpscares();
    }

    void Update()
    {
        //if (isJumpscareActive)
            //return;
        if (MonsterManager.Instance.GetMonster(1).state == MonsterState.Critical) { NutsJumpscare(); }
        if (MonsterManager.Instance.GetMonster(3).state == MonsterState.Critical) { MaskJumpscare(); }
        if (MonsterManager.Instance.GetMonster(4).state == MonsterState.Critical) { AngelJumpscare(); }
        if (MonsterManager.Instance.GetMonster(5).state == MonsterState.Critical) { SlendermanJumpscare(); }
        if (MonsterManager.Instance.GetMonster(7).state == MonsterState.Critical) { BallerinaJumpscare(); }
    }

    void GameOver()
    {
        Debug.Log("StartMenu 씬 로드");
    }

    void DeactivateAllJumpscares()
    {
        // 모든 점프스케어 이미지 비활성화
        if (jumpscareCanvas != null)
            jumpscareCanvas.enabled = false;

        if (nutsJumpscareImage != null)
            nutsJumpscareImage.enabled = false;

        if (maskJumpscareImage != null)
            maskJumpscareImage.enabled = false;

        if (angelJumpscareImage != null)
            angelJumpscareImage.enabled = false;

        if (slendermanJumpscareImage != null)
            slendermanJumpscareImage.enabled = false;

        if (ballerinaJumpscareImage != null)
            ballerinaJumpscareImage.enabled = false;
    }

    void NutsJumpscare()
    {
        Debug.Log("Nuts한테 쥬금");
        ShowJumpscareOverlay(nutsJumpscareImage);
    }

    void MaskJumpscare()
    {
        Debug.Log("Mask한테 쥬금");
        ShowJumpscareOverlay(maskJumpscareImage);
    }

    void AngelJumpscare()
    {
        Debug.Log("Angel한테 쥬금");
        ShowJumpscareOverlay(angelJumpscareImage);
    }

    void SlendermanJumpscare()
    {
        Debug.Log("Slenderman한테 쥬금");
        ShowJumpscareOverlay(slendermanJumpscareImage);
    }

    void BallerinaJumpscare()
    {
        Debug.Log("Ballerina한테 쥬금");
        ShowJumpscareOverlay(ballerinaJumpscareImage);
    }

    void ShowJumpscareOverlay(Image jumpscareImage)
    {
        //if (isJumpscareActive || jumpscareImage == null || jumpscareCanvas == null)
        //    return;

        //isJumpscareActive = true;
        CameraManager cameraManager = FindObjectOfType<CameraManager>();
        cameraManager.SwitchToCamera(CameraMonitor.Office);
        jumpscareCanvas.enabled = true;
        jumpscareImage.enabled = true;
        StartCoroutine(DelayedGameOver());
    }

    IEnumerator DelayedGameOver()
    {
        yield return new WaitForSeconds(2f);
        DeactivateAllJumpscares();
        GameOver();
        //isJumpscareActive = false;
    }
}
