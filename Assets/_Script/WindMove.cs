using UnityEngine;

public class WindMove : MonoBehaviour
{
    [Header("風の力")]
    [SerializeField] private float windX = 0f;
    [SerializeField] private float windY = 0f;
    [SerializeField] private float windZ = 0f;

    [Header("初期状態")]
    [SerializeField] private bool isWindActive = true;

    [Header("見た目")]
    [SerializeField] private GameObject windEffect; // 風のエフェクトやパーティクル

    void Start()
    {
        // ゲーム開始時にエフェクトの状態を合わせる
        UpdateEffect();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isWindActive) return;

        Rigidbody otherRigidbody = other.gameObject.GetComponent<Rigidbody>();

        if (otherRigidbody != null)
        {
            otherRigidbody.AddForce(windX, windY, windZ, ForceMode.Acceleration);
        }
    }

    // スイッチから呼ぶ関数：切り替え
    public void ToggleWind()
    {
        isWindActive = !isWindActive;
        UpdateEffect();
        Debug.Log("風の状態: " + (isWindActive ? "ON" : "OFF"));
    }

    // スイッチから呼ぶ関数：強制指定
    public void SetWind(bool active)
    {
        isWindActive = active;
        UpdateEffect();
    }

    // エフェクトの表示/非表示を更新
    private void UpdateEffect()
    {
        if (windEffect != null)
        {
            windEffect.SetActive(isWindActive);
        }
    }
}