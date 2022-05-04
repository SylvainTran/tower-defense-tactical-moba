using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFactory : MonoBehaviour
{
    public static TurretFactory Instance { get; private set; }
    public GameObject TurretPrefab;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }

    public void Create(GameObject gameObject)
    {
        print("Creating a turret!");
        Instantiate(TurretPrefab, gameObject.transform.position, Quaternion.identity);
    }
    public void Create(Vector3 position)
    {
        print("Creating a turret!");
        Instantiate(TurretPrefab, position, Quaternion.identity);
    }
}
