using System;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;
    public AngelGameController angelGameController;
    public int currentPuzzle = 0;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        angelGameController = GetComponentInChildren<AngelGameController>();
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
}
