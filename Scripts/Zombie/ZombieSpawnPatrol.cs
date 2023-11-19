using UnityEngine;

public class ZombieSpawnPatrol : MonoBehaviour
{
    public GameObject[] zombiePrefabs;
    public bool inHouseSpawn = false;
    [HideInInspector]
    public bool canSpawn = true;
    public bool spawnForward;
    public GameObject goingForward;
    public GameObject goingBack;

    [SerializeField] private int zombieSpawnAmount;
    [SerializeField] private float respawnTimer;
    public Transform[] spawnPoints;
    private float resetTimer = 0;

    private void Update()
    {
        if (!canSpawn && !inHouseSpawn)
        {
            resetTimer += 1 * Time.deltaTime;
            if(resetTimer >= respawnTimer)
            {
                canSpawn = true;
                resetTimer = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (spawnForward)
        {
            spawnPoints = goingForward.GetComponent<SpawnDirection>().targetList;
        }
        if (!spawnForward && !inHouseSpawn)
        {
            spawnPoints = goingBack.GetComponent<SpawnDirection>().targetList;
        }

        if (other.CompareTag("Player") && canSpawn && SaveScripts.zombiesInGameAmount < 100 - zombieSpawnAmount)
        {
            SpawnZombies();
            canSpawn = false;
        }
        else if (other.CompareTag("Player") && canSpawn && SaveScripts.zombiesInGameAmount >= 100 - zombieSpawnAmount)
        {

            GameObject[] zombiesToDestroy = GameObject.FindGameObjectsWithTag("zombie");
            for(int i = 0;  i < zombieSpawnAmount; i++)
            {
                if (zombiesToDestroy.Length >= zombieSpawnAmount)
                {
                    float furthestDistance = Vector3.Distance(transform.position, zombiesToDestroy[i].transform.position);
                    if (furthestDistance > 30)
                        Destroy(zombiesToDestroy[i]);
                }
            }
            SpawnZombies();
        }
    }

    private void SpawnZombies()
    {
        for (int i = 0; i < zombieSpawnAmount; i++)
        {
            if (!inHouseSpawn)
            {
                int spawnRandom = Random.Range(0, spawnPoints.Length);
                Instantiate(zombiePrefabs[Random.Range(0, zombiePrefabs.Length)], new Vector3(spawnPoints[spawnRandom].position.x - Random.Range(0,5), spawnPoints[spawnRandom].position.y, spawnPoints[spawnRandom].position.z - Random.Range(0, 5)), spawnPoints[spawnRandom].rotation);
            }
            else
                Instantiate(zombiePrefabs[Random.Range(0, zombiePrefabs.Length)], spawnPoints[i].position, spawnPoints[i].rotation);

            SaveScripts.zombiesInGameAmount++;
        }
    }
}
