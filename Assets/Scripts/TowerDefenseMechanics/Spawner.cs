using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject m_SpawnPrefab;

    [SerializeField] GameObject m_SpawnPathStarter; // Start node of a network path, also notifies/wakes it to start path lerping

    [SerializeField] [Range(1, 10)] float m_SpawnRate;

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

    private IEnumerator SpawnInstance()
    {
        yield return new WaitForSeconds(m_SpawnRate);

        GameObject newSpawn = Instantiate(m_SpawnPrefab, startingPosition, Quaternion.identity);
        TowerDefenseManager.Instance.m_EnemiesAlive.Add(newSpawn);
        newSpawn.transform.SetParent(startPathTransform);

        // Notify path starter to start handling newSpawn
        DispatchPathObject(newSpawn);
    }

    public void StartSpawning()
    {
        m_SpawnerIsPaused = false;
        StartCoroutine(SpawnInstance());
    }

    private void DispatchPathObject(GameObject pathObject)
    {
        m_SpawnPathStarter.GetComponent<PathObjectManager>().EnqueueReady(pathObject);
    }
}
