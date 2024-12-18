
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
    private Rigidbody2D rbody;
    private Collider2D coll;

    // for ranged enemy
    public GameObject bullet_prefab;
    public Transform firePoint;
    private float fireDelay = 2f;
    private float nextTimeToFire = 0;
    public Transform playerTransform;
    public Vector2 facingDirection = Vector2.right;

    private Vector2 switchX = Vector2.right;
    

    private float offset;
    private bool frozen = false;
    private bool isDead = false;
    private bool isSlicing = false;

    //Audio
    private AudioSource Audio;

    public AudioClip swordAttack;
    public AudioClip rangedAttack;
    public AudioClip deathSound;

    public bool isPlayerDead()
    { return isDead; }

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        Audio = GetComponent<AudioSource>();
        Vector2 switchX = Vector2.left;

        if (enemyType == EnemyType.Melee)
        {
            gameObject.tag = "MeleeEnemy";
        } else if (enemyType == EnemyType.Ranged)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            gameObject.tag = "RangedEnemy";
        }
    }

    private void FixedUpdate()
    {
        Vector3 lowerPosition = new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z);
        // Enemies are put on their own layer so then the raycast can ignore them with ~EnemyMask
        LayerMask EnemyMask = LayerMask.GetMask("enemyLayer");
        RaycastHit2D hit = Physics2D.Raycast(lowerPosition, Vector2.down, .6f, ~EnemyMask);
        Debug.DrawRay(lowerPosition, Vector2.down, Color.green, .6f);

        //Debug.DrawRay(transform.position, Vector2.down, Color.red);
        //Debug.Log(hit.collider);


        //enemy die
        if (health <= 0 && !isDead) 
        {
            isDead = true;

            moveSpeed = 0;
            moveForce = 0;

            if(enemyType == EnemyType.Melee)
            {
                anim.ResetTrigger("isWalking");
            }

            Audio.PlayOneShot(deathSound);
            
            frozen = true;
            Freeze();

            anim.SetTrigger("isDead");
            Invoke("Die", 2f);
        }


        if (hit.collider == null && rbody.velocity.y == 0)
        {
            if (switchX == Vector2.left)
            {
                FlipX();
                switchX = Vector2.right;
            }
            else
            {
                FlipX();
                switchX = Vector2.left;
            }
        }
        if (!frozen)
            rbody.velocity = switchX * moveSpeed;

        //triggers the walking animation
        if(moveSpeed > 0 && isSlicing == false)
        {
            anim.SetTrigger("isWalking");
            //anim.SetBool("isSlicing", false);
        }

        if (enemyType == EnemyType.Melee)
        {

            RaycastHit2D hitPlayer = Physics2D.Raycast(transform.position, switchX, 2f, ~EnemyMask);
            RaycastHit2D hitWall = Physics2D.Raycast(lowerPosition, switchX, 1.5f, ~EnemyMask);

            if (!isDead)
            {
                if (hitWall.collider != null && (hitWall.collider.CompareTag("Ground") || hitWall.collider.CompareTag("RangedEnemy")))
                {
                    //Debug.Log(hitWall.collider);
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
                else if (hitPlayer.collider != null && hitPlayer.collider.CompareTag("Player"))
                {
                    // creates an invisible collider that damages the player
                    GameObject EnemyattackCollider = new GameObject("EnemyAttackCollider");
                    EnemyattackCollider.gameObject.tag = "EnemyAttack";
                    BoxCollider2D boxCollider = EnemyattackCollider.AddComponent<BoxCollider2D>();
                    boxCollider.isTrigger = true;

                    if (switchX == Vector2.left)
                        offset = -.25f;
                    else if (switchX == Vector2.right)
                        offset = .25f;

                    // freezes enemy after attack
                    Invoke("Freeze", 2f);
                    frozen = true;
                    anim.ResetTrigger("isWalking");

                    EnemyattackCollider.transform.position = transform.position + new Vector3(switchX.x + offset, switchX.y, 0);

                    Audio.PlayOneShot(swordAttack);

                    boxCollider.size = new Vector2(.5f, .5f);

                    Destroy(EnemyattackCollider, 0.5f);
                    moveSpeed = 0;
                    anim.SetBool("isSlicing", true);
                    
                } else if (hitPlayer.collider == null)
                {
                    moveSpeed = 5;
                    anim.SetBool("isSlicing", false);
                }
            }
        }
        
        if (enemyType == EnemyType.Ranged)
        {
            
            //Begins firing when the player is within 20 distance
            if (!isDead)
            {
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
                    Audio.PlayOneShot(rangedAttack);

                    //bullet fire time, DELAY!
                    nextTimeToFire = Time.time + fireDelay;
                }
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
            Debug.Log(collision.gameObject);
            rend.color = Color.red;
            health -= 50;
            Invoke("ColorDelay", .5f);
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        // If the enemy touches any of these listed tags then it should be ignored
        if (collider.CompareTag("EnemyAttack") || 
            collider.CompareTag("Ammo") || 
            collider.CompareTag("Health") || 
            collider.CompareTag("Coin") ||
            collider.CompareTag("EnemyBullet") ||
            collider.CompareTag("Platform") ||
            collider.CompareTag("CheckPoint"))
        {
            return;
        }
        Debug.Log(collider);
        rend.color = Color.red;
        health -= 50;
        Invoke("ColorDelay", .5f);
    }

    //flips the sprite 
    void FlipX()
    {
        Vector3 theScale = transform.localScale;
        theScale.x = theScale.x * -1;
        transform.localScale = theScale;
    }

    void ColorDelay()
    {
        rend.color = Color.white;
    }


    void Die()
    {
        GameManager.instance.MarkEnemyAsDefeated(gameObject.name); // Ensure each enemy has a unique name
        gameObject.SetActive(false);
    }
}
