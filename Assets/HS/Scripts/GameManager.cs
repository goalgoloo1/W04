using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using static CameraManager;
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
    
    // Track when audio was last played
    private float nutsAnomalyLastPlayTime = 0f;
    private float maskAnomalyLastPlayTime = 0f;
    private float angelAnomalyLastPlayTime = 0f;
    private float ballerinaAnomalyLastPlayTime = 0f;
    
    // Minimum time between audio playback (2 seconds)
    private const float MIN_AUDIO_REPLAY_TIME = 2.0f;

    // References to active coroutines for stopping audio
    private Coroutine nutsAnomalyCoroutine = null;
    private Coroutine maskAnomalyCoroutine = null;
    private Coroutine angelAnomalyCoroutine = null;
    private Coroutine ballerinaAnomalyCoroutine = null;
    
    // Track if Update has already processed audio this frame
    private int lastFrameCount = -1;

    void Start()
    {
        DeactivateAllJumpscares();
    }

    void Update()
    {
        // Only process audio once per frame
        if (Time.frameCount == lastFrameCount)
            return;
            
        lastFrameCount = Time.frameCount;
        
        CheckAndResetAudioFlags();
        
        // Check monster states and play audio if needed
        if (MonsterManager.Instance != null)
        {
            CheckMonsterAnomaly();
            CheckMonsterCritical();
        }
    }
    
    private void CheckMonsterAnomaly()
    {
        if (MonsterManager.Instance.GetMonster(1).state == MonsterState.Anomalous) { NutsAnomalousAudio(); }
        if (MonsterManager.Instance.GetMonster(3).state == MonsterState.Anomalous) { MaskAnomalousAudio(); }
        if (MonsterManager.Instance.GetMonster(4).state == MonsterState.Anomalous) { AngelAnomalousAudio(); }
        //if (MonsterManager.Instance.GetMonster(5).state == MonsterState.Anomalous) { SlendermanAnomalousAudio(); }
        if (MonsterManager.Instance.GetMonster(7).state == MonsterState.Anomalous) { BallerinaAnomalousAudio(); }
    }
    
    private void CheckMonsterCritical() 
    {
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

    // Check if enough time has passed since this audio was last played
    private bool CanPlayAudio(ref float lastPlayTime)
    {
        if (Time.time - lastPlayTime >= MIN_AUDIO_REPLAY_TIME)
        {
            lastPlayTime = Time.time;
            return true;
        }
        return false;
    }
    
    void NutsAnomalousAudio()
    {
        if (!nutsAnomalyAudioPlayed && SoundManager.Instance != null && 
            CanPlayAudio(ref nutsAnomalyLastPlayTime))
        {
            Debug.Log("Nuts 아노말리");
            // Stop previous coroutine if it exists
            StopAnomalyAudio(ref nutsAnomalyCoroutine, SoundManager.Instance.nutsAnomalyAudio);
            // Start new coroutine to play for exactly 2 seconds
            if (SoundManager.Instance.nutsAnomalyAudio != null)
            {
                nutsAnomalyCoroutine = StartCoroutine(PlayAudioFor2Seconds(SoundManager.Instance.nutsAnomalyAudio));
                nutsAnomalyAudioPlayed = true;
            }
        }
    }

    void MaskAnomalousAudio()
    {
        if (!maskAnomalyAudioPlayed && SoundManager.Instance != null && 
            CanPlayAudio(ref maskAnomalyLastPlayTime))
        {
            Debug.Log("Mask 아노말리");
            // Stop previous coroutine if it exists
            StopAnomalyAudio(ref maskAnomalyCoroutine, SoundManager.Instance.maskAnomalyAudio);
            // Start new coroutine to play for exactly 2 seconds
            if (SoundManager.Instance.maskAnomalyAudio != null)
            {
                maskAnomalyCoroutine = StartCoroutine(PlayAudioFor2Seconds(SoundManager.Instance.maskAnomalyAudio));
                maskAnomalyAudioPlayed = true;
            }
        }
    }

    void AngelAnomalousAudio()
    {
        if (!angelAnomalyAudioPlayed && SoundManager.Instance != null && 
            CanPlayAudio(ref angelAnomalyLastPlayTime))
        {
            Debug.Log("Angel 아노말리");
            // Stop previous coroutine if it exists
            StopAnomalyAudio(ref angelAnomalyCoroutine, SoundManager.Instance.angelAnomalyAudio);
            // Start new coroutine to play for exactly 2 seconds
            if (SoundManager.Instance.angelAnomalyAudio != null)
            {
                angelAnomalyCoroutine = StartCoroutine(PlayAudioFor2Seconds(SoundManager.Instance.angelAnomalyAudio));
                angelAnomalyAudioPlayed = true;
            }
        }
    }

    void BallerinaAnomalousAudio()
    {
        if (!ballerinaAnomalyAudioPlayed && SoundManager.Instance != null && 
            CanPlayAudio(ref ballerinaAnomalyLastPlayTime))
        {
            Debug.Log("Ballerina 아노말리");
            // Stop previous coroutine if it exists
            StopAnomalyAudio(ref ballerinaAnomalyCoroutine, SoundManager.Instance.ballerinaAnomalyAudio);
            // Start new coroutine to play for exactly 2 seconds
            if (SoundManager.Instance.ballerinaAnomalyAudio != null)
            {
                ballerinaAnomalyCoroutine = StartCoroutine(PlayAudioFor2Seconds(SoundManager.Instance.ballerinaAnomalyAudio));
                ballerinaAnomalyAudioPlayed = true;
            }
        }
    }

    // Coroutine to play audio for exactly 2 seconds
    IEnumerator PlayAudioFor2Seconds(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            // Only play if it's not already playing
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                yield return new WaitForSeconds(2f);
                if (audioSource != null && audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
            else
            {
                yield return new WaitForSeconds(2f);
            }
        }
    }
    
    public void PlayAudio(AudioSource audioSource)
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    
    void NutsJumpscare()
    {
        Debug.Log("Nuts한테 쥬금");
        PlayAudio(SoundManager.Instance.nutsJumpscareAudio);
        ShowJumpscareOverlay(nutsJumpscareImage);
    }

    void MaskJumpscare()
    {
        Debug.Log("Mask한테 쥬금");
        PlayAudio(SoundManager.Instance.maskJumpscareAudio);
        ShowJumpscareOverlay(maskJumpscareImage);
    }

    void AngelJumpscare()
    {
        Debug.Log("Angel한테 쥬금");
        PlayAudio(SoundManager.Instance.angelscareAudio);
        ShowJumpscareOverlay(angelJumpscareImage);
    }

    void SlendermanJumpscare()
    {
        Debug.Log("Slenderman한테 쥬금");
        PlayAudio(SoundManager.Instance.slendermanJumpscareAudio);
        ShowJumpscareOverlay(slendermanJumpscareImage);
    }

    void BallerinaJumpscare()
    {
        Debug.Log("Ballerina한테 쥬금");
        PlayAudio(SoundManager.Instance.ballerinaJumpscareAudio);
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
