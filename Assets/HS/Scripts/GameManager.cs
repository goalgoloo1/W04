using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using static CameraManager;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Jumpscare UI Elements")]
    [SerializeField] private Canvas jumpscareCanvas;
    [SerializeField] private Image nutsJumpscareImage;
    [SerializeField] private Image maskJumpscareImage;
    [SerializeField] private Image angelJumpscareImage;
    [SerializeField] private Image slendermanJumpscareImage;
    [SerializeField] private Image ballerinaJumpscareImage;

    private bool nutsAnomalyAudioPlayed = false;
    private bool maskAnomalyAudioPlayed = false;
    private bool angelAnomalyAudioPlayed = false;
    private bool ballerinaAnomalyAudioPlayed = false;


    void Start()
    {
        DeactivateAllJumpscares();
    }

    void Update()
    {
        
        CheckAndResetAudioFlags();
        
        if (MonsterManager.Instance.GetMonster(1).state == MonsterState.Anomalous) { NutsAnomalousAudio(); }
        if (MonsterManager.Instance.GetMonster(3).state == MonsterState.Anomalous) { MaskAnomalousAudio(); }
        if (MonsterManager.Instance.GetMonster(4).state == MonsterState.Anomalous) { AngelAnomalousAudio(); }
        //if (MonsterManager.Instance.GetMonster(5).state == MonsterState.Anomalous) { SlendermanAnomalousAudio(); }
        if (MonsterManager.Instance.GetMonster(7).state == MonsterState.Anomalous) { BallerinaAnomalousAudio(); }

        if (MonsterManager.Instance.GetMonster(1).state == MonsterState.Critical) { NutsJumpscare(); }
        if (MonsterManager.Instance.GetMonster(3).state == MonsterState.Critical) { MaskJumpscare(); }
        if (MonsterManager.Instance.GetMonster(4).state == MonsterState.Critical) { AngelJumpscare(); }
        if (MonsterManager.Instance.GetMonster(5).state == MonsterState.Critical) { SlendermanJumpscare(); }
        if (MonsterManager.Instance.GetMonster(7).state == MonsterState.Critical) { BallerinaJumpscare(); }
    }

    // Check for state changes to reset audio flags
    private void CheckAndResetAudioFlags()
    {
        if (MonsterManager.Instance.GetMonster(1).state != MonsterState.Anomalous)
            nutsAnomalyAudioPlayed = false;
            
        if (MonsterManager.Instance.GetMonster(3).state != MonsterState.Anomalous)
            maskAnomalyAudioPlayed = false;
            
        if (MonsterManager.Instance.GetMonster(4).state != MonsterState.Anomalous)
            angelAnomalyAudioPlayed = false;
            
        //if (MonsterManager.Instance.GetMonster(5).state != MonsterState.Anomalous)
        //    slendermanAnomalyAudioPlayed = false;
            
        if (MonsterManager.Instance.GetMonster(7).state != MonsterState.Anomalous)
            ballerinaAnomalyAudioPlayed = false;
    }

    void GameOver()
    {
        SceneManager.LoadScene("StartScene");
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

    void NutsAnomalousAudio()
    {
        if (!nutsAnomalyAudioPlayed)
        {
            Debug.Log("Nuts 아노말리");
            SoundManager.Instance.nutsAnomalyAudio.Play();
            nutsAnomalyAudioPlayed = true;
        }
    }

    void MaskAnomalousAudio()
    {
        if (!maskAnomalyAudioPlayed)
        {
            Debug.Log("Mask 아노말리");
            SoundManager.Instance.maskAnomalyAudio.Play();
            maskAnomalyAudioPlayed = true;
        }
    }

    void AngelAnomalousAudio()
    {
        if (!angelAnomalyAudioPlayed)
        {
            Debug.Log("Angel 아노말리");
            SoundManager.Instance.angelAnomalyAudio.Play();
            angelAnomalyAudioPlayed = true;
        }
    }

    //void SlendermanAnomalousAudio()
    //{
    //    if (!slendermanAnomalyAudioPlayed)
    //    {
    //        Debug.Log("Slenderman 아노말리");
    //        SoundManager.Instance.slendermanAnomalyAudio.Play();
    //        slendermanAnomalyAudioPlayed = true;
    //    }
    //}

    void BallerinaAnomalousAudio()
    {
        if (!ballerinaAnomalyAudioPlayed)
        {
            Debug.Log("Ballerina 아노말리");
            SoundManager.Instance.ballerinaAnomalyAudio.Play();
            ballerinaAnomalyAudioPlayed = true;
        }
    }

    void NutsJumpscare()
    {
        Debug.Log("Nuts한테 쥬금");
        SoundManager.Instance.nutsJumpscareAudio.Play();
        ShowJumpscareOverlay(nutsJumpscareImage);
    }

    void MaskJumpscare()
    {
        Debug.Log("Mask한테 쥬금");
        SoundManager.Instance.maskJumpscareAudio.Play();
        ShowJumpscareOverlay(maskJumpscareImage);
    }

    void AngelJumpscare()
    {
        Debug.Log("Angel한테 쥬금");
        SoundManager.Instance.slendermanJumpscareAudio.Play();
        ShowJumpscareOverlay(angelJumpscareImage);
    }

    void SlendermanJumpscare()
    {
        Debug.Log("Slenderman한테 쥬금");
        SoundManager.Instance.slendermanJumpscareAudio.Play();
        ShowJumpscareOverlay(slendermanJumpscareImage);
    }

    void BallerinaJumpscare()
    {
        Debug.Log("Ballerina한테 쥬금");
        SoundManager.Instance.ballerinaJumpscareAudio.Play();
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
