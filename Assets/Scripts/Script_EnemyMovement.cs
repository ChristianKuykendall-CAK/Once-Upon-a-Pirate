using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Script_EnemyMovement : MonoBehaviour
{
    public Vector2 facingDirection = Vector2.right;
    public float moveForce;
    private Rigidbody2D rbody;
    public GameObject bullet_prefab;
    private Vector2 switchX = Vector2.left;
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
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);
        if (hit.collider != GameObject.FindWithTag("Ground"))
        {
            rbody.AddForce(switchX * moveForce);
        }
        else
        {
            if (switchX == Vector2.left)
            {
                switchX = Vector2.right;
            }
            else
            {
                switchX = Vector2.left;
            }
        }
    }
}
