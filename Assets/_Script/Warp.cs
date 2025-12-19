using UnityEngine;
using Unity.Cinemachine;

public class Warp : MonoBehaviour
{
    [Header("ワープの出口")]
    public Transform exitPoint; 

    [Header("ワープ後の設定")]
    public bool resetVelocity = true;
    public bool isOneTimeOnly = false;

    [Header("演出設定")]
    [SerializeField] private GameObject warpEffectPrefab; // ワープ瞬間のエフェクト
    [SerializeField] private float shakeForce = 1.0f;     // 揺れの強さ

    private float coolTime = 0.5f;
    private float lastWarpTime = -10f;
    private bool hasUsed = false;
    
    private CinemachineImpulseSource impulseSource;

    void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (isOneTimeOnly && hasUsed) return;

        if (other.CompareTag("Player") && Time.time - lastWarpTime > coolTime)
        {
            if (exitPoint != null)
            {
                // 演出実行（ワープ移動の前に行う）
                PlayEffects(other.transform.position);

                // 移動処理
                other.transform.position = exitPoint.position;

                if (resetVelocity)
                {
                    if (other.TryGetComponent<Rigidbody>(out Rigidbody rb))
                    {
                        rb.linearVelocity = Vector3.zero;
                    }
                }

                lastWarpTime = Time.time;
                if (isOneTimeOnly) hasUsed = true;

                Debug.Log("ワープしました！");
            }
        }
    }

    // 演出をまとめた関数
    private void PlayEffects(Vector3 position)
    {
        AudioManager.Instance.PlaySE("Warp");
        // 画面シェイク
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulseWithForce(shakeForce);
        }

        // エフェクト生成
        if (warpEffectPrefab != null)
        {
            Instantiate(warpEffectPrefab, position, Quaternion.identity);
        }
    }
}