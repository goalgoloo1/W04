using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class PeanutConcentController : MonoBehaviour
{
    public bool isDraging = false;
    public bool isConnect = false;
    Vector3 mousePosition;
    Vector2 worldPosition;
    Light2D _light;
    LineRenderer _concentLine;

    void Start()
    {
        _light = transform.parent.GetComponentInChildren<Light2D>();
        _concentLine = GetComponent<LineRenderer>();
    }

    void Update()
    {
        ConnectPlug();
        _concentLine.positionCount = 2;
        _concentLine.SetPosition(0, new Vector3(-6.18f, 0.14f, 0));
        _concentLine.SetPosition(1, new Vector3(transform.position.x, transform.position.y));

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Concent")){
            Debug.Log("connect");
            isConnect = true;
            transform.position = new Vector2(transform.position.x+0.95f, collision.transform.position.y);
            StartCoroutine(LerpLightRadius(27.66f, 0.5f));
        }
    }
    IEnumerator LerpLightRadius(float targetRadius, float duration)
    {
        float startRadius = _light.pointLightOuterRadius; // 시작 값
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration; // 0에서 1로 진행률
            _light.pointLightOuterRadius = Mathf.Lerp(startRadius, targetRadius, t);
            _light.intensity = 2.0f;
            yield return null; // 다음 프레임까지 대기
        }

        _light.pointLightOuterRadius = targetRadius; // 정확히 목표 값으로 설정
    }
    /// <summary>
    /// connect plug method
    /// </summary>
    void ConnectPlug()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePosition = Input.mousePosition;
            worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.transform == this.transform)
                    isDraging = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDraging = false;
        }

        if (isDraging == true && isConnect == false)
        {
            mousePosition = Input.mousePosition;
            worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = worldPosition;
        }
        else if (isDraging == false && isConnect == false)
            transform.position = new Vector2(-4.4f, 0.14f);
    }
}