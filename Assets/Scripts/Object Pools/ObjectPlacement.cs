using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPlacement : MonoBehaviour
{
    public enum ObjectPlaced {health, ammo, coin}
    public ObjectPlaced objectPlaced;

    public GameObject healthPickupPrefab;
    public GameObject ammoPickupPrefab;
    public GameObject coinPickupPrefab;

    public Vector3 spawnAreaSize = new Vector3(24f, 13f, 2f);
    public List<Health_ObjectPool> healthDataList;
    //public List<Ammo_ObjectPool> ammoDataList;
    //public List<Coin_ObjectPool> coinDataList;


    void Start()
    {
        InvokeRepeating("LaunchCircle", 2.0f, 0.3f);
        if(objectPlaced == ObjectPlaced.health)
        {
            if (Health_ObjectPool.SharedInstance != null)
                healthDataList = Health_ObjectPool.SharedInstance.healthDataList;
            if (healthPickupPrefab != null)
            {
                healthPickupPrefab.transform.position = spawnAreaSize;
                healthPickupPrefab.transform.rotation = Quaternion.identity; // Or set to desired rotation
                healthPickupPrefab.SetActive(true); // Activate the pickup
            }
        }
    }

    void LaunchCircle()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            spawnAreaSize.y,
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );
        /*GameObject bullet = ObjectPool.SharedInstance.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.position = randomPosition;
            bullet.transform.rotation = circle.transform.rotation;
            bullet.SetActive(true);
        }
        */
        // used to bring object back to the pool so they can be used again
        //bullet.SetActive(false);

    }
}
