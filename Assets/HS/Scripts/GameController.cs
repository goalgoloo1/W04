using UnityEngine;
using Unity.Cinemachine;

public class GameController : MonoBehaviour
{
    public enum CameraMonitor
    {
        Office,
        Cam1,
        Cam2,
        Cam3,
        Cam4,
        Cam5,
        Cam6,
        Cam7,
        Cam8,

    }

    [SerializeField] private CinemachineCamera[] cameras;
    private CameraMonitor currentMonitor = CameraMonitor.Office;


    //Detects where the mouse is
    private Ray ray;
    private RaycastHit hit;


    void Start()
    {
        SwitchToCamera(CameraMonitor.Office);
    }

    void Update()
    {
        //Where the mouse is pointing
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit) && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log(hit.collider.name);
            if (hit.collider.name == "001") { SwitchToCamera(CameraMonitor.Cam1); }
            if (hit.collider.name == "002") { SwitchToCamera(CameraMonitor.Cam2); }
            if (hit.collider.name == "003") { SwitchToCamera(CameraMonitor.Cam3); }
            if (hit.collider.name == "004") { SwitchToCamera(CameraMonitor.Cam4); }
            if (hit.collider.name == "005") { SwitchToCamera(CameraMonitor.Cam5); }
            if (hit.collider.name == "006") { SwitchToCamera(CameraMonitor.Cam6); }
            if (hit.collider.name == "007") { SwitchToCamera(CameraMonitor.Cam7); }
            if (hit.collider.name == "ToOffice") { SwitchToCamera(CameraMonitor.Office); }
            if (hit.collider.name == "Cctv") { SwitchToCamera(CameraMonitor.Cam1); }

        }
    }
    private void SwitchToCamera(CameraMonitor cameraMonitor)
    {
        int cameraIndex = (int)cameraMonitor;
        TempData.playerViewingCamera = cameraMonitor;
        Debug.Log(TempData.playerViewingCamera + " hi");


        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        cameras[cameraIndex].gameObject.SetActive(true);
    }
    
}