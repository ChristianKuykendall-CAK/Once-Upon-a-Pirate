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
        if (rbody.velocity.y < 2 && rbody.velocity.y > -2)
            rbody.AddForce(Vector2.up * V * moveForce * 10);
    }
}
