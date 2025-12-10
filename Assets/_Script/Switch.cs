using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] private GameObject SwitchFloor;
    [SerializeField] private float floorAngle = 180f;
    [SerializeField] private Collider BoxCol;
    [SerializeField] private Collider BoxCol_1;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(SwitchFloor != null)
            {
                BoxCol.transform.Rotate(0, floorAngle, 0); 
                BoxCol_1.transform.Rotate(0, floorAngle, 0); 
            }
        }
    }
}
