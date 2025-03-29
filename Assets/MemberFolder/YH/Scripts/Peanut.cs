using UnityEngine;
using UnityEngine.Rendering.Universal;


public class Peanut : MonoBehaviour
{
    public Light2D peanutLight;
    public float timer = 0;
    public float maxTime = 0.1f;
    
    private void Start()
    {
        peanutLight = GetComponentInChildren<Light2D>();
    }
    
    private void Update()
    {
        if (MonsterManager.Instance.GetMonster(1).state ==  MonsterState.Anomalous)
        {
            timer += Time.deltaTime;
            if (timer >= maxTime)
            {
                if (peanutLight.intensity == 200f)
                {
                    peanutLight.intensity = 0;
                    maxTime = Random.Range(0.05f, 0.3f);
                }
                else if(peanutLight.intensity == 0)
                {
                    peanutLight.intensity = 200f;
                    maxTime = Random.Range(0.05f, 0.3f);
                }
                timer = 0;
            }
        }
        else
        {
            peanutLight.intensity = 200f;
        }
    }
}
