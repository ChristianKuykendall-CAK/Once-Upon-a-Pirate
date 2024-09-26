using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Script_EnemyMovement : MonoBehaviour
{
    public enum EnemyType { Melee, Ranged }
    public EnemyType enemyType;

    public float moveForce;
    public float moveSpeed;

    private int health = 100;

    // for ranged enemy
    public GameObject bullet_prefab;

    private Vector2 switchX = Vector2.left;
    private Rigidbody2D rbody;

    private float offset;
    private bool frozen = false;

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
        if(health <= 0)
        {
            Destroy(gameObject);
        }
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
        if(!frozen)
            rbody.velocity = switchX * moveSpeed;

        if(enemyType == EnemyType.Melee)
        {
            RaycastHit2D hitPlayer = Physics2D.Raycast(transform.position, switchX, 2f, ~EnemyMask);
            if(hitPlayer.collider != null && hitPlayer.collider.CompareTag("Player"))
            {
                GameObject EnemyattackCollider = new GameObject("EnemyAttackCollider");

                BoxCollider2D boxCollider = EnemyattackCollider.AddComponent<BoxCollider2D>();
                boxCollider.isTrigger = true;
                if (switchX == Vector2.left)
                    offset = -.25f;
                else if (switchX == Vector2.right)
                    offset = .25f;

                Invoke("Freeze", 2f);
                frozen = true;

                EnemyattackCollider.transform.position = transform.position + new Vector3(switchX.x + offset, switchX.y, 0);
                
                boxCollider.size = new Vector2(.5f, .5f);

                Destroy(EnemyattackCollider, 0.5f);
            }
        }
        if(enemyType == EnemyType.Ranged)
        {

        }
    }
    void Freeze()
    {
        frozen = false;
        rbody.velocity = switchX * 0;
    }
    
}
