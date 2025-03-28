using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class MaskPuzzle : MonoBehaviour
{
    private PlayableDirector pd;
    [SerializeField]
    private float time; // 착용하고 있는 시간
    private float maxTime = 0.1f; // 클리어 되는 시간
    public static bool isMaskOn;
    public static bool isPlaying;
    public TimelineAsset MaskOn;
    public TimelineAsset MaskOff;
    private void Start()
    {
        pd = GetComponent<PlayableDirector>();
        isMaskOn = false;
        isPlaying = false;
    }
    public void Update()
    {
        TimeUpdate();
        if(Input.GetKeyDown(KeyCode.Mouse0))
            OffMask();
    }

    public void OnMask()
    {
        if (!isMaskOn && !isPlaying)
        {
            pd.playableAsset = MaskOn;
            time = 0;
            pd.Play();
            isPlaying = true;
        }
    }

    public void OffMask()
    {
        if (isMaskOn && !isPlaying)
        {
            pd.playableAsset = MaskOff;
            pd.Play();
            isPlaying = true;
        }
    }
    
    public void OnTimelineStopped()
    {
        isMaskOn = !isMaskOn;
        isPlaying = false;
    }

    private void TimeUpdate()
    {
        if(isMaskOn)
            time += Time.deltaTime;
        if (time >= maxTime)
        {
            time = 0;
            MonsterManager.Instance.SetCommon(3);
        }
    }
}
