using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AngelGameController : MonoBehaviour
{
    public GameObject[] Games;
    private void Start()
    {
        OffGame();
    }

    private void OffGame()
    {
        for (int i = 0; i < Games.Length; i++)
        {
            Games[i].SetActive(false);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            StartGame();
    }

    private void StartGame()
    {
        int random = Random.Range(0, Games.Length);
        for (int i = 0; i < Games.Length; i++)
        {
            if (i == random)
            {
                Games[i].SetActive(true);
                Games[i].GetComponent<AngelLaser>().GameStart();
            }
            else
            {
                Games[i].SetActive(false);
            }
        }
    }
}
