using System;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;
    public MirrorPuzzleManager mpm;
    public MaskPuzzle maskPuzzle;
    public int currentPuzzle = 0;
    
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        mpm = MirrorPuzzleManager.Instance;
        maskPuzzle = GetComponentInChildren<MaskPuzzle>();
    }


    public void StartMirror()
    {
        currentPuzzle = 0;
        mpm.ResetPuzzle();
        currentPuzzle = 1;
    }
    

    public void OnMask()
    {
        maskPuzzle.OnMask();
    }
    
    public void OffMask()
    {
        maskPuzzle.OffMask();
    }

    public void ResetButton()
    {
        Debug.Log("Reset Button");
        MonsterManager.Instance.SetCommon(6);
        for(int i = 0; i < 6; i++)
            RoomManager.Instance.SetCamImage(i+1);
    }
}
