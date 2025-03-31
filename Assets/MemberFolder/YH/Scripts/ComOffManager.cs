using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class ComOffManager : MonoBehaviour
{
    public static ComOffManager Instance;
    private PlayableDirector pd;
    
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

        pd = GetComponent<PlayableDirector>();
    }
    
    public void PlayCut()
    {
        pd.Play();
    }
    
    
    public void PlayCutCoroutine()
    {
        StopCoroutine(ResetCoroutine());
        StartCoroutine(ResetCoroutine());
    }
    
    public IEnumerator ResetCoroutine()
    {
        CameraManager.Instance.isClick = false;
        PlayCut();
        yield return new WaitForSeconds(0.4f);
        PuzzleManager.Instance.ResetButton();
        yield return new WaitForSeconds(1.0f);
        CameraManager.Instance.isClick = true;
    }
}
