using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(LoadClearScene());
    }

    private System.Collections.IEnumerator LoadClearScene()
    {
        yield return new WaitForSeconds(25f);
        SceneManager.LoadScene("ClearScene");
    }


}
