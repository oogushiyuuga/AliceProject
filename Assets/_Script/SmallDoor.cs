using UnityEngine;

public class SmallDoorSensor : MonoBehaviour
{
    [SerializeField] private float sizeThreshold = 0.8f;
    [SerializeField] private GameObject targetDoor; // 消したいドア（壁）を指定

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 小さい時だけ
            if (other.transform.localScale.x < sizeThreshold)
            {
                // 指定したドアを消す
                if (targetDoor != null)
                {
                    targetDoor.SetActive(false);
                }
            }
        }
    }
}