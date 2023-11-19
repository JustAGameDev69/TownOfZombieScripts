using UnityEngine;

public class InHouseSpawnTrigger : MonoBehaviour
{
    public GameObject spawnZone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spawnZone.GetComponent<ZombieSpawnPatrol>().canSpawn = true;
        }
    }
}
