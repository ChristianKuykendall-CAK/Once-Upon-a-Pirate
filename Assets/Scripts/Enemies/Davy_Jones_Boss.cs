using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Davy_Jones_Script : MonoBehaviour
{
    //public enum EnemyType { DavyJones };
    //public EnemyType enemyType;

    //public enum DavyState { Patrol, Chase, Attack };
    public float chaseDistance = 10f;
    public float shootDistance = 2f;
    public float sliceDistance = 1f;
    public float recoverTime = 2.5f;
    public float speed = 10;
    public float jumpForce = 3f;
    
    public LayerMask shipGround;
    private bool isGrounded;
    private bool shouldJump;

    private float offset;
    private float distance;
    private int health = 500;
    private SpriteRenderer rend;
    private Animator anim;
    private Vector2 switchX = Vector2.right;
    private Rigidbody2D rbody;

    public float moveForce;
    public float moveSpeed;

    public Transform playerTransform;
    public GameObject bullet_prefab;
    public Transform bulletPoint;
    private float fireDelay = 2f;
    private float nextTimeToFire = 0;
    public Vector2 facingDirection = Vector2.right;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        Vector2 switchX = Vector2.left;
    }

    void Update()
    {

        //gets player transform and makes davy follow that
        distance = Vector2.Distance(playerTransform.position, transform.position);
        Vector2 direction = playerTransform.position - transform.position;

        transform.position = Vector2.MoveTowards(this.transform.position, playerTransform.position, speed * Time.deltaTime);
    }

    
    private void FixedUpdate()
    {

        //triggers the walking anim
        if (moveSpeed > 0)
        {
            anim.SetTrigger("isWalking");
            //anim.SetBool("isSlicing", false);
            //anim.SetBool("isShooting", false);
        }

        //if player is to the right, face right and vice versa
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

        //remove davy health if the player shoots him
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Bullet"))
            {
                health -= 25;
            }
        }

        //kills davy if health runs out
        if (health <= 0)
        {
            anim.SetTrigger("isDead");
            Invoke("Die", 4f);
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

    void FlipX()
    {
        Vector3 theScale = transform.localScale;
        theScale.x = theScale.x * -1;
        transform.localScale = theScale;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
