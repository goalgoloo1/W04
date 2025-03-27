using UnityEngine;
using UnityEngine.Rendering.Universal;


public class Peanut : MonoBehaviour
{
    public Light2D light;
    public float timer = 0;
    public float maxTime = 0.1f;
    
    
    
    private void Update()
    {
        if (MonsterManager.Instance.GetMonster(1).state ==  MonsterState.Anomalous)
        {
            timer += Time.deltaTime;
            if (timer >= maxTime)
            {
                if (light.intensity == 200f)
                {
                    light.intensity = 0;
                    maxTime = Random.Range(0.05f, 0.3f);
                }
                else if(light.intensity == 0)
                {
                    light.intensity = 200f;
                    maxTime = Random.Range(0.05f, 0.3f);
                }
                timer = 0;
            }
        }
        else
        {
            light.intensity = 200f;
        }
    }
}
