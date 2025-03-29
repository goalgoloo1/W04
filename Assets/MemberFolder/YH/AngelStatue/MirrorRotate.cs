using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MirrorRotate : MonoBehaviour
{
    //Detects where the mouse is
    private Ray ray;
    private RaycastHit hit;
    public bool isRandomStart = true;
    [SerializeField]
    private float oriRotationZ;


    private void Start()
    {
        RotateRandom();
        transform.GetComponentInParent<AngelLaser>().StartGameAction += RotateRandom;
        oriRotationZ = transform.localEulerAngles.z;
    }

    

    private void RotateRandom()
    {
        if(isRandomStart)
            transform.Rotate(0, 0, Random.Range(0, 8) * 45);
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, oriRotationZ);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 클릭
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                transform.Rotate(0f, 0f, 45f); // Z축으로 90도 회전 (2D 회전)
            }
        }
    }
}