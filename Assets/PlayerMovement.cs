using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    Transform modelTransform;
    Transform legTransform;
    Transform leg1Transform;
    float moveForce = 6.6f;
    float jumpForce = 5f;
    bool isFacingRight = false;
    public float delay = 0.3f;
    private bool attackBlocked;

    // Input variables
    float horizontalInput;
    bool jumpInput;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        modelTransform = transform.Find("model");
        legTransform = modelTransform.Find("Leg");
        leg1Transform = modelTransform.Find("Leg (1)");
    }

    // Update is called once per frame
    void Update()
    {
        // Handle input in Update
        horizontalInput = Input.GetAxis("Horizontal");

        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && 
            IsGrounded() && !DeathZone.IsGameOver)
        {
            jumpInput = true;
        }

        // Update animation state
        if (rb.velocity.magnitude > 0.01f)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        // Handle character flipping
        bool movingLeft = horizontalInput < 0;
        bool movingRight = horizontalInput > 0;

        if (movingLeft || movingRight)
        {
            FlipCharacter(movingRight);
        }
    }

    // FixedUpdate is called at a fixed interval and is independent of frame rate
    void FixedUpdate()
    {
        // Apply movement force
        rb.AddForce(transform.right * moveForce * horizontalInput, ForceMode2D.Force);

        // Handle jump in FixedUpdate to ensure consistent physics behavior
        if (jumpInput)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            anim.SetTrigger("Jump");
            jumpInput = false;
        }
    }

    void FlipCharacter(bool moveRight)
    {
        if ((moveRight && !isFacingRight) || (!moveRight && isFacingRight))
        {
            Vector3 theScale = modelTransform.localScale;
            theScale.x *= -1;
            modelTransform.localScale = theScale;

            isFacingRight = !isFacingRight;
        }
    }

    bool IsGrounded()
    {
        // Implement your ground check logic here
        return Mathf.Abs(rb.velocity.y) < 0.01f;
    }
}
