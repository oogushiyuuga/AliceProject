using UnityEngine;

public class FanRound : MonoBehaviour
{
    [SerializeField] float speedParSec = 1f;
    
    private bool isRunning = true;

    void Update()
    {
        if (isRunning)
        {
            transform.Rotate(0, 0, speedParSec * Time.deltaTime);
        }
    }
    
    public void SetRunning(bool active)
    {
        isRunning = active;
    }
}