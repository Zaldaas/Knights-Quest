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
    float moveForce = 2;
    float jumpForce = 5;
    bool isFacingRight = false;
    public float delay = .3f;
    private bool attackBlocked;

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
        rb.AddForce(transform.right * moveForce * Input.GetAxis("Horizontal"), ForceMode2D.Force);
        
        if(rb.velocity.magnitude > .01f)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
        
        bool movingLeft = Input.GetAxis("Horizontal") < 0;
        bool movingRight = Input.GetAxis("Horizontal") > 0;

        if(movingLeft || movingRight)
        {
            FlipCharacter(movingRight);
        }

        bool isntJumping = GetComponent<Rigidbody2D>().velocity.y == 0;

        if((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && isntJumping && !DeathZone.IsGameOver)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            anim.SetTrigger("Jump");
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
}
