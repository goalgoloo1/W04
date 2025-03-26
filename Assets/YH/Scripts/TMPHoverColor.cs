using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;


public class TMPHoverColor : MonoBehaviour,  IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TMP_Text text;
    public Color hoverColor;
    public Color defaultColor;
    public Color pressColor;
    private void Reset()
    {
        text = GetComponent<TMP_Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ButtonColorChangeCoroutine(hoverColor));
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ButtonColorChangeCoroutine(defaultColor));
     
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ButtonColorChangeCoroutine(pressColor));
    }

    IEnumerator ButtonColorChangeCoroutine(Color newColor)
    {
        Color oldColor = text.color;
        float elapsedTime = 0f;
        float duration = 0.2f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            text.color = Color.Lerp(oldColor, newColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 마지막 보정
        text.color = newColor;
    }
}
