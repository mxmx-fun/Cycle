using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleEntity : MonoBehaviour
{
    //Component
    public Rigidbody2D rb;

    //Attri
    public int hp;


    //Ability
    // * Move
    public float moveSpeed;

    // * Jump
    bool inGround;
    public int jumpTime;
    public int maxJumpTime;
    public float jumpForce;


    //Init
    public void Awake()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        hp = 100;
        moveSpeed = 5;
        maxJumpTime = 1;
        jumpForce = 5;
        jumpTime = maxJumpTime;
    }

    public void Jump()
    {
        if (jumpTime > 0)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jumpTime--;
        }
    }

    public void Move(Vector2 dir)
    {
        rb.velocity = dir * moveSpeed + new Vector2(0, rb.velocity.y);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D");
        if (collision.gameObject.tag == "Ground")
        {
            inGround = true;
            jumpTime = maxJumpTime;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("OnCollisionExit2D");
        if (collision.gameObject.tag == "Ground")
        {
            inGround = false;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.A))
        {
            Move(Vector2.left);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Move(Vector2.right);
        }
        else
        {
            Move(Vector2.zero);
        }
    }
}
