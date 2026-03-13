using UnityEngine;

public class WindMove : MonoBehaviour
{
    [Header("風の力")]
    [SerializeField] private float windX = 0f;
    [SerializeField] private float windY = 0f;
    [SerializeField] private float windZ = 0f;

    [Header("初期状態")]
    [SerializeField] private bool startWithNormalDirection = true; 

    [Header("見た目の移動設定")]
    [SerializeField] private Transform visualObject; //親オブジェクト
    [SerializeField] private Transform normalMarker; //正方向の時の場所と向き(空オブジェクトを入れる)
    [SerializeField] private Transform reverseMarker;  //逆方向の時の場所と向き(ゞ)

    [Header("エフェクト制御")]
    [SerializeField] private ParticleSystem windParticle;
    [SerializeField] private FanRound fanScript;

    private float directionMultiplier = 1.0f; // 風の向き倍率 (1 = 通常, -1 = 逆)

    void Start()
    {
        // 初期状態の設定
        directionMultiplier = startWithNormalDirection ? 1.0f : -1.0f;
        UpdateEffect();
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody otherRigidbody = other.gameObject.GetComponent<Rigidbody>();
        if (otherRigidbody != null)
        {
            // 設定値 × 向き倍率 で力を加える
            otherRigidbody.AddForce(
                windX * directionMultiplier,
                windY * directionMultiplier,
                windZ * directionMultiplier,
                ForceMode.Acceleration
            );
        }
    }

    // スイッチから呼ぶ関数
    public void ToggleWind()
    {
        // 1 と -1 を入れ替える
        directionMultiplier *= -1.0f;
        UpdateEffect();
    }

    // 強制指定用
    public void SetWindDirection(bool isNormal)
    {
        directionMultiplier = isNormal ? 1.0f : -1.0f;
        UpdateEffect();
    }

    // 見た目（位置・回転・パーティクル）を更新
    private void UpdateEffect()
    {
        //場所と向きの移動
        if (visualObject != null && normalMarker != null && reverseMarker != null)
        {
            // 現在の向きに合わせてターゲット（マーカー）を決める
            Transform targetMarker = (directionMultiplier > 0f) ? normalMarker : reverseMarker;

            // 扇風機をそのマーカーの位置と回転にワープさせる
            visualObject.position = targetMarker.position;
            visualObject.rotation = targetMarker.rotation;
        }

        //エフェクト制御
        if (windParticle != null)
        {
            if (!windParticle.isPlaying) windParticle.Play();
        }

        if (fanScript != null)
        {
            fanScript.SetRunning(true);
        }
    }
}