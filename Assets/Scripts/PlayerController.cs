using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public float H;
    public float V;
    public Vector2 facingDirection = Vector2.right;
    public float moveForce;
    private Rigidbody2D rbody;
    private float delay = .8f;
    private bool isJumping = false;
    private bool Falling = false;

    private TilemapCollider2D tilemapCollider;
    // Start is called before the first frame update

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        GameObject tilemapObject = GameObject.Find("Tilemap_Dropdown_Platform");
        if (tilemapObject != null) 
        {
            tilemapCollider = tilemapObject.GetComponent<TilemapCollider2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        H = Input.GetAxis("Horizontal");
        V = Input.GetAxis("Vertical");
        
        if (H < 0 && facingDirection == Vector2.right)
        {
            FlipX();
        }
        else if (H > 0 && facingDirection == Vector2.left)
        {
            FlipX();
        }
        if (Input.GetKey(KeyCode.W) && !isJumping)
            StartCoroutine(JumpPeriod());
    }

    private void FixedUpdate()
    {
        LayerMask PlayerMask = LayerMask.GetMask("playerLayer");

        if (rbody.velocity.x < 10 && rbody.velocity.x > -10)
            rbody.AddForce(Vector2.right * Mathf.Round(H) * moveForce);

        //if (rbody.velocity.y < 2 && rbody.velocity.y > -2) // dropdown platform
        Vector2 raycastStart = new Vector2(transform.position.x, transform.position.y - 1f);
        RaycastHit2D hit = Physics2D.Raycast(raycastStart, Vector2.down, .1f, ~PlayerMask);
        Debug.DrawRay(raycastStart, Vector2.down, Color.red);
        Debug.Log(hit.collider);

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

    }
    IEnumerator JumpPeriod()
    {
        isJumping = true;
        
        rbody.AddForce(Vector2.up * (moveForce / 2), ForceMode2D.Impulse);

        yield return new WaitForSeconds(delay);
        
        isJumping = false;
    }
    IEnumerator FallThrough()
    {
        Falling = true;
        tilemapCollider.isTrigger = true;
        Debug.Log("Working");
        yield return new WaitForSeconds(1);
        Falling = false;

    }
    void FlipX()
    {
        Vector3 theScale = transform.localScale;
        theScale.x = theScale.x * -1;
        transform.localScale = theScale;
    }
}
