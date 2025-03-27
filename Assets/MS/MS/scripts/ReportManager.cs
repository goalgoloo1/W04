using UnityEngine;

public class ReportManager : MonoBehaviour
{
    [SerializeField] GameObject[] report;
    [SerializeField] GameObject leftButton;
    [SerializeField] GameObject rightButton;

    private int currentIndex = 0;

    void Start()
    {
        // 모든 리포트 비활성화
        foreach (GameObject r in report)
            r.SetActive(false);

        // 첫 번째 리포트만 표시
        if (report.Length > 0)
            report[0].SetActive(true);

        UpdateButtonVisibility();
    }

    public void OnClickNext()
    {
        if (currentIndex < report.Length - 1)
        {
            report[currentIndex].SetActive(false);
            currentIndex++;
            report[currentIndex].SetActive(true);
            UpdateButtonVisibility();
        }
    }

    public void OnClickPrevious()
    {
        if (currentIndex > 0)
        {
            report[currentIndex].SetActive(false);
            currentIndex--;
            report[currentIndex].SetActive(true);
            UpdateButtonVisibility();
        }
    }

    void UpdateButtonVisibility()
    {
        leftButton.SetActive(currentIndex > 0);
        rightButton.SetActive(currentIndex < report.Length - 1);
    }
}
