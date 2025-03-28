using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class CutChangeManager : MonoBehaviour
{
    public static CutChangeManager Instance;
    public PlayableAsset[] playableAsset;
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
    
    public void ToOfficePlayCut()
    {
        StopAllCoroutines();
        StartCoroutine(ToOfficeCoroutine());
    }
    
    public void ResetPlayCut()
    {
        StopAllCoroutines();
        StartCoroutine(ResetCoroutine());
    }
    
    public IEnumerator ToOfficeCoroutine()
    {
        pd.playableAsset = playableAsset[0];
        PlayCut();
        yield return new WaitForSeconds(0.4f);
        CameraManager.Instance.SwitchToCamera(CameraManager.CameraMonitor.Office);
    }
    
    public IEnumerator ResetCoroutine()
    {
        pd.playableAsset = playableAsset[1];
        CameraManager.Instance.isClick = false;
        PlayCut();
        yield return new WaitForSeconds(0.4f);
        PuzzleManager.Instance.ResetButton();
        yield return new WaitForSeconds(1.0f);
        CameraManager.Instance.isClick = true;
    }
}
