using UnityEngine;
using System.Collections;
using System.Numerics;

public class PlayerMove : MonoBehaviour
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

    [Header("Camera Settings")]
    public Transform viewCamera; 

    public UnityEngine.Vector3 camPositionOffset = new UnityEngine.Vector3(0, 1.5f, -3.0f); 
    public UnityEngine.Vector3 camRotationOffset = new UnityEngine.Vector3(20f, 0, 0);

    public float camPositionLag = 0.2f; //回転した時のカメラ位置の遅れ具合
    public float camRotationLag = 0.2f; //回転した時のカメラ角度の遅れ具合

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        if (anim == null) anim = GetComponent<Animator>();

        lastPosition = transform.position;

        // ゲーム開始時にカメラの位置を初期化
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

    // カメラの追従処理
    void LateUpdate()
    {
        if (viewCamera != null)
        {
            UpdateCameraTransform(false);
        }
    }

    // カメラ位置・回転の更新ロジック
    private void UpdateCameraTransform(bool instant) //　アピールポイント：曲がった時にカクカクしないように先生と相談して完成させた。こだわったポイント。
    {
        // プレイヤーの向きに合わせてカメラ位置を変える
        UnityEngine.Vector3 targetPos = transform.position + (transform.rotation * camPositionOffset);

        if(instant || camPositionLag <= 0f){
            viewCamera.position = targetPos;
        }
        else
        {
            // 時間経過に応じた減衰（Lag）計算
            float t2 = 1f - Mathf.Exp(-Time.deltaTime / camRotationLag);
            viewCamera.position = UnityEngine.Vector3.Lerp(viewCamera.position, targetPos,t2);
        }


        // 回転の追従（遅延あり）
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

    // 外部呼び出し用

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