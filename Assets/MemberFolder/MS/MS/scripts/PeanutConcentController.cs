using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using Random = UnityEngine.Random;

public class PeanutConcentController : MonoBehaviour
{
    public static PeanutConcentController instance;
    public bool isDraging = false;
    public bool isConnect = false;
    public GameObject successText;
    public GameObject failureText;

    Vector3 startPos;
    Vector3 mousePosition;
    Vector2 worldPosition;
    Light2D _light;
    LineRenderer _concentLine;

    private GameObject targetConcent;
    private bool hasDragged = false;

    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        startPos = transform.position;
        _light = transform.parent.GetComponentInChildren<Light2D>();
        _concentLine = GetComponent<LineRenderer>();

        // 시작 시 UI 텍스트 모두 숨김
        successText.gameObject.SetActive(false);
        failureText.gameObject.SetActive(false);

        GameObject[] concentList = GameObject.FindGameObjectsWithTag("Concent");
        if (concentList.Length > 0)
        {
            int randomIndex = Random.Range(0, concentList.Length);
            targetConcent = concentList[randomIndex];
            Debug.Log("선택된 콘센트: " + targetConcent.name);
        }
        else
        {
            Debug.LogError("씬에 Concent 태그를 가진 오브젝트가 없습니다.");
        }
    }

    public void Reset()
    {
        GameObject[] concentList = GameObject.FindGameObjectsWithTag("Concent");
        int randomIndex = Random.Range(0, concentList.Length);
        targetConcent = concentList[randomIndex];
        Debug.Log("선택된 콘센트: " + targetConcent.name);
        isConnect = false;
        isDraging = false;
        hasDragged = false;
        _light.intensity = 0.5f;
        _light.pointLightOuterRadius = 8.23f;

    }
    void Update()
    {
        ConnectPlug();

        _concentLine.positionCount = 2;
        _concentLine.SetPosition(0, startPos - new Vector3(2.00f, 0.09f));
        _concentLine.SetPosition(1, new Vector3(transform.position.x - 1, transform.position.y));

        // // 드래그 시도가 아직 없으면 아무 텍스트도 표시하지 않음
        // if (!hasDragged)
        // {
        //     successText.gameObject.SetActive(false);
        //     failureText.gameObject.SetActive(false);
        // }
        // // 드래그가 끝난 상태에서 텍스트 표시
        // else if (!isDraging)
        // {
        //     if (isConnect)
        //     {
        //         successText.gameObject.SetActive(true);
        //         failureText.gameObject.SetActive(false);
        //     }
        //     else
        //     {
        //         successText.gameObject.SetActive(false);
        //         failureText.gameObject.SetActive(true);
        //     }
        // }
        // else
        // {
        //     // 드래그 중일 때는 텍스트 모두 숨김
        //     successText.gameObject.SetActive(false);
        //     failureText.gameObject.SetActive(false);
        // }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isConnect)
            return;

        if (collision.gameObject == targetConcent)
        {
            Debug.Log("connect");
            isConnect = true;
            transform.position = new Vector2(transform.position.x + 0.95f, collision.transform.position.y);
            StartCoroutine(LerpLightRadius(27.66f, 0.5f));
            CutChangeManager.Instance.PlayCutCoroutine();
            
        }
        else
        {
            transform.position = new Vector2(transform.position.x + 0.95f, transform.position.y);
        }
    }

    IEnumerator LerpLightRadius(float targetRadius, float duration)
    {
        float startRadius = _light.pointLightOuterRadius;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;
            _light.pointLightOuterRadius = Mathf.Lerp(startRadius, targetRadius, t);
            _light.intensity = 2.0f;
            yield return null;
        }

        _light.pointLightOuterRadius = targetRadius;
    }

    /// <summary>
    /// 플러그 이동 및 충돌 검사 메서드
    /// </summary>
    void ConnectPlug()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 최초 드래그 시도 시 hasDragged를 true로 설정
            hasDragged = true;

            mousePosition = Input.mousePosition;
            worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

            if (hit.collider != null && hit.transform == this.transform)
            {
                isDraging = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDraging = false;
        }

        if (isDraging && !isConnect)
        {
            mousePosition = Input.mousePosition;
            worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = worldPosition;
        }
        else if (!isDraging && !isConnect)
        {
            // 드래그가 끝났으나 연결되지 않았을 경우 원래 자리로 복귀
            transform.localPosition = new Vector2(-4.4f, 0.14f);
        }
    }
}
