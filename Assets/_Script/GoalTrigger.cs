using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GoalTrigger : MonoBehaviour
{

    [SerializeField] private ParticleSystem goalParticle;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameClear();
            }

            if(goalParticle != null)
            {
                if (!goalParticle.isPlaying) goalParticle.Play();
            }
        }
    }
}