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
    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        Vector2 switchX = Vector2.left;
    }


    private void FixedUpdate()
    {
        LayerMask EnemyMask = LayerMask.GetMask("enemyLayer");
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, ~EnemyMask);
        Debug.DrawRay(transform.position, Vector2.down, Color.red);
        Debug.Log(hit.collider);
        
        if (hit.collider == null)
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
        rbody.velocity = switchX * moveSpeed;
    }
}
