using UnityEngine;

public class DashPad : MonoBehaviour
{
     [Header("エフェクト設定")]
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private float effectDuration = 2.0f;

    [Header("ダッシュ設定")]
    [SerializeField] private float dashMultiplier = 3.0f;
    [SerializeField] private float duration = 0.5f;

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerMove>(out PlayerMove player))
        {
            player.ApplyDash(dashMultiplier, duration);
        }

        if (effectPrefab != null)
            {
                AudioManager.Instance.PlaySE("SpeedUp");
                Vector3 spawnPos = player.transform.position + Vector3.up * 1.0f;
                GameObject effectObj = Instantiate(effectPrefab, spawnPos, Quaternion.identity);

                effectObj.transform.SetParent(player.transform);

                Destroy(effectObj, effectDuration);
            }
    }
}