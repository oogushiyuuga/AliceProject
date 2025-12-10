using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GoalTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameClear();
            }
        }
    }
}