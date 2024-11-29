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
    private bool isPaused = false;

    //health bar slider variable
    public Slider HealthBar;

    //Text variables
    public Text HealthText;
    public Text AmmoText;
    public Text CoinText;
    public Text CheckText;

    //pause image variable
    public Image pause;

    public Button Load;
    public Button Menu;

    //Audio
    private AudioSource Audio;

    public AudioClip ammoPickup;
    public AudioClip checkPickup;
    public AudioClip coinPickup;
    public AudioClip healthPickup;
    public AudioClip swordAttack;
    public AudioClip rangedAttack;
    public AudioClip deathSound;

    public bool isPlayerDead()
    { return isDead; }

    private TilemapCollider2D tilemapCollider;

    void Start()
    {
        isDead = false;
        rend = GetComponent<SpriteRenderer>();
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Audio = GetComponent<AudioSource>();
        HealthBar.maxValue = 100;
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
            if (Input.GetMouseButtonDown(0) && Time.time > nextTimeToFire && !isPaused)
            {
                //triggers the melee animation
                anim.SetTrigger("isSlicing");
                Audio.PlayOneShot(swordAttack);

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

                //swing delay
                nextTimeToFire = Time.time + fireDelay;
            }
            // right mouse button
            if (Input.GetMouseButtonDown(1) && GameManager.instance.ammo > 0 && Time.time > nextTimeToFire && !isPaused)
            {
                //triggers the shooting animation
                anim.SetTrigger("isShooting");
                Audio.PlayOneShot(rangedAttack);

                //Summons bullet prefab, remove ammo!!!!!!!!
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

            //Pause inputs
            if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
            {
                Time.timeScale = 0;
                Menu.image.enabled = true;
                Menu.enabled = true;
                Load.image.enabled = true;
                Load.enabled = true;
                pause.enabled = true;
                isPaused = true;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
            {
                Time.timeScale = 1;
                Menu.image.enabled = false;
                Menu.enabled = false;
                Load.image.enabled = false;
                Load.enabled = false;
                pause.enabled = false;
                isPaused = false;
            }

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
        HealthBar.value = GameManager.instance.health;
        HealthText.text = "Health: " + GameManager.instance.health;
        AmmoText.text = "Ammo: " + GameManager.instance.ammo;
        CoinText.text = "Coins: " + GameManager.instance.coin;

        //Player death
        if (GameManager.instance.health <= 0 && !isDead)
        {
            isDead = true;
            Audio.PlayOneShot(deathSound);
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
            if (collider.CompareTag("Barrier"))
            {
                GameManager.instance.Load();
                transform.position = GameManager.instance.playerTransformBarrier;
            }
            //Enemy attack trigger 
            if (collider.CompareTag("EnemyAttack") && !isPaused)
            {
                Vector2 directionAwayFromEnemy = (transform.position - collider.transform.position).normalized;
                directionAwayFromEnemy.y = 0;
                rbody.AddForce(directionAwayFromEnemy * (moveForce / 2), ForceMode2D.Impulse);
                if (!noDamage)
                    GameManager.instance.health -= 25;
                StartCoroutine(Invicibility());

            }
            if(collider.CompareTag("Tentacle") && !isPaused)
            {
                Vector2 directionAwayFromEnemy = (transform.position - collider.transform.position).normalized;
                directionAwayFromEnemy.y = 0;
                rbody.AddForce(directionAwayFromEnemy * (moveForce / 2), ForceMode2D.Impulse);
                if (!noDamage)
                    GameManager.instance.health -= 15;
                StartCoroutine(Invicibility());
            }
            if (collider.CompareTag("EnemyBullet") && !isPaused)
            {
                StartCoroutine(Invicibility());
            }
            if (GameManager.instance != null)
            {
                //Item Pickup triggers
                if (collider.CompareTag("Ammo"))
                {
                    GameManager.instance.ammo += 3;
                    Audio.PlayOneShot(ammoPickup);
                }
                if (collider.CompareTag("Health"))
                {
                    GameManager.instance.health += 50;
                    Audio.PlayOneShot(healthPickup);
                }
                if (collider.CompareTag("Coin"))
                {
                    GameManager.instance.coin += 1;
                    Audio.PlayOneShot(coinPickup);
                }
                if (collider.CompareTag("CheckPoint"))
                {
                    GameManager.instance.Save();
                    CheckText.enabled = true;
                    Audio.PlayOneShot(checkPickup);
                    Invoke("TextDisable", 2f);
                }
            }
        }
        IEnumerator Invicibility()
        {
            rend.color = Color.red;
            noDamage = true;
            yield return new WaitForSeconds(2);
            noDamage = false;
            rend.color = Color.white;
        }
    }

    void Die()
    {
        SceneManager.LoadScene("Death");
    }

    void TextDisable()
    {
        CheckText.enabled = false;
    }

    void LoadMenu() //clicking the menu button will load the main menu
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }

    void LoadSave() //load saved data
    {
        SceneManager.LoadScene("LevelOne");
        // Ensure we load the game after the scene has fully loaded
        SceneManager.sceneLoaded += OnGameSceneLoaded;
        Time.timeScale = 1;
    }

    private void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "LevelOne" && GameManager.instance != null)
        {
            GameManager.instance.Load();
            GameObject player = GameObject.FindWithTag("Player");

            if (player != null)
            {
                // Set the player's position to the saved position in GameManager
                player.transform.position = GameManager.instance.playerTransform;
            }
            else
            {
                Debug.LogError("Player object not found in the scene!");
            }
            SceneManager.sceneLoaded -= OnGameSceneLoaded; // Unsubscribe to prevent multiple calls
        }
    }
}
