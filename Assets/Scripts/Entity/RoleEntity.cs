using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleEntity : MonoBehaviour
{
    //Component
    public Rigidbody2D rb;

    //Attri
    public int hp;
    public Vector2 size;


    //Ability
    // * Move
    public float moveSpeed;

    public Vector2 moveXRange;

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
        moveXRange = new Vector2(-9.7F, 9.7F);
        size = transform.localScale;
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
        if (CheckMove(out var fixPos))
        {
            rb.velocity = dir * moveSpeed + new Vector2(0, rb.velocity.y);
        }
        else
        {
            Debug.Log("LimitMove");
            transform.position = fixPos;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    bool CheckMove(out Vector2 fixPos)
    {
        var pos = transform.position;
        var scale = size.x / 2;
        if(pos.x - scale < moveXRange.x) {
            fixPos = new Vector2(moveXRange.x + scale, pos.y);
            return false;
        } 

        if(pos.x + scale > moveXRange.y) {
            fixPos = new Vector2(moveXRange.y - scale, pos.y);
            return false;
        }

        fixPos = Vector2.zero;
        return true;
    }

    //Collision
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            inGround = true;
            jumpTime = maxJumpTime;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
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
