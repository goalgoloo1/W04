using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    
    // Awake method to set up singleton instance
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    //효과음
    public AudioSource cameraTransitionAudio;
    public AudioSource mouseClickAudio;

    //한성 : 몬스터 사운드
    public AudioSource nutsAnomalyAudio;
    public AudioSource nutsJumpscareAudio;

    public AudioSource maskAnomalyAudio;
    public AudioSource maskJumpscareAudio;

    public AudioSource angelAnomalyAudio;
    public AudioSource angelscareAudio;

    public AudioSource slendermanJumpscareAudio;

    public AudioSource childAnomalyAudio;

    public AudioSource ballerinaAnomalyAudio;
    public AudioSource ballerinaJumpscareAudio;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayAudio(mouseClickAudio);
        }
    }
    
    // Play audio if the AudioSource exists
    public void PlayAudio(AudioSource audioSource)
    {
        audioSource.Play();
    }
    public void StopAllAudio()
    {
        if (nutsAnomalyAudio != null && nutsAnomalyAudio.isPlaying) nutsAnomalyAudio.Stop();
        if (nutsJumpscareAudio != null && nutsJumpscareAudio.isPlaying) nutsJumpscareAudio.Stop();

        if (maskAnomalyAudio != null && maskAnomalyAudio.isPlaying) maskAnomalyAudio.Stop();
        if (maskJumpscareAudio != null && maskJumpscareAudio.isPlaying) maskJumpscareAudio.Stop();

        if (angelAnomalyAudio != null && angelAnomalyAudio.isPlaying) angelAnomalyAudio.Stop();
        if (angelscareAudio != null && angelscareAudio.isPlaying) angelscareAudio.Stop();

        if (slendermanJumpscareAudio != null && slendermanJumpscareAudio.isPlaying) slendermanJumpscareAudio.Stop();

        if (childAnomalyAudio != null && childAnomalyAudio.isPlaying) childAnomalyAudio.Stop();

        if (ballerinaAnomalyAudio != null && ballerinaAnomalyAudio.isPlaying) ballerinaAnomalyAudio.Stop();
        if (ballerinaJumpscareAudio != null && ballerinaJumpscareAudio.isPlaying) ballerinaJumpscareAudio.Stop();
    }
}
