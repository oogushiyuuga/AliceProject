using UnityEngine;

public class DashPad : MonoBehaviour
{
    [Header("ダッシュ設定")]
    [SerializeField] private float dashMultiplier = 3.0f;
    [SerializeField] private float duration = 0.5f;

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerMove>(out PlayerMove player))
        {
            player.ApplyDash(dashMultiplier, duration);
        }
    }
}