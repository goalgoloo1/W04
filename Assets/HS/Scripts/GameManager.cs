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

    // Flags to track audio playback
    private bool nutsAnomalyAudioPlayed = false;
    private bool maskAnomalyAudioPlayed = false;
    private bool angelAnomalyAudioPlayed = false;
    private bool ballerinaAnomalyAudioPlayed = false;

    // References to active coroutines for stopping audio
    private Coroutine nutsAnomalyCoroutine = null;
    private Coroutine maskAnomalyCoroutine = null;
    private Coroutine angelAnomalyCoroutine = null;
    private Coroutine ballerinaAnomalyCoroutine = null;

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
        // If monster state changes from Anomalous to something else, stop sound and reset flag
        if (MonsterManager.Instance.GetMonster(1).state != MonsterState.Anomalous)
        {
            StopAnomalyAudio(ref nutsAnomalyCoroutine, SoundManager.Instance.nutsAnomalyAudio);
            nutsAnomalyAudioPlayed = false;
        }
            
        if (MonsterManager.Instance.GetMonster(3).state != MonsterState.Anomalous)
        {
            StopAnomalyAudio(ref maskAnomalyCoroutine, SoundManager.Instance.maskAnomalyAudio);
            maskAnomalyAudioPlayed = false;
        }
            
        if (MonsterManager.Instance.GetMonster(4).state != MonsterState.Anomalous)
        {
            StopAnomalyAudio(ref angelAnomalyCoroutine, SoundManager.Instance.angelAnomalyAudio);
            angelAnomalyAudioPlayed = false;
        }
            
        if (MonsterManager.Instance.GetMonster(7).state != MonsterState.Anomalous)
        {
            StopAnomalyAudio(ref ballerinaAnomalyCoroutine, SoundManager.Instance.ballerinaAnomalyAudio);
            ballerinaAnomalyAudioPlayed = false;
        }
    }

    // Helper method to stop an audio source and clear its coroutine
    private void StopAnomalyAudio(ref Coroutine coroutine, AudioSource audioSource)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
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
        if (!nutsAnomalyAudioPlayed && SoundManager.Instance != null)
        {
            Debug.Log("Nuts 아노말리");
            // Stop previous coroutine if it exists
            StopAnomalyAudio(ref nutsAnomalyCoroutine, SoundManager.Instance.nutsAnomalyAudio);
            // Start new coroutine to play for exactly 2 seconds
            nutsAnomalyCoroutine = StartCoroutine(PlayAudioFor2Seconds(SoundManager.Instance.nutsAnomalyAudio));
            nutsAnomalyAudioPlayed = true;
        }
    }

    void MaskAnomalousAudio()
    {
        if (!maskAnomalyAudioPlayed && SoundManager.Instance != null)
        {
            Debug.Log("Mask 아노말리");
            // Stop previous coroutine if it exists
            StopAnomalyAudio(ref maskAnomalyCoroutine, SoundManager.Instance.maskAnomalyAudio);
            // Start new coroutine to play for exactly 2 seconds
            maskAnomalyCoroutine = StartCoroutine(PlayAudioFor2Seconds(SoundManager.Instance.maskAnomalyAudio));
            maskAnomalyAudioPlayed = true;
        }
    }

    void AngelAnomalousAudio()
    {
        if (!angelAnomalyAudioPlayed && SoundManager.Instance != null)
        {
            Debug.Log("Angel 아노말리");
            // Stop previous coroutine if it exists
            StopAnomalyAudio(ref angelAnomalyCoroutine, SoundManager.Instance.angelAnomalyAudio);
            // Start new coroutine to play for exactly 2 seconds
            angelAnomalyCoroutine = StartCoroutine(PlayAudioFor2Seconds(SoundManager.Instance.angelAnomalyAudio));
            angelAnomalyAudioPlayed = true;
        }
    }

    void BallerinaAnomalousAudio()
    {
        if (!ballerinaAnomalyAudioPlayed && SoundManager.Instance != null)
        {
            Debug.Log("Ballerina 아노말리");
            // Stop previous coroutine if it exists
            StopAnomalyAudio(ref ballerinaAnomalyCoroutine, SoundManager.Instance.ballerinaAnomalyAudio);
            // Start new coroutine to play for exactly 2 seconds
            ballerinaAnomalyCoroutine = StartCoroutine(PlayAudioFor2Seconds(SoundManager.Instance.ballerinaAnomalyAudio));
            ballerinaAnomalyAudioPlayed = true;
        }
    }

    // Coroutine to play audio for exactly 2 seconds
    IEnumerator PlayAudioFor2Seconds(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            audioSource.Play();
            yield return new WaitForSeconds(2f);
            audioSource.Stop();
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
        // Stop all anomaly audio when jumpscare happens
        StopAllAnomalyAudios();

        CameraManager cameraManager = FindObjectOfType<CameraManager>();
        cameraManager.SwitchToCamera(CameraMonitor.Office);
        jumpscareCanvas.enabled = true;
        jumpscareImage.enabled = true;
        StartCoroutine(DelayedGameOver());
    }

    // Helper method to stop all anomaly audio sources
    private void StopAllAnomalyAudios()
    {
        StopAnomalyAudio(ref nutsAnomalyCoroutine, SoundManager.Instance.nutsAnomalyAudio);
        StopAnomalyAudio(ref maskAnomalyCoroutine, SoundManager.Instance.maskAnomalyAudio);
        StopAnomalyAudio(ref angelAnomalyCoroutine, SoundManager.Instance.angelAnomalyAudio);
        StopAnomalyAudio(ref ballerinaAnomalyCoroutine, SoundManager.Instance.ballerinaAnomalyAudio);
    }

    IEnumerator DelayedGameOver()
    {
        yield return new WaitForSeconds(2f);
        DeactivateAllJumpscares();
        GameOver();
    }
}
