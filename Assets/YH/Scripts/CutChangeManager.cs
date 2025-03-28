using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class CutChangeManager : MonoBehaviour
{
    public static CutChangeManager Instance;
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
        StartCoroutine(ToOfficeCoroutine());
    }
    
    public IEnumerator ToOfficeCoroutine()
    {
        PlayCut();
        yield return new WaitForSeconds(0.4f);
        CameraManager.Instance.SwitchToCamera(CameraManager.CameraMonitor.Office);
    }
}
