using UnityEngine;

public class JumpAction : MonoBehaviour
{

    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerMove>(out PlayerMove player))
        {
            anim.SetTrigger("Jump");
        }
    }

}
