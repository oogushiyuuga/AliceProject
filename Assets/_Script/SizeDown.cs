using UnityEngine;

public class SizeDown : MonoBehaviour
{
    [Header("エフェクト設定")]
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private float duration = 2.0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerMove>(out PlayerMove player))
        {
            player.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            if (effectPrefab != null)
            {
                //AudioManager.Instance.PlaySE("SizeChange");
                Vector3 spawnPos = player.transform.position + Vector3.up * 1.0f;
                GameObject effectObj = Instantiate(effectPrefab, spawnPos, Quaternion.identity);

                effectObj.transform.SetParent(player.transform);

                Destroy(effectObj, duration);
            }
        }
    }
}