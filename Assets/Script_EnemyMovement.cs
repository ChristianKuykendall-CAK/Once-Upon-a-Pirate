using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_EnemyMovement : MonoBehaviour
{
    public Vector2 facingDirection = Vector2.right;
    public float moveForce;
    private Rigidbody2D rbody;
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rbody.AddForce(Vector2.left * moveForce);
    }
}
