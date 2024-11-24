using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArenaTrigger : MonoBehaviour
{

    public GameObject wall;
    public GameObject camera;

    // Start is called before the first frame update
    void Start()
    {
        wall.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            wall.SetActive(true);
            camera.transform.SetParent(null);
            camera.transform.position = new Vector3(698.5f, 21f, -1f);
            Camera.main.orthographicSize = 8;
        }
    }
}
