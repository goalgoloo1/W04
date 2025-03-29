using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] GameObject startPanel, reportPanel, backPanel, settingPanel;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this) // Ensure we destroy duplicates
        {
            Destroy(gameObject);
        }

    }

    public void ReportScene()
    {
        reportPanel?.SetActive(true); // Use null-conditional operator for safety
        settingPanel?.SetActive(false);
        startPanel?.SetActive(false);
        backPanel?.SetActive(true); // Show back button?
    }

    public void BackMenuScene()
    {
        reportPanel?.SetActive(false);
        settingPanel?.SetActive(false);
        startPanel?.SetActive(true);
        backPanel?.SetActive(false); // Hide back button?
    }

    public void StartGameScene()
    {
        // If you want explicit stop *immediately*:
        // if (AudioManager.instance != null)
        // {
        //     AudioManager.instance.StopMusic();
        // }
        SceneManager.LoadScene("Main"); // Ensure "Main" is the exact name of your game scene
    }

    public void MenuScene()
    {
        // Play music *after* loading the scene (AudioManager handles this now via OnSceneLoaded)
        SceneManager.LoadScene("StartScene"); // Ensure "StartScene" is the exact name
    }

    public void SettingScene()
    {
        settingPanel?.SetActive(true);
        reportPanel?.SetActive(false);
        // Potentially hide startPanel as well?
        startPanel?.SetActive(false);
        backPanel?.SetActive(true); // Show back button?
    }
}