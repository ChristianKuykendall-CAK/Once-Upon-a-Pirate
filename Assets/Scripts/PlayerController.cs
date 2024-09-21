using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float H;
    public float V;
    public Vector2 facingDirection = Vector2.right;
    public float moveForce;
    private Rigidbody2D rbody;
    private float delay = .8f;
    private bool isJumping = false;
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
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
        void FlipX()
        {
            Vector3 theScale = transform.localScale;
            theScale.x = theScale.x * -1;
            transform.localScale = theScale;
        }
    }

    private void FixedUpdate()
    {
        if (rbody.velocity.x < 10 && rbody.velocity.x > -10)
            rbody.AddForce(Vector2.right * Mathf.Round(H) * moveForce);
        
        //if (rbody.velocity.y < 2 && rbody.velocity.y > -2) // dropdown platform
            
    }
    IEnumerator JumpPeriod()
    {
        isJumping = true;
        
        rbody.AddForce(Vector2.up * (moveForce / 2), ForceMode2D.Impulse);

        yield return new WaitForSeconds(delay);
        
        isJumping = false;
    }
}
