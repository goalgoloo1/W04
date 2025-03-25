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
        Cam9
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
        }
        // Simple camera switching with number keys (1-9 for stages, 0 for office)
        if (Input.GetKeyDown(KeyCode.Alpha0)) SwitchToCamera(CameraMonitor.Office);
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchToCamera(CameraMonitor.Cam1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchToCamera(CameraMonitor.Cam2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchToCamera(CameraMonitor.Cam3);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SwitchToCamera(CameraMonitor.Cam4);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SwitchToCamera(CameraMonitor.Cam5);
        if (Input.GetKeyDown(KeyCode.Alpha6)) SwitchToCamera(CameraMonitor.Cam6);
        if (Input.GetKeyDown(KeyCode.Alpha7)) SwitchToCamera(CameraMonitor.Cam7);
        if (Input.GetKeyDown(KeyCode.Alpha8)) SwitchToCamera(CameraMonitor.Cam8);
        if (Input.GetKeyDown(KeyCode.Alpha9)) SwitchToCamera(CameraMonitor.Cam9);
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
    private void OnChangeCamera(CameraMonitor monitor)
    {
        //if (!TempData.dying && !TempData.playerWon)
        //{
        currentMonitor = monitor;
        SwitchToCamera(monitor);
        //blipClip.Play();
        //}
    }
}