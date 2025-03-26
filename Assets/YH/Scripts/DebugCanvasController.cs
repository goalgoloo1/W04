using System;
using UnityEngine;
using TMPro;

public class DebugCanvasController : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TMP_Text Debugtext;

    private void Start()
    {
        OnCanvas();
        canvasGroup.blocksRaycasts = false;
    }
    
    private void Update()
    {
        SetText();
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (canvasGroup.alpha == 0)
            {
                OnCanvas();
            }
            else
            {
                OffCanvas();
            }
        }
    }

    private void SetText()
    {
        string[] mon = new string[10];
        for (int i = 1; i <= 7; i++)
        {
            if (i == 2) continue;
            if (MonsterManager.Instance.GetMonster(i).state == MonsterState.Common)
                mon[i] = "Common";
            else if (MonsterManager.Instance.GetMonster(i).state == MonsterState.Anomalous)
                mon[i] = "Anomalous";
            else
                mon[i] = "Critical";
        }

        Debugtext.text = "Cam1" + " : " + mon[1]
                         + "\nCam2" + " : " + mon[7]
                         + "\nCam3" + " : " + mon[6]
                         + "\nCam4" + " : " + mon[5]
                         + "\nCam5" + " : " + mon[4]
                         + "\nCam6" + " : " + mon[3];

    }
    private void OnCanvas()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
    }
    
    private void OffCanvas()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
    }
}
