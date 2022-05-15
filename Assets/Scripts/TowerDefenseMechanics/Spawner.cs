using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject m_MinionPrefab;

    [SerializeField] GameObject m_ChallengerPrefab;

    [SerializeField] GameObject m_SpawnPathStarter; // Start node of a network path, also notifies/wakes it to start path lerping

    [SerializeField] [Range(1, 10)] float m_MinionSpawnRate;    
    [SerializeField] [Range(1, 10)] float m_ChallengerSpawnRate;

    //[SerializeField] List<GameObject> m_SpawnsProduced = new List<GameObject>();
    private PathObjectManager m_pathObjectManager;
    private Vector3 startingPosition;
    private Transform startPathTransform;
    
    public bool m_SpawnerIsPaused = false;

    private void Awake()
    {
        m_pathObjectManager = m_SpawnPathStarter.GetComponent<PathObjectManager>();
        startingPosition = m_pathObjectManager.GetStartingPosition();
        startPathTransform = m_pathObjectManager.m_StartPathNode.transform;
    }

    public void InstantiateSpawnPrefab(GameObject spawnPrefab, Vector3 startingPosition, Transform parent)
    {
        GameObject newSpawn = Instantiate(spawnPrefab, startingPosition, Quaternion.identity);
        TowerDefenseManager.Instance.m_EnemiesAlive.Add(newSpawn);
        newSpawn.transform.SetParent(parent);

        // Notify path starter to start handling newSpawn
        DispatchPathObject(newSpawn);
    }
    
    private IEnumerator SpawnMinion()
    {
        yield return new WaitForSeconds(m_MinionSpawnRate);

        InstantiateSpawnPrefab(m_MinionPrefab, startingPosition, startPathTransform);
    }

    private IEnumerator SpawnChallenger()
    {
        yield return new WaitForSeconds(m_ChallengerSpawnRate);

        InstantiateSpawnPrefab(m_ChallengerPrefab, startingPosition, startPathTransform);
    }

    public void StartSpawning()
    {
        m_SpawnerIsPaused = false;
        StartCoroutine((SpawnMinion()));
        float threshold = 33.3f;
        UnityEngine.Random.InitState((int) DateTime.Now.Ticks);
        // Uses XOR shift algorithm
        if (UnityEngine.Random.Range(0, 100) < threshold)
        {
            StartCoroutine(SpawnChallenger());
        }
    }

    private void DispatchPathObject(GameObject pathObject)
    {
        m_SpawnPathStarter.GetComponent<PathObjectManager>().EnqueueReady(pathObject);
    }
}
