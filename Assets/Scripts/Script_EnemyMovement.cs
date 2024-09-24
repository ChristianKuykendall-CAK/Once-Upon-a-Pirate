using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Script_EnemyMovement : MonoBehaviour
{
    public enum EnemyType { Melee, Ranged }
    public EnemyType enemyType;

    public Vector2 facingDirection = Vector2.right;
    public float moveForce;
    public float moveSpeed;

    // for ranged enemy
    public GameObject bullet_prefab;

    private Vector2 switchX = Vector2.left;
    private Rigidbody2D rbody;

    private bool attacking = false;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        Vector2 switchX = Vector2.left;
    }


    private void FixedUpdate()
    {
        // Enemies are put on their own layer so then the raycast can ignore them with ~EnemyMask
        LayerMask EnemyMask = LayerMask.GetMask("enemyLayer");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, ~EnemyMask);

        //Debug.DrawRay(transform.position, Vector2.down, Color.red);
        //Debug.Log(hit.collider);
        
        if (hit.collider == null && rbody.velocity.y == 0)
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
        if(!attacking)
            rbody.velocity = switchX * moveSpeed;

        if(enemyType == EnemyType.Melee)
        {
            // raycast is set in front of enemy
            RaycastHit2D hitPlayer = Physics2D.Raycast(transform.position, switchX, 2f, ~EnemyMask);
            if(hitPlayer.collider != null && hitPlayer.collider.CompareTag("Player"))
            {
                // helps toggle the enemy to start and stop moving when attacking
                attacking = true;
                // add new gameobject and the make a boxcollider2D component for that gameobject
                GameObject attackCollider = new GameObject("AttackCollider");
                BoxCollider2D boxCollider = attackCollider.AddComponent<BoxCollider2D>();

                // set collider to a trigger for player to get damaged by
                boxCollider.isTrigger = true;
                // stops enemy before attack
                rbody.velocity = Vector2.zero;

                // creates collider
                attackCollider.transform.position = transform.position + new Vector3(switchX.x, switchX.y, 0);
                boxCollider.size = new Vector2(.5f, .5f);
                void OnTriggerEnter2D(BoxCollider2D other)
                {
                    if (other.CompareTag("Player"))
                    {
                        Rigidbody2D rbodyPlayer = other.GetComponent<Rigidbody2D>();
                        rbodyPlayer.AddForce(switchX * 10);
                        // health = health - 25;
                        // Player.rbody.Addforce : pull from PlayerController class?
                    }
                }
                
                // times collider
                Destroy(attackCollider, 0.5f);
                Invoke("AttackPlayer", 1f);
            }
        }
    }
    void AttackPlayer()
    {
        attacking = false;
    }
}
