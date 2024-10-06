
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Controller : MonoBehaviour
{
    private int speed = 16;
    private Vector2 direction = Vector2.right;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destroy", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
    void Destroy()
    {
        if (gameObject != null)
            Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Script_EnemyMovement>();
            // health -= 25;
            Destroy(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
