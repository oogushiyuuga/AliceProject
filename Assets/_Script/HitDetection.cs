using UnityEngine;

public class HitDetection : MonoBehaviour
{
    [SerializeField] private float sizeThreshold = 1.3f;
    [SerializeField] private GameObject targetDoor; // 消したいドア（壁）を指定

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 大きい時だけ
            if (other.transform.localScale.x > sizeThreshold)
            {
                // 指定した木箱を消す
                if (targetDoor != null)
                {
                    AudioManager.Instance.PlaySE("BreakBox");
                    targetDoor.SetActive(false);
                }
            }
        }
    }
}