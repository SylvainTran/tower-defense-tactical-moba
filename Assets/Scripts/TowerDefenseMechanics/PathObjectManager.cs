using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathObjectManager : MonoBehaviour
{
    public Queue<GameObject> m_ReadyQueue = new Queue<GameObject>();

    [SerializeField] public GameObject m_StartPathNode;

    [SerializeField] private List<GameObject> m_EndPathNodes = new List<GameObject>();

    public Vector3 GetStartingPosition()
    {
        return m_StartPathNode.transform.GetChild(0).transform.position;
    }

    public void EnqueueReady(GameObject pathObject)
    {
        m_ReadyQueue.Enqueue(pathObject);
    }

    public void StartPathing()
    {
        if (m_ReadyQueue.Count <= 0)
        {
            return;
        }

        GameObject pathObject = m_ReadyQueue.Dequeue();

        for (int i = 0; i < m_EndPathNodes.Count; i++)
        {
            pathObject.GetComponent<PathObject>().m_EndPathNodes.Enqueue(m_EndPathNodes[i]);
        }

        // Move translate this particular path object across path until it reaches End Node
        StartCoroutine(pathObject.GetComponent<PathObject>().TravelAlongPath());
    }

    private void FixedUpdate()
    {
        if (m_ReadyQueue.Count > 0 && !LevelScriptManager.Instance.m_PauseSpawning) // maybe should handle this in LevelScriptManager instead
        {
            StartPathing();
        }
    }
}
