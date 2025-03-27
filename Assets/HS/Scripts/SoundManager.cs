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
    public AudioSource cameraTransition;
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
}
