using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathObject : MonoBehaviour
{
    private GameObject m_EndPathNode;

    public Queue<GameObject> m_EndPathNodes = new Queue<GameObject>(); // End path nodes, first out is the nearest and the next node to travel to

    private GameObject? m_CurrentEndPathNode;

    [SerializeField] float m_TravelStartDelay = 0.1f;

    public float m_TravelSpeed = 5f; // slowed down during dialogue/encounters with hero projectiles/attacks!

    private bool m_StartTravel = false;

    public delegate void ReachedPlayerEvent();
    public static event ReachedPlayerEvent OnReachedPlayerEvent;

    public IEnumerator TravelAlongPath()
    {
        yield return new WaitForSeconds(m_TravelStartDelay);

        m_StartTravel = true;
    }

    void GetNextEndNode()
    {
        m_CurrentEndPathNode = m_EndPathNodes.Dequeue();
    }

    void FixedUpdate()
    {
        if (m_StartTravel)
        {
            if (m_CurrentEndPathNode == null && m_EndPathNodes.Count > 0)
            {
                GetNextEndNode();
            }

            if (m_CurrentEndPathNode != null)
            {
                Vector3 newPath = (m_CurrentEndPathNode.transform.position - transform.position);

                if (newPath.magnitude > 0.1f)
                {
                    transform.Translate(newPath.normalized * m_TravelSpeed * Time.deltaTime);
                }
                else
                {
                    m_CurrentEndPathNode = null;
                    m_StartTravel = false;

                    if (m_EndPathNodes.Count == 0)
                    {
                        // If no next nodes
                        // Path ended and
                        // Player loses health
                        OnReachedPlayerEvent();
                        StopAllCoroutines();
                        Destroy(gameObject);
                    }
                    else
                    {
                        StartCoroutine(TravelAlongPath());
                    }
                }
            }
        }
    }
}
