using UnityEngine;

public class ClearPlayer : MonoBehaviour
{
    float speed = 3.0f;
    Rigidbody rb;

    void FixedUpdate()
    {
        // 移動処理
        float yVelocity = rb.linearVelocity.y;
        
        Vector3 moveVelocity = transform.forward * speed;
        rb.linearVelocity = new Vector3(moveVelocity.x, yVelocity, moveVelocity.z);
    }
        
}
