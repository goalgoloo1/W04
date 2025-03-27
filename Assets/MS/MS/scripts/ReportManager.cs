using UnityEngine;

public class ReportManager : MonoBehaviour
{
    [SerializeField] GameObject[] report;

    private int currentIndex = 0;

    void Start()
    {
        // 모든 이미지 비활성화
        foreach (GameObject r in report)
        {
            r.SetActive(false);
        }

        // 첫 번째 이미지만 활성화
        if (report.Length > 0)
            report[0].SetActive(true);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 클릭
        {
            ShowNextImage();
        }
    }

    void ShowNextImage()
    {
        if (currentIndex < report.Length - 1)
        {
            // 현재 이미지 비활성화
            report[currentIndex].SetActive(false);
            currentIndex++;
            // 다음 이미지 활성화
            report[currentIndex].SetActive(true);
        }
        // 원하면 아래 주석 해제해서 마지막 다음에 처음으로 돌아가게도 가능!
        
        else
        {
            report[currentIndex].SetActive(false);
            currentIndex = 0;
            report[currentIndex].SetActive(true);
        }
        
    }
}
