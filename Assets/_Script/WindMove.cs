using UnityEngine;

public class WindMove : MonoBehaviour
{
    [Header("風の力")]
    [SerializeField] private float windX = 0f;
    [SerializeField] private float windY = 0f;
    [SerializeField] private float windZ = 0f;

    [Header("初期状態")]
    [SerializeField] private bool isWindActive = true; 

    [Header("エフェクト")]
    [SerializeField] private ParticleSystem windParticle;
    [SerializeField] private FanRound fanScript;

    void Start()
    {
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

    // スイッチから呼ぶ関数
    public void ToggleWind()
    {
        isWindActive = !isWindActive;
        UpdateEffect();
    }

    // 強制指定用
    public void SetWind(bool active)
    {
        isWindActive = active;
        UpdateEffect();
    }

    // パーティクルを再生/停止する
    private void UpdateEffect()
    {
        if (windParticle != null)
        {
            if (isWindActive)
            {
                if (!windParticle.isPlaying) windParticle.Play();
            }
            else
            {
                windParticle.Stop();
            }
        }

        if (fanScript != null)
        {
            fanScript.SetRunning(isWindActive);
        }
    }
}