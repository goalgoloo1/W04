using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class DeskLight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField]
    private Light2D light;

    private void Awake()
    {
        light = GetComponentInChildren<Light2D>();
    }

    private void Start()
    {
        light.color = Color.black;
    }

    public void OnLight()
    {
        light.color = Color.white;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerEnter);
        light.color = Color.white;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerEnter + " ddd");
        light.color = Color.black;
    }
}
