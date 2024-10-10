
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
    private SpriteRenderer rend;
    private Animator anim;

    // for ranged enemy
    public GameObject bullet_prefab;
    public Transform firePoint;
    private float fireDelay = 2f;
    private float nextTimeToFire = 0;
    public Transform playerTransform;
    public Vector2 facingDirection = Vector2.right;

    private Vector2 switchX = Vector2.right;
    private Rigidbody2D rbody;

    private float offset;
    private bool frozen = false;

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        Vector2 switchX = Vector2.left;
    }
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // Enemies are put on their own layer so then the raycast can ignore them with ~EnemyMask
        LayerMask EnemyMask = LayerMask.GetMask("enemyLayer");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2f, ~EnemyMask);

        //Debug.DrawRay(transform.position, Vector2.down, Color.red);
        //Debug.Log(hit.collider);
        if (health <= 0)
        {
            //anim.SetTrigger("isDead");
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
        if (!frozen)
            rbody.velocity = switchX * moveSpeed;

        //triggers the walking animation
        if(moveSpeed > 0)
        {
            anim.SetTrigger("isWalking");
            anim.SetBool("isSlicing", false);
        }

        if (enemyType == EnemyType.Melee)
        {
            gameObject.tag = "MeleeEnemy";
            
            RaycastHit2D hitPlayer = Physics2D.Raycast(transform.position, switchX, 2f, ~EnemyMask);
            RaycastHit2D hitWall= Physics2D.Raycast(transform.position, switchX, .8f, ~EnemyMask);

            if (hitPlayer.collider != null && hitPlayer.collider.CompareTag("Player"))
            {

                GameObject EnemyattackCollider = new GameObject("EnemyAttackCollider");
                EnemyattackCollider.gameObject.tag = "EnemyAttack";
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
                anim.SetBool("isSlicing", true);
            } else if(hitWall.collider != null && (hitWall.collider.CompareTag("Ground") || hitWall.collider.CompareTag("RangedEnemy")))
            {
                if (switchX == Vector2.left)
                {
                    //flips the sprite
                    FlipX();

                    switchX = Vector2.right;
                }
                else
                {
                    //flips the sprite
                    FlipX();

                    switchX = Vector2.left;
                }
            }
        }
        if (enemyType == EnemyType.Ranged)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            gameObject.tag = "RangedEnemy";
            //Begins firing when the player is within 20 distance
            if (Vector2.Distance(playerTransform.position, transform.position) < 20f && Time.time > nextTimeToFire)
            {
                if (playerTransform.position.x > transform.position.x)
                {
                    switchX = Vector2.right;
                    facingDirection = Vector2.right;
                    if (transform.localScale.x < 0) // If the sprite is flipped, flip it back
                    {
                        FlipX();
                    }
                }
                else if (playerTransform.position.x < transform.position.x)
                {
                    switchX = Vector2.left;
                    facingDirection = Vector2.left;
                    if (transform.localScale.x > 0) // If the sprite is not flipped, flip it
                    {
                        FlipX();
                    }
                }
                //trigger the shooting anim
                anim.SetTrigger("isShooting");

                //spawns bullet prefab in direction facing
                Instantiate(bullet_prefab, firePoint.position, facingDirection == Vector2.left ? Quaternion.Euler(0, 180, 0) : firePoint.rotation);

                //bullet fire time, DELAY!
                nextTimeToFire = Time.time + fireDelay;
            }

        }
    }
    void Freeze()
    {
        frozen = false;
        rbody.velocity = switchX * 0;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            health -= 25;
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("EnemyAttack") || 
            collider.CompareTag("Ammo") || 
            collider.CompareTag("Health") || 
            collider.CompareTag("Coin") ||
            collider.CompareTag("EnemyBullet"))
        {
            return;
        }
        health -= 20;
    }

    //flips the sprite 
    void FlipX()
    {
        Vector3 theScale = transform.localScale;
        theScale.x = theScale.x * -1;
        transform.localScale = theScale;
    }
}
