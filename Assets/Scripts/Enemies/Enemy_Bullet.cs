
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{

    private int speed = 8;
    private Vector2 direction = Vector2.right;

    // Start is called before the first frame update
    void Start()
    {
        //Despawn bullet after it travels a certain distance
        Invoke("Die", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void Die()
    {
        if (gameObject != null)
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.health -= 20;
            Destroy(gameObject);
        }
    }
}
