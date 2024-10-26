using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Davy_Jones_Script : MonoBehaviour
{
    public enum EnemyType { DavyJones };
    public EnemyType enemyType;

    public enum DavyState { Patrol, Chase, Attack };
    public float chaseDistance = 10f;
    public float shootDistance = 2f;
    public float sliceDistance = 1f;
    public float recoverTime = 2.5f;
    public float speed = 10;

    public DavyState davyState;

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

        //ChangeState(davyState);
    }

    void Update()
    {
        distance = Vector2.Distance(playerTransform.position, transform.position);
        Vector2 direction = playerTransform.position - transform.position;

        transform.position = Vector2.MoveTowards(this.transform.position, playerTransform.position, speed * Time.deltaTime);
    }

    
    private void FixedUpdate()
    {

        if (moveSpeed > 0)
        {
            anim.SetTrigger("isWalking");
            //anim.SetBool("isSlicing", false);
            //anim.SetBool("isShooting", false);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Bullet"))
            {
                health -= 25;
            }
        }

        if (health <= 0)
        {
            anim.SetTrigger("isDead");
            Invoke("Die", 4f);
        }

    }

    /*
    void ChangeState(DavyState newState)
    {
        StopAllCoroutines();
        davyState = newState;
        switch (davyState)
        {
            case DavyState.Patrol:
                StartCoroutine("AI_Patrol");
                break;
            case DavyState.Chase:
                StartCoroutine("AI_Chase");
                break;
            case DavyState.Attack:
                StartCoroutine("AI_Attack");
                break;
        }
    }

    IEnumerator AI_Patrol()
    {
        anim.SetBool("IsIdle", true);
        anim.SetBool("IsWalking", false);

        while (true) // keep the object patrolling 
        {
            // Are we close to the player -- if so, we need to chase
            if (Vector2.Distance(playerTransform.position, transform.position) < chaseDistance)
            {
                ChangeState(EnemyState.Chase);
                yield break;
            }
            else // Keep patrolling
            {
                Vector3 randomPosition = Random.insideUnitSphere * chaseDistance;
                randomPosition += transform.position;

                yield return new WaitForSeconds(3f);
            }

        }
    }

    IEnumerator AI_Chase()
    {
        anim.SetBool("IsWalking", true);
        anim.SetBool("IsSlicing", false);
        anim.SetBool("IsIdle", false);
        billboard.enabled = true; //make face player

        while (true) // keep the object patrolling 
        {
            //playerTransform = GameObject.FindGameObjectWithTag("player").transform;

            // Are we close to the player -- if so, we need to chase
            if (Vector3.Distance(playerTransform.position, transform.position) < attackDistance)
            {
                ChangeState(EnemyState.Attack);  //updated state here!
                yield break;
            }
            else if (Vector3.Distance(playerTransform.position, transform.position) > chaseDistance)
            {
                ChangeState(EnemyState.Patrol);
                yield break;
            }
            //playerTransform = GameObject.FindGameObjectWithTag("player").transform;
            //agent.SetDestination(playerTransform.position); // the player's position
            yield return new WaitForSeconds(1f);

        }
    }

    IEnumerator AI_Attack()
    {
        anim.SetBool("IsAttacking", true);
        anim.SetBool("IsChasing", false);
        float elapsedTime = 3f;

        while (true)
        {
            playerTransform = GameObject.FindGameObjectWithTag("player").transform;

            // Are we close to the player -- if so, we need to chase
            if (Vector3.Distance(playerTransform.position, transform.position) > attackDistance)
            {
                ChangeState(EnemyState.Chase);  //updated state here!
                yield break;
            }
            //Debug.Log(elapsedTime);
            elapsedTime += 1;
            if (elapsedTime >= recoverTime)
            {
                //PlayerController player_controller = GameObject.FindGameObjectWithTag("player").GetComponent<PlayerController>();
                //player_controller.PlayerDamage(20);  // this is a bad way to do this; we'll update this later!
                EventManager.TriggerEvent("PlayerDamage", 20);
                Debug.Log("Damage player");
                elapsedTime = 0f;
            }

            yield return new WaitForSeconds(1f);
        }


        
        void Freeze()
        {
            frozen = false;
            rbody.velocity = switchX * 0;
        }
        */

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

    void Die()
    {
        Destroy(gameObject);
    }
}
