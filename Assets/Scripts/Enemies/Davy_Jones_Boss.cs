using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.SceneManagement;

public class Davy_Jones_Script : MonoBehaviour
{
    public enum EnemyType { DavyJones, DavyBones };
    public EnemyType enemyType;

    //public enum DavyState { Patrol, Chase, Attack };
    public GameObject player;
    public GameObject platform;
    public float chaseDistance = 10f;
    public float shootDistance = 2f;
    public float sliceDistance = 1f;
    public float recoverTime = 2.5f;
    public float speed = 10f;
    public float jumpForce = 2f;
    
    private bool isDead = false;
    private bool frozen = false;
    private bool isAttacking = false;

    //public LayerMask shipGround;
    //private bool isGrounded;
    //private bool shouldJump;
    private bool isShooting;
    private float delay = .8f;

    //private bool isJumping = false;

    private float offset;
    private float distance;
    public int health = 500;
    private SpriteRenderer rend;
    private Animator anim;
    private Vector2 switchX = Vector2.right;
    private Rigidbody2D rbody;
    //private TilemapCollider2D tilemapCollider;

    public float moveForce;
    public float moveSpeed;

    public GameObject tentacle;
    public Transform playerTransform;
    public GameObject bullet_prefab;
    public Transform bulletPoint;
    private float fireDelay = 2.5f;
    private float nextTimeToFire = 0;
    public Vector2 facingDirection = Vector2.right;

    private AudioSource Audio;

    public AudioClip swordAttack;
    public AudioClip rangedAttack;
    public AudioClip deathSound;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        Audio = GetComponent<AudioSource>();
        Vector2 switchX = Vector2.left;

