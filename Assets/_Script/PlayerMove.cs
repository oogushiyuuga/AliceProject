using UnityEngine;
using System.Collections; // コルーチンに必要

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 5.0f; // 基本速度
    private Rigidbody rb;

    // 詰まり判定用
    private float stuckTimer = 0f;
    private Vector3 lastPosition;

    // ★追加: 速度倍率の管理
    private float zoneMultiplier = 1.0f; // 減速フィールドなどの倍率
    private float dashMultiplier = 1.0f; // ダッシュボードの倍率

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        // 1. 現在の最終的な速度を計算 (基本 × エリア補正 × ダッシュ補正)
        float finalSpeed = baseSpeed * zoneMultiplier * dashMultiplier;

        // 2. 移動処理
        float yVelocity = rb.linearVelocity.y;
        Vector3 moveVelocity = transform.right * finalSpeed; // finalSpeedを使う
        rb.linearVelocity = new Vector3(moveVelocity.x, yVelocity, moveVelocity.z);

        // --- 以下、既存の判定ロジック ---
        if (transform.position.y < -10f)
        {
            if(GameManager.Instance) GameManager.Instance.TriggerGameOver();
            return;
        }

        float distanceMoved = Vector3.Distance(transform.position, lastPosition);
        if (distanceMoved < 0.01f) 
        {
            stuckTimer += Time.fixedDeltaTime;
            if (stuckTimer > 2.0f)
            {
                if(GameManager.Instance) GameManager.Instance.TriggerGameOver();
            }
        }
        else
        {
            stuckTimer = 0f;
        }
        lastPosition = transform.position;
    }

    // 外部から呼ぶ関数：ゾーン（乗っている間ずっと）
    public void SetZoneSpeed(float multiplier)
    {
        zoneMultiplier = multiplier;
    }

    // 外部から呼ぶ関数：ダッシュ（一瞬だけ）
    public void ApplyDash(float multiplier, float duration)
    {
        // 既にダッシュ中なら上書きするためにコルーチンを一度止める処理を入れる
        StartCoroutine(DashCoroutine(multiplier, duration));
    }

    private IEnumerator DashCoroutine(float multiplier, float duration)
    {
        dashMultiplier = multiplier;
        yield return new WaitForSeconds(duration); // 指定時間待つ
        dashMultiplier = 1.0f; // 元に戻す
    }
}