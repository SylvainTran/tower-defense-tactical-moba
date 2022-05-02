using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject m_SpawnPrefab;

    [SerializeField] GameObject m_SpawnPathStarter; // Start node of a network path, also notifies/wakes it to start path lerping

    [SerializeField] [Range(1, 10)] float m_SpawnRate;

    [SerializeField] List<GameObject> m_SpawnsProduced = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(SpawnInstance());
    }

    private IEnumerator SpawnInstance()
    {
        yield return new WaitForSeconds(m_SpawnRate);

        PathObjectManager pathObjectManager = m_SpawnPathStarter.GetComponent<PathObjectManager>();
        Vector3 startingPosition = pathObjectManager.GetStartingPosition();
        Transform startPathTransform = pathObjectManager.m_StartPathNode.transform;

        GameObject newSpawn = Instantiate(m_SpawnPrefab, startingPosition, Quaternion.identity);
        m_SpawnsProduced.Add(newSpawn);
        newSpawn.transform.SetParent(startPathTransform);

        Debug.Log("Spawned new!");

        // Notify path starter to start handling newSpawn
        DispatchPathObject(newSpawn);

        StartCoroutine(SpawnInstance());
    }

    private void DispatchPathObject(GameObject pathObject)
    {
        m_SpawnPathStarter.GetComponent<PathObjectManager>().EnqueueReady(pathObject);
    }
}
