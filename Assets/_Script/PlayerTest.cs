using UnityEngine;
using System.Collections;
using System.Numerics;

public class PlayerMoveTest : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float baseSpeed = 3.0f;
    [SerializeField] private Animator anim;

    private Rigidbody rb;
    private float stuckTimer = 0f;
    private UnityEngine.Vector3 lastPosition;
    
    // 速度倍率
    private float zoneMultiplier = 1.0f;
    private float dashMultiplier = 1.0f;

    // --- ★追加: カメラ制御用の変数 ---
    [Header("Camera Settings")]
    [Tooltip("操作したいカメラ（Main Cameraなど）を入れてください")]
    public Transform viewCamera; 

    [Tooltip("カメラの位置オフセット（プレイヤーからどれくらい離すか）")]
    public UnityEngine.Vector3 camPositionOffset = new UnityEngine.Vector3(0, 2.5f, -4.0f);

    [Tooltip("カメラの角度オフセット（少し下に向けるなど）")]
    public UnityEngine.Vector3 camRotationOffset = new UnityEngine.Vector3(15.0f, 0, 0);

    [Tooltip("回転の遅れ（秒）。0で即追従、数値が大きいほどゆっくりついてくる")]
    [Range(0f, 2f)]
    public float camRotationLag = 0.2f;
    public float camTranslateLag = 0.2f;
    // -------------------------------

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        if (anim == null) anim = GetComponent<Animator>();

        lastPosition = transform.position;

        // ★追加: ゲーム開始時にカメラの位置を初期化（いきなりガタッと動かないように）
        if (viewCamera != null)
        {
            UpdateCameraTransform(true);
        }
    }

    void Update()
    {
        if (anim != null)
        {
            bool isRunning = GameManager.Instance.CurrentState == GameManager.GameState.Action;

            anim.SetBool("IsRun", isRunning);

            if (isRunning)
            {
                anim.speed = zoneMultiplier * dashMultiplier;
            }
            else
            {
                anim.speed = 1.0f;
            }
        }
    }

    void FixedUpdate()
    {
        // 速度計算
        float finalSpeed = baseSpeed * zoneMultiplier * dashMultiplier;

        // 移動処理
        float yVelocity = rb.linearVelocity.y;
        
        if (GameManager.Instance.CurrentState == GameManager.GameState.Action)
        {
            UnityEngine.Vector3 moveVelocity = transform.forward * finalSpeed;
            rb.linearVelocity = new UnityEngine.Vector3(moveVelocity.x, yVelocity, moveVelocity.z);
        }
        else
        {
            rb.linearVelocity = new UnityEngine.Vector3(0, yVelocity, 0);
        }

        // 落下・詰まり判定
        if (transform.position.y < -10f)
        {
            if(GameManager.Instance) GameManager.Instance.TriggerGameOver();
            return;
        }

        if (GameManager.Instance.CurrentState == GameManager.GameState.Action)
        {
            float distanceMoved = UnityEngine.Vector3.Distance(transform.position, lastPosition);
            if (distanceMoved < 0.05f) 
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
        }
        lastPosition = transform.position;
    }

    // ★追加: カメラの追従処理（LateUpdateで行うのが定石）
    void LateUpdate()
    {
        if (viewCamera != null)
        {
            UpdateCameraTransform(false);
        }
    }

    // ★追加: カメラ位置・回転の更新ロジック
    private void UpdateCameraTransform(bool instant)
    {
        // 1. 位置の追従（基本はプレイヤーの位置 + オフセット）
        // ※ プレイヤーの回転に合わせて位置も回したい場合は transform.TransformPoint を使いますが、
        //    今回はシンプルなTPS想定で「ワールド座標でのオフセット加算」または「背後への追従」どちらかになります。
        //    提供されたコードの挙動（Target.position + offset）に合わせるなら以下になります。
        
        // プレイヤーの向きに合わせてカメラ位置を変えたい場合（背後をついてくる）:
        UnityEngine.Vector3 targetPos = transform.position + (transform.rotation * camPositionOffset);
        
        // 単純に座標だけ足す場合（向きが変わってもカメラ位置は固定）:
        // Vector3 targetPos = transform.position + camPositionOffset;

        if(instant || camTranslateLag <= 0f){
            viewCamera.position = targetPos;
        }
        else
        {
            // 時間経過に応じた減衰（Lag）計算
            float t2 = 1f - Mathf.Exp(-Time.deltaTime / camRotationLag);
            viewCamera.position = UnityEngine.Vector3.Lerp(viewCamera.position, targetPos,t2);
        }


        // 2. 回転の追従（遅延あり）
        UnityEngine.Quaternion targetRot = transform.rotation * UnityEngine.Quaternion.Euler(camRotationOffset);

        if (instant || camRotationLag <= 0f)
        {
            viewCamera.rotation = targetRot;
        }
        else
        {
            // 時間経過に応じた減衰（Lag）計算
            float t = 1f - Mathf.Exp(-Time.deltaTime / camRotationLag);
            viewCamera.rotation = UnityEngine.Quaternion.Slerp(viewCamera.rotation, targetRot, t);
        }
    }

    // --- 外部呼び出し用 ---

    public void SetZoneSpeed(float multiplier)
    {
        zoneMultiplier = multiplier;
    }

    public void ApplyDash(float multiplier, float duration)
    {
        StartCoroutine(DashCoroutine(multiplier, duration));
    }

    public void DoJump(float jumpForce)
    {
        UnityEngine.Vector3 velocity = rb.linearVelocity;
        velocity.y = 0;
        rb.linearVelocity = velocity;
        
        rb.AddForce(UnityEngine.Vector3.up * jumpForce, ForceMode.Impulse);

        if (anim != null)
        {
            anim.SetTrigger("Jump");
        }
    }

    private IEnumerator DashCoroutine(float multiplier, float duration)
    {
        dashMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        dashMultiplier = 1.0f;
    }

    public void OnVictory()
    {
        rb.linearVelocity = UnityEngine.Vector3.zero;
        rb.angularVelocity = UnityEngine.Vector3.zero;
        rb.isKinematic = true;

        if (anim != null)
        {
            anim.SetTrigger("Win");
        }
    }
}