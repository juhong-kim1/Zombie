using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs;
    public Transform spawnPoint;
    public float spawnRange = 20;
    public int itemSpawnNumber = 2;
    public int itemCount = 0;
    public int maxItemCount = 10;
    public float itemSpawnTime = 0f;
    public float itemSpawnDelay = 10f;

    public UIManager manager;

    //private List<GameObject> items = new List<GameObject>();

    private void Awake()
    {
        SpawnItems();
    }

    private void Update()
    {
        itemSpawnTime += Time.deltaTime;

        if (itemSpawnTime > itemSpawnDelay && itemCount < maxItemCount)
        {
            SpawnItems();

            itemSpawnTime = 0f;
        }



    }

    private void SpawnItems()
    {
        Vector3 itemPosition = spawnPoint.position;

        itemPosition.y += 0.5f;

        for (int i = 0; i < itemSpawnNumber; i++)
        {
            if (RandomPoint(spawnPoint.position, spawnRange, out Vector3 spawnPos))
            {
                GameObject randomItem = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

                Instantiate(randomItem, itemPosition, Quaternion.identity);

                ++itemCount;
            }
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit drop;
            if (NavMesh.SamplePosition(randomPoint, out drop, 1.0f, NavMesh.AllAreas))
            {
                result = drop.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}
