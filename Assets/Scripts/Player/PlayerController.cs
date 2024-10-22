using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float H;
    public Vector2 facingDirection = Vector2.right;
    public float moveForce;
    
    //Bullet stuff
    public GameObject bullet;
    private float fireDelay = 1f;
    private float nextTimeToFire = 0;
    public Transform bullet_point;

    private SpriteRenderer rend;
    private Rigidbody2D rbody;
    private Animator anim;

    private float delay = .8f;
    private float offset;

    private bool isJumping = false;
    private bool Falling = false; // Helps toggle platform
    private bool noDamage = false; // Invicibility frames
    private bool isDead = false;

    //Text variables
    public Text HealthText;
    public Text AmmoText;
    public Text CoinText;

    public bool isPlayerDead()
    { return isDead; }

    private TilemapCollider2D tilemapCollider;

    void Start()
    {
        isDead = false;
        rend = GetComponent<SpriteRenderer>();
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        // Set up change tilemap collider to turn into trigger so player can drop through
        GameObject tilemapObject = GameObject.Find("Platform");
        if (tilemapObject != null)
        {
            tilemapCollider = tilemapObject.GetComponent<TilemapCollider2D>();
            // Debug.Log("Tilemap found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            H = Input.GetAxis("Horizontal");
            // left mouse button
            if (Input.GetMouseButtonDown(0))
            {
                //triggers the melee animation
                anim.SetTrigger("isSlicing");

                GameObject playerAttackCollider = new GameObject("PlayerAttackCollider");
                BoxCollider2D boxCollider = playerAttackCollider.AddComponent<BoxCollider2D>();
                playerAttackCollider.gameObject.tag = "PlayerAttack";
                boxCollider.isTrigger = true;

                if (facingDirection == Vector2.left)
                    offset = -.25f;
                else if (facingDirection == Vector2.right)
                    offset = .25f;

                playerAttackCollider.transform.position = transform.position + new Vector3(facingDirection.x + offset, facingDirection.y, 0);
                boxCollider.size = new Vector2(1f, 1f);

                Destroy(playerAttackCollider, 0.5f);
            }
            // right mouse button
            if (Input.GetMouseButtonDown(1) && GameManager.instance.ammo > 0 && Time.time > nextTimeToFire)
            {
                //triggers the shooting animation
                anim.SetTrigger("isShooting");

                //Summons bullet prefab, remove ammo!!!!!!!!11
                Instantiate(bullet, bullet_point.position, facingDirection == Vector2.left ? Quaternion.Euler(0, 180, 0) : bullet_point.rotation);
                if (GameManager.instance != null)
                    GameManager.instance.ammo -= 1;

                //bullet fire time, DELAY!
                nextTimeToFire = Time.time + fireDelay;


            }

            //if the player moves, trigger the walking animation
            if (H > 0 || H < 0)
            {
                anim.SetBool("isWalking", true);
            }
            else
            {
                anim.SetBool("isWalking", false);
            }

            //Sprite flipping
            if (H < 0 && facingDirection == Vector2.right)
            {
                FlipX();
                facingDirection = Vector2.left;
            }
            else if (H > 0 && facingDirection == Vector2.left)
            {
                FlipX();
                facingDirection = Vector2.right;
            }

            //Jumping
            if (Input.GetKey(KeyCode.W) && !isJumping)
                StartCoroutine(JumpPeriod());

        }
    }

    private void FixedUpdate()
    {
        LayerMask PlayerMask = LayerMask.GetMask("playerLayer"); // player has their own layer so raycast can ignore it

        if (rbody.velocity.x < 10 && rbody.velocity.x > -10)
            rbody.AddForce(Vector2.right * Mathf.Round(H) * moveForce);

        Vector2 raycastStart = new Vector2(transform.position.x, transform.position.y - 1f);
        RaycastHit2D hit = Physics2D.Raycast(raycastStart, Vector2.down, 1f, ~PlayerMask);

        //Debug.DrawRay(raycastStart, Vector2.down, Color.red);
        //Debug.Log(hit.collider);

        if (hit.collider != null && hit.collider.CompareTag("Platform") && !Falling)
        {
            tilemapCollider.isTrigger = false;
            if (Input.GetKey(KeyCode.S))
                StartCoroutine(FallThrough());
        }
        else
        {
            tilemapCollider.isTrigger = true;
        }

        //Updates the player's health, ammo, and coin count every frame
        HealthText.text = "Health: " + GameManager.instance.health;
        AmmoText.text = "Ammo: " + GameManager.instance.ammo;
        CoinText.text = "Coins: " + GameManager.instance.coin;

        //Player death
        if (GameManager.instance.health <= 0)
        {
            isDead = true;
            anim.SetBool("isWalking", false);
            anim.SetTrigger("isDead");
            Invoke("Die", 3f);
        }
    }
    // Forces player to jump once
    IEnumerator JumpPeriod()
    {
        //triggers the jumping animation
        anim.SetTrigger("isJumping");

        isJumping = true;

        rbody.AddForce(Vector2.up * (moveForce / 2), ForceMode2D.Impulse);

        yield return new WaitForSeconds(delay);

        isJumping = false;
    }
    // Keeps platform turned off long enough for player to fall through
    IEnumerator FallThrough()
    {
        Falling = true;
        tilemapCollider.isTrigger = true;
        // Debug.Log("Is working");
        yield return new WaitForSeconds(1f);
        Falling = false;

    }

    //flips the sprite along the x axis
    void FlipX()
    {
        Vector3 theScale = transform.localScale;
        theScale.x = theScale.x * -1;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!isDead)
        {
            //Enemy attack trigger 
            if (collider.CompareTag("EnemyAttack"))
            {
                Vector2 directionAwayFromEnemy = (transform.position - collider.transform.position).normalized;
                rbody.AddForce(directionAwayFromEnemy * (moveForce / 2), ForceMode2D.Impulse);
                if (!noDamage)
                    GameManager.instance.health -= 25;
                StartCoroutine(Invicibility());

            }
            if (GameManager.instance != null)
            {
                //Item Pickup triggers
                if (collider.CompareTag("Ammo"))
                {
                    GameManager.instance.ammo += 2;
                }
                if (collider.CompareTag("Health"))
                {
                    GameManager.instance.health += 50;
                }
                if (collider.CompareTag("Coin"))
                {
                    GameManager.instance.coin += 1;
                }
                if (collider.CompareTag("CheckPoint"))
                {
                    GameManager.instance.Save();
                }
            }
        }
        IEnumerator Invicibility()
        {
            //rend.color = Color.blue;
            noDamage = true;
            yield return new WaitForSeconds(2);
            noDamage = false;
            //rend.color = Color.red;
        }
    }

    void Die()
    {
        SceneManager.LoadScene("Death");
    }
}
