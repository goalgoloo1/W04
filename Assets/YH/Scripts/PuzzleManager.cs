using System;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;
    public AngelGameController angelGameController;
    public MaskPuzzle maskPuzzle;
    public int currentPuzzle = 0;
    
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        angelGameController = GetComponentInChildren<AngelGameController>();
        maskPuzzle = GetComponentInChildren<MaskPuzzle>();
    }


    public void StartAngel()
    {
        angelGameController.StartGame();
        currentPuzzle = 1;
    }

    public void EndAngel()
    {
        angelGameController.OffGame();
        currentPuzzle = 0;
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
