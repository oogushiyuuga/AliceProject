using UnityEngine;

public class FanRound : MonoBehaviour
{
    [SerializeField] float speedParSec = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, speedParSec * Time.deltaTime);
    }
}
