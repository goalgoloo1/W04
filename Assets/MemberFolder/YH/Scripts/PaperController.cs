using System;
using UnityEngine;
using UnityEngine.UI;

public class PaperController : MonoBehaviour
{
    public static PaperController Instance;
    public CanvasGroup canvasGroup;
    public Image image;
    public GameObject paper;
    private GameObject[] papers;
    public Button leftButton;
    public Button rightButton;
    
    public int currentIdx = 0;
    public bool isPaper = false;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OffPaper();
        leftButton.onClick.AddListener(OnClickPrevious);
        rightButton.onClick.AddListener(OnClickNext);
        papers = new GameObject[paper.transform.childCount];
        for (int i = 0; i < paper.transform.childCount; i++)
        {
            papers[i] = paper.transform.GetChild(i).gameObject;
        }
    }

    public void OnPaper()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        isPaper = true;
    }
    
    public void OffPaper()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        isPaper = false;
    }
    
    
    public void OnClickNext()
    {
        if (currentIdx < papers.Length - 1)
        {
            currentIdx++;
            SetPaper(currentIdx);
        }
        else
        {
            currentIdx = 0;
            SetPaper(currentIdx);
        }
    }
    
    public void OnClickPrevious()
    {
        if (currentIdx > 0)
        {
            currentIdx--;
            SetPaper(currentIdx);
        }
        else
        {
            currentIdx = papers.Length - 1;
            SetPaper(currentIdx);
        }
    }
    
    public void SetPaper(int idx)
    {
        currentIdx = idx;
        papers[idx].SetActive(true);
        for (int i = 0; i < papers.Length; i++)
        {
            if (i != idx)
            {
                papers[i].SetActive(false);
            }
        }
    }
    
}
