using UnityEngine;

public class SpeedZone : MonoBehaviour
{
    [Header("速度倍率 (0.5なら半分の遅さ)")]
    [SerializeField] private float multiplier = 0.5f;

    // 乗った時
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerMove>(out PlayerMove player))
        {
            player.SetZoneSpeed(multiplier);
        }
    }

    // 出た時
    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerMove>(out PlayerMove player))
        {
            player.SetZoneSpeed(1.0f); // 元に戻す
        }
    }
}