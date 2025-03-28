using UnityEngine;
using Unity.Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    public enum CameraMonitor
    {
        Office,
        Cam1,
        Cam2,
        Cam3,
        Cam4,
        Cam5,
        Cam6,
        Puzzle1,
        Puzzle2,
        Puzzle3,
        
    }

    [SerializeField] private CinemachineCamera[] cameras;
    public int currentCamera = 0;
    
    //Detects where the mouse is
    private Ray ray;
    private RaycastHit hit;

    private void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        SwitchToCamera(CameraMonitor.Office);
    }

    void Update()
    {
        CameraMapClick();
        CameraMapKeyBoard();
    }
    private void CameraMapClick() {
        if(PaperController.Instance.isPaper) return;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit) && Input.GetKeyDown(KeyCode.Mouse0) && !MaskPuzzle.isMaskOn && MaskPuzzle.isPlaying == false)
        {
            //Debug.Log(hit.collider.name);
            if (hit.collider.name == "001") { SwitchToCamera(CameraMonitor.Cam1); currentCamera = 1; }
            if (hit.collider.name == "002") { SwitchToCamera(CameraMonitor.Cam2); currentCamera = 2;}
            if (hit.collider.name == "003") { SwitchToCamera(CameraMonitor.Cam3); currentCamera = 3;}
            if (hit.collider.name == "004") { SwitchToCamera(CameraMonitor.Cam4); currentCamera = 4;}
            if (hit.collider.name == "005") { SwitchToCamera(CameraMonitor.Cam5); currentCamera = 5;}
            if (hit.collider.name == "006") { SwitchToCamera(CameraMonitor.Cam6); currentCamera = 6; }
            if (hit.collider.name == "ToOffice") { SwitchToCamera(CameraMonitor.Office); currentCamera = 0;}
            if (hit.collider.name == "Cctv") { SwitchToCamera(CameraMonitor.Cam1); currentCamera = 1;} // Default가 1번 카메라
            if (hit.collider.name == "Desk1") { SwitchToCamera(CameraMonitor.Puzzle1); currentCamera = -1; } //거울
            if (hit.collider.name == "Desk2") { SwitchToCamera(CameraMonitor.Puzzle2); currentCamera = -2; } //라디오
            if (hit.collider.name == "Desk3") { SwitchToCamera(CameraMonitor.Puzzle3); currentCamera = -3; } //전선
            if (hit.collider.name == "Desk4"){ PuzzleManager.Instance.OnMask(); currentCamera = -4; }
            if (hit.collider.name == "Reset")
            {
                PuzzleManager.Instance.ResetButton();
            }
            if (hit.collider.name == "Paper")
            {
                PaperController.Instance.OnPaper();
            }
        }
    }
    
    private void CameraMapKeyBoard()
    {
        if(currentCamera < 1) return;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchToCamera(CameraMonitor.Cam1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchToCamera(CameraMonitor.Cam2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchToCamera(CameraMonitor.Cam3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchToCamera(CameraMonitor.Cam4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchToCamera(CameraMonitor.Cam5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SwitchToCamera(CameraMonitor.Cam6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchToCamera(CameraMonitor.Office);
            currentCamera = 0;
        }
    }
    
    
    public void SwitchToCamera(CameraMonitor cameraMonitor)
    {

        int cameraIndex = (int)cameraMonitor;
        TempData.playerViewingCamera = cameraMonitor;
        //Debug.Log(TempData.playerViewingCamera + " hi");


        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        cameras[cameraIndex].gameObject.SetActive(true);

        if (cameraMonitor == CameraMonitor.Cam1)
        {
            RoomManager.Instance.SetCamImage(1);
            currentCamera = 1;
        }
        if (cameraMonitor == CameraMonitor.Cam2)
        {
            RoomManager.Instance.SetCamImage(2);
            currentCamera = 2; 
        }
        if (cameraMonitor == CameraMonitor.Cam3)
        {
            RoomManager.Instance.SetCamImage(3);
            currentCamera = 3; 
        }
        if (cameraMonitor == CameraMonitor.Cam4) // 슬렌더맨의 특수 상황인데 나중에 따로 빼는걸 추천합니다.
        {
            currentCamera = 4; 
            RoomManager.Instance.SetCamImage(4);
            MonsterManager.Instance.slenderTimer = 0;
            if (MonsterManager.Instance.GetMonster(5).state == MonsterState.Anomalous)
            {
                int n = Random.Range(0, 2);
                if (n == 0)
                {
                    MonsterManager.Instance.SetCommon(5);
                }
            }
        }
        if (cameraMonitor == CameraMonitor.Cam5)
        {
            currentCamera = 5; 
            RoomManager.Instance.SetCamImage(5);
        }
        if (cameraMonitor == CameraMonitor.Cam6)
        {
            currentCamera = 6; 
            RoomManager.Instance.SetCamImage(6);
        }
        if(cameraMonitor == CameraMonitor.Puzzle1)
        {
            currentCamera = -1; 
            PuzzleManager.Instance.StartAngel();
        }
        if(cameraMonitor == CameraMonitor.Puzzle2)
        {
            currentCamera = -2; 
        }
        if(cameraMonitor == CameraMonitor.Puzzle3)
        {
            currentCamera = -3; 
        }

        if (cameraMonitor == CameraMonitor.Office)
        {
            currentCamera = 0;
            if (PuzzleManager.Instance.currentPuzzle == 1)
            {
                PuzzleManager.Instance.EndAngel();
            }
        }
        
    }
    
    
    
    

}