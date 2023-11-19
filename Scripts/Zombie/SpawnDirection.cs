using UnityEngine;

public class SpawnDirection : MonoBehaviour
{
    public bool forward = true;
    public bool backward = false;
    public Transform[] targetList;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (forward)
            {
                gameObject.GetComponentInParent<ZombieSpawnPatrol>().spawnForward = true;
            }
            else if(backward)
            {
                gameObject.GetComponentInParent<ZombieSpawnPatrol>().spawnForward = false;
            }
        }
    }
}