        if (enemyType == EnemyType.DavyJones)
        {
            gameObject.tag = "DavyJones";
        }
        else if (enemyType == EnemyType.DavyBones)
        {
            gameObject.tag = "DavyBones";
        }
    }

    void Update()
    {
        //layermasks for melee attacks
        LayerMask EnemyMask = LayerMask.GetMask("enemyLayer");
        RaycastHit2D hitPlayer = Physics2D.Raycast(transform.position, switchX, 4f, ~EnemyMask);

        //makes davy move
        if (moveForce > 0 && moveSpeed > 0)
        {
            //gets player transform and makes davy follow that
            distance = Mathf.Abs(playerTransform.position.x - transform.position.x);
            Vector2 direction = new Vector2(playerTransform.position.x - transform.position.x, 0).normalized;

            transform.position = Vector2.MoveTowards(this.transform.position, playerTransform.position, jumpForce * Time.deltaTime);
            //Debug.Log("Hit Platform");
        }

        //Shooting attacks and Melee attacks
        if (!isDead && enemyType == EnemyType.DavyJones)
        {
            if (Vector2.Distance(playerTransform.position, transform.position) < 25f && Time.time > nextTimeToFire)
            {
                //makes him stop moving while shooting
                moveSpeed = 0;
                moveForce = 0;

                isShooting = true;
                //trigger the shooting anim
                frozen = true;
                Freeze();
                anim.ResetTrigger("isWalking");
                anim.SetTrigger("isShooting");

                //spawns bullet prefab in direction facing
                Instantiate(bullet_prefab, bulletPoint.position, facingDirection == Vector2.left ? Quaternion.Euler(0, 180, 0) : bulletPoint.rotation);

                Audio.PlayOneShot(rangedAttack);

                //bullet fire time, DELAY!
                nextTimeToFire = Time.time + fireDelay;
                frozen = false;
                isShooting = false;
                //moveSpeed = 5;
            }
            else if (Vector2.Distance(playerTransform.position, transform.position) > 15f && Time.time < nextTimeToFire)
            {
                //makes him move again
                moveSpeed = 5;
                moveForce = 10;
            }

            if (Vector2.Distance(playerTransform.position, transform.position) < 5f && !frozen)
            {
                //makes him speed up and hit the player when close enough
                transform.position = Vector2.MoveTowards(this.transform.position, playerTransform.position, 2 * Time.deltaTime);

                //melee attack
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

                    frozen = true;
                    Invoke("Freeze", 2f);


                    EnemyattackCollider.transform.position = transform.position + new Vector3(switchX.x + offset, switchX.y, 0);

                    Audio.PlayOneShot(swordAttack);

                    boxCollider.size = new Vector2(.5f, .5f);

                    Destroy(EnemyattackCollider, 0.5f);
                    anim.SetTrigger("isSlicing");
                }
            }
        }
        else if (!isDead && enemyType == EnemyType.DavyBones)
        {
            isAttacking = false;
            if (Vector2.Distance(playerTransform.position, transform.position) > 20f /*&& Time.time < nextTimeToFire*/)
            {
                //makes him move
                moveSpeed = 10;
                moveForce = 12;
            }

            //coffin slam attack
            if (Vector2.Distance(playerTransform.position, transform.position) < 10f && !frozen)
            {
                //makes him speed up and hit the player when close enough
                transform.position = Vector2.MoveTowards(this.transform.position, playerTransform.position, 2 * Time.deltaTime);

                //slam attack
                if (hitPlayer.collider != null && hitPlayer.collider.CompareTag("Player"))
                {
                    if(Vector2.Distance(playerTransform.position, transform.position) < 3f && !frozen)
                    {
                        anim.SetTrigger("isSlammingCoffin");
                        GameObject EnemyattackCollider = new GameObject("EnemyAttackCollider");
                        EnemyattackCollider.gameObject.tag = "EnemyAttack";
                        BoxCollider2D boxCollider = EnemyattackCollider.AddComponent<BoxCollider2D>();
                        boxCollider.isTrigger = true;

                        if (switchX == Vector2.left)
                            offset = -.25f;
                        else if (switchX == Vector2.right)
                            offset = .25f;

                        frozen = true;
                        Invoke("Freeze", 1f);


                        EnemyattackCollider.transform.position = transform.position + new Vector3(switchX.x + offset, switchX.y, 0);

                        //Audio.PlayOneShot(swordAttack);

                        boxCollider.size = new Vector2(.5f, .5f);

                        Destroy(EnemyattackCollider, 0.5f);
                        //anim.ResetTrigger("isWalking");
                        isAttacking = true;
                    }else if (Vector2.Distance(playerTransform.position, transform.position) < 5f && !frozen)
                    {
                        anim.SetTrigger("isTentacleAttacking");
                        GameObject EnemyattackCollider = new GameObject("EnemyAttackCollider");
                        EnemyattackCollider.gameObject.tag = "EnemyAttack";
                        BoxCollider2D boxCollider = EnemyattackCollider.AddComponent<BoxCollider2D>();
                        boxCollider.isTrigger = true;

                        if (switchX == Vector2.left)
                            offset = -.25f;
                        else if (switchX == Vector2.right)
                            offset = .25f;

                        frozen = true;
                        Invoke("Freeze", 1f);


                        EnemyattackCollider.transform.position = transform.position + new Vector3(switchX.x + offset, switchX.y, 0);

                        //Audio.PlayOneShot(swordAttack);

                        boxCollider.size = new Vector2(.5f, .5f);

                        Destroy(EnemyattackCollider, 0.5f);
                        //anim.ResetTrigger("isWalking");
                        isAttacking = true;
                    }

                }
            //AOE tentacle attack!
            }else if(Vector2.Distance(playerTransform.position, transform.position) > 10f && Vector2.Distance(playerTransform.position, transform.position) < 25f && !frozen)
            {
                float spawnX = playerTransform.position.x + 6; // Use the player's X position
                float spawnY = transform.position.y;      // Use the Davy Bones' current Y position
                Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0); // Spawn position with adjusted Y

                //if there is no tentacle spawned, spawn one in
                if (GameObject.Find("Tentacle(Clone)") == null)
                {
                    Instantiate(tentacle, spawnPosition, Quaternion.identity);
                    Destroy(GameObject.Find("Tentacle(Clone)"), 5f);
                }
            }
        }
    }

    
    private void FixedUpdate()
    {

        //triggers the walking anim
        if (moveSpeed > 0)
        {
            anim.SetTrigger("isWalking");
        }

        //if player is to the right, face right and vice versa
        if (playerTransform.position.x > transform.position.x && !isDead)
        {
            switchX = Vector2.right;
            facingDirection = Vector2.right;
            if (transform.localScale.x < 0) // If the sprite is flipped, flip it back
            {
                FlipX();
            }
        }
        else if (playerTransform.position.x < transform.position.x && !isDead)
        {
            switchX = Vector2.left;
            facingDirection = Vector2.left;
            if (transform.localScale.x > 0) // If the sprite is not flipped, flip it
            {
                FlipX();
            }
        }

        //kills davy if health runs out
        if (health <= 0 && !isDead)
        {
            isDead = true;

            Audio.PlayOneShot(deathSound);

            moveForce = 0;
            frozen = true;
            Freeze();

            anim.SetTrigger("isDead");
            if(enemyType == EnemyType.DavyJones)
            {
                Invoke("Die", 3f);
            }else if(enemyType == EnemyType.DavyBones)
            {
                Invoke("Die2", 3f);
            }
        }

    }
    
    //remove davy health if the player shoots him
    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Bullet"))
        {
            rend.color = Color.red;
            health -= 50;
            Invoke("ColorDelay", .5f);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("EnemyAttack") ||
            collider.CompareTag("Ammo") ||
            collider.CompareTag("Health") ||
            collider.CompareTag("Coin") ||
            collider.CompareTag("EnemyBullet") ||
            collider.CompareTag("Tentacle") ||
            collider.CompareTag("Platform"))
        {
            return;
        }
        rend.color = Color.red;
        health -= 50;
        Invoke("ColorDelay", .5f);
    }

    void FlipX()
    {
        Vector3 theScale = transform.localScale;
        theScale.x = theScale.x * -1;
        transform.localScale = theScale;
    }

    void Freeze()
    {
        rbody.velocity = switchX * 0;
        frozen = false;
    }

    void Die()
    {
        SceneManager.LoadScene("Level Change");
    }

    void Die2()
    {
        SceneManager.LoadScene("Credits");
    }

    void ColorDelay()
    {
        rend.color = Color.white;
    }
}
