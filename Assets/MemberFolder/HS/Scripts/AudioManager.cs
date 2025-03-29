using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement; // Required for scene management

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioSource bgmSource; 
    [SerializeField] private AudioClip menuMusicClip; 

    public const string MASTER_KEY = "masterVolume";
    public const string MIXER_MASTER = "MasterVolume"; 

    private string currentSceneName; 

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; 
            currentSceneName = SceneManager.GetActiveScene().name; 
            LoadVolume(); 
            PlayAppropriateMusic(currentSceneName); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void OnDestroy()
    {
        if (instance == this) 
        {
            SceneManager.sceneLoaded -= OnSceneLoaded; 
        }
    }

    // Method called automatically when a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Always ensure the volume is applied when a scene loads
        LoadVolume();

        currentSceneName = scene.name; 
        PlayAppropriateMusic(currentSceneName);
    }

    void LoadVolume()
    {
        float masterVolumeLinear = PlayerPrefs.GetFloat(MASTER_KEY, 0.75f);
        ApplyVolumeToMixer(masterVolumeLinear);
    }

    public void SetMasterVolume(float linearVolume)
    {
        ApplyVolumeToMixer(linearVolume);

        PlayerPrefs.SetFloat(MASTER_KEY, linearVolume);
        PlayerPrefs.Save(); // Ensure it's written to disk immediately
        Debug.Log($"Volume Set and Saved: Linear={linearVolume}");
    }

    // Helper method to apply the volume correctly to the AudioMixer
    private void ApplyVolumeToMixer(float linearVolume)
    {
        //linearVolume = 0.1 ~ 3.0 로그 3은 0.5
        mixer.SetFloat(MIXER_MASTER, Mathf.Log10(linearVolume) * 20);
    }

    void PlayAppropriateMusic(string sceneName)
    {
        if (bgmSource == null || menuMusicClip == null)
        {
            Debug.LogWarning("BGM Source or Menu Music Clip not assigned in AudioManager.");
            return;
        }

        if (sceneName == "StartScene") // Or your exact Start Menu scene name
        {
            if (!bgmSource.isPlaying || bgmSource.clip != menuMusicClip)
            {
                bgmSource.clip = menuMusicClip;
                bgmSource.loop = true;
                bgmSource.Play();
                Debug.Log("Playing menu music.");
            }
        }
        else
        {
            // Stop music if it's playing and we're not in the start scene
            if (bgmSource.isPlaying)
            {
                bgmSource.Stop();
                Debug.Log("Stopping menu music.");
            }
        }
    }

    // Optional: Explicit methods if needed elsewhere
    public void PlayMenuMusic()
    {
        PlayAppropriateMusic("StartScene");
    }

    public void StopMusic()
    {
        if (bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.Stop();
            Debug.Log("Music stopped explicitly.");
        }
    }
}