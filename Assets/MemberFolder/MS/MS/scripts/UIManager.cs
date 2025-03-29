using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] GameObject startPanel, reportPanel, exitPanel, backPanel;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void reportScene()
    {
        reportPanel.SetActive(true);
    }
    public void BackMenuScene()
    {
        reportPanel.SetActive(false);
    }
    public void startGameScene()
    {
        SceneManager.LoadScene("OpeningScene");
    }
    public void menuScene()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
