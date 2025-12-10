using UnityEngine;

public class Warp : MonoBehaviour
{
    [Header("ワープの出口")]
    public Transform exitPoint; 

    [Header("ワープ後の設定")]
    public bool resetVelocity = true; //速度リセット

    [Header("制限設定")]
    public bool isOneTimeOnly = false;

    // ワープ直後の多重反応防止用
    private float coolTime = 0.5f;
    private float lastWarpTime = -10f;

    private bool hasUsed = false; //使い切りか記憶するための

    void OnTriggerEnter(Collider other)
    {
        if (isOneTimeOnly && hasUsed) return;

        // プレイヤーかつ、クールタイムが経過していれば実行
        if (other.CompareTag("Player") && Time.time - lastWarpTime > coolTime)
        {
            if (exitPoint != null)
            {
                other.transform.position = exitPoint.position;
                
                other.transform.rotation = exitPoint.rotation;

                // 物理挙動のリセット
                if (resetVelocity)
                {
                    if (other.TryGetComponent<Rigidbody>(out Rigidbody rb))
                    {
                        rb.linearVelocity = Vector3.zero;
                    }
                }

                // クールタイム更新
                lastWarpTime = Time.time;

                if (isOneTimeOnly)
                {
                    hasUsed = true;
                }

                // 音やエフェクトを出すならここ！
                Debug.Log("ワープしました！");
            }
        }
    }
}