using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class DeskLight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler , IPointerDownHandler
{

    [SerializeField]
    private Light2D spotlight;

    private void Awake()
    {
        spotlight = GetComponentInChildren<Light2D>();
    }

    private void Start()
    {
        spotlight.color = Color.black;
    }

    public void OnLight()
    {
        spotlight.color = Color.white;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(MaskPuzzle.isPlaying || MaskPuzzle.isMaskOn) return;
        spotlight.color = Color.white;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        spotlight.color = Color.black;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        spotlight.color = Color.black;
    }
}
