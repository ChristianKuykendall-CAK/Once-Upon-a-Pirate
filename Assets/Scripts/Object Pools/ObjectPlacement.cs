using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    public enum ObjectPlaced { health, ammo, coin }
    public ObjectPlaced objectPlaced;

    public GameObject healthPickupPrefab; // This can be used for reference only
    public GameObject ammoPickupPrefab;   // You can define this as well
    public GameObject coinPickupPrefab;    // You can define this as well

    public Vector3 spawnAreaSize = new Vector3(24f, 13f, 2f);
    public List<Health_ObjectPool> healthDataList;

    void Start()
    {
        InvokeRepeating("LaunchCircle", 2.0f, 0.3f);
    }

    void LaunchCircle()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            spawnAreaSize.y,
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );

        if (objectPlaced == ObjectPlaced.health)
        {
            // Check if the pool has been initialized
            if (Health_ObjectPool.SharedInstance != null)
            {
                // Get a pooled object from the health pool
                GameObject healthPickup = Health_ObjectPool.SharedInstance.GetPooledObject();

                if (healthPickup != null)
                {
                    // Set the position and rotation for the object
                    healthPickup.transform.position = randomPosition;
                    healthPickup.transform.rotation = Quaternion.identity; // Or set to desired rotation
                    healthPickup.SetActive(true); // Activate the pickup
                }
            }
        }
        // Similar logic can be applied for ammo and coin objects if needed
    }
}
