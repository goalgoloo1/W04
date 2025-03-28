using DG.Tweening;
using UnityEngine;
using System.Collections;

public class FadingPaper : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    private Tween m_fadeTween;
    void Start()
    {
        FadeIn(1f);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(FadeOutAndLoadScene());
        }
    }
    public void FadeIn(float duration)
    {
        Fade(1f, duration, () =>
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        });
    }
    public void FadeOut(float duration)
    {
        Fade(0f, duration, () =>
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        });
    }
    void Fade(float endValue, float duration, TweenCallback onEnd)
    {
        if (m_fadeTween != null)
        {
            m_fadeTween.Kill();
        }
        m_fadeTween = canvasGroup.DOFade(endValue, duration).OnComplete(onEnd);
    }
    IEnumerator FadeOutAndLoadScene()
    {
        FadeOut(0.3f);
        yield return new WaitForSeconds(0.8f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");

    }
}
