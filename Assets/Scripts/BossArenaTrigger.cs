using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This code turns on and off the walls for the boss arenas

public class BossArenaTrigger : MonoBehaviour
{

    public GameObject wall;
    public GameObject bosswall;
    public GameObject camera;

    private Rigidbody2D rbofy;

    public int cameraOrthographic;

    Vector3 cameraPosition;
    public Transform cameraTransform;

    void Start()
    {
        wall.SetActive(false);
        bosswall.SetActive(true);
        cameraPosition.Set(cameraTransform.position.x, cameraTransform.position.y, cameraTransform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            wall.SetActive(true);
            bosswall.SetActive(false);
            camera.transform.SetParent(null);
            camera.transform.position = cameraPosition;
            Camera.main.orthographicSize = cameraOrthographic;
        }
    }
}
