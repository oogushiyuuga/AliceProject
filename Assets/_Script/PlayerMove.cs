using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 3.0f;
    [SerializeField] private Animator anim;

    private Rigidbody rb;
    private float stuckTimer = 0f;
    private Vector3 lastPosition;
    
    // 速度倍率
    private float zoneMultiplier = 1.0f;
    private float dashMultiplier = 1.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        if (anim == null) anim = GetComponent<Animator>();

        lastPosition = transform.position;
    }

    void Update()
    {
        if (anim != null)
        {
            bool isRunning = GameManager.Instance.CurrentState == GameManager.GameState.Action;

            anim.SetBool("IsRun", isRunning);

            if (isRunning)
            {
                // 「エリア倍率 × ダッシュ倍率」をアニメ速度に反映
                anim.speed = zoneMultiplier * dashMultiplier;
            }
            else
            {
                // ゴールした時や待機中は、ポーズなどを正常な速さで再生するために1に戻す
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
        
        // GameManagerがActionフェーズの時だけ前に進むように
        if (GameManager.Instance.CurrentState == GameManager.GameState.Action)
        {
            Vector3 moveVelocity = transform.forward * finalSpeed;
            rb.linearVelocity = new Vector3(moveVelocity.x, yVelocity, moveVelocity.z);
        }
        else
        {
            // 待機中は物理的な横移動を止める（重力はそのまま）
            rb.linearVelocity = new Vector3(0, yVelocity, 0);
        }

        // 落下・詰まり判定
        if (transform.position.y < -10f)
        {
            if(GameManager.Instance) GameManager.Instance.TriggerGameOver();
            return;
        }

        // 設置フェーズ中は詰まり判定をしない
        if (GameManager.Instance.CurrentState == GameManager.GameState.Action)
        {
            float distanceMoved = Vector3.Distance(transform.position, lastPosition);
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
        // 物理挙動：一度Y速度をリセットして跳ねさせる
        Vector3 velocity = rb.linearVelocity;
        velocity.y = 0;
        rb.linearVelocity = velocity;
        
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

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
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        if (anim != null)
        {
            anim.SetTrigger("Win");
        }
    }
}