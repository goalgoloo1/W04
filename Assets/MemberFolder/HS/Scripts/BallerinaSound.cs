using UnityEngine;

public class BallerinaSound : MonoBehaviour
{
    [SerializeField] private AudioSource ballerinaAudioSource;

    private void OnEnable()
    {
        if (MonsterManager.Instance.GetMonster(7).state == MonsterState.Common)
        {
            SoundManager.Instance.PlayAudio(ballerinaAudioSource);
        }
    }
}
