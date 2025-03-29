using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CCTVController : MonoBehaviour
{
    [SerializeField] Canvas cctvCanvas;         // 캔버스 참조 (Inspector에서 할당)
    [SerializeField] GameObject officeButton;     // 버튼 게임오브젝트 (Inspector에서 할당)

    private Button officeBtn;  // Button 컴포넌트 참조

    void Start()
    {
        // officeButton에 Button 컴포넌트가 있다면 가져옵니다.
        officeBtn = officeButton.GetComponent<Button>();
        if (officeBtn != null)
        {
            // 버튼 클릭 시 ToggleCanvasRoutine 코루틴을 실행하도록 등록
            officeBtn.onClick.AddListener(() => StartCoroutine(ToggleCanvasRoutine()));
        }
    }

    // 캔버스를 껐다가 일정 시간 후 다시 켜는 코루틴
    IEnumerator ToggleCanvasRoutine()
    {
        // 캔버스 비활성화 (꺼짐)
        cctvCanvas.enabled = false;
        // 원하는 시간(예제에서는 2초) 동안 대기
        yield return new WaitForSeconds(2f);
        // 캔버스 활성화 (다시 켜짐)
        cctvCanvas.enabled = true;
    }
}
