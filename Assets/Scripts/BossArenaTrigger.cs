using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArenaTrigger : MonoBehaviour
{

    public GameObject wall;
    public GameObject camera;

    Vector3 cameraPosition;
    public Transform cameraTransform;

    void Start()
    {
        wall.SetActive(false);
        cameraPosition.Set(cameraTransform.position.x, cameraTransform.position.y, cameraTransform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            wall.SetActive(true);
            camera.transform.SetParent(null);
            camera.transform.position = cameraPosition;
            Camera.main.orthographicSize = 8;
        }
    }
}
