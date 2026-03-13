using UnityEngine;

public class HitDetection : MonoBehaviour
{
    [SerializeField] private float sizeThreshold = 1.3f;
    [SerializeField] private GameObject targetObject; // 消したいオブジェクトを指定

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 大きい時だけ
            if (other.transform.localScale.x > sizeThreshold)
            {
                // 指定した木箱を消す
                if (targetObject != null)
                {
                    AudioManager.Instance.PlaySE("BreakBox");
                    targetObject.SetActive(false);
                }
            }
        }
    }
}