using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RoleState
{
    DeActivate,
    Normal,
    Behit,
    Stun,
    Invincible,
    Dead
}

public class RoleEntity : MonoBehaviour
{
    //Component
    Rigidbody2D rb;
    Text roleTxt;
    SpriteRenderer sr;

    //Attri
    public int hp;
    public int maxHp;
    public int shield;
    public int maxShield;
    public RoleState state;
    Vector2 size;

    public bool isInvincible;
    public float InvincibleDurationTime;


    //Ability
    // * Move
    public float moveSpeed;

    public Vector2 moveXRange;

    // * Jump
    bool inGround;
    public int jumpTime;
    public int maxJumpTime;
    float jumpForce;

    // FSM
    bool isEnter = false;

    // TEMP
    float time;
    float durationTime;


    //Init
    public void Awake()
    {
        roleTxt = GetComponentInChildren<Text>();
        rb = GetComponentInChildren<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        maxHp = 100;
        maxShield = maxHp;
        shield = 0;
        hp = maxHp;
        moveSpeed = 5;
        jumpForce = 5;
        jumpTime = maxJumpTime;
        moveXRange = new Vector2(-9.7F, 9.7F);
        size = transform.localScale;
        ApplyState(RoleState.Normal);
    }

    public void ApplyState(RoleState state)
    {
        switch (state)
        {
            case RoleState.Normal:
                EnterNormal();
                break;
            case RoleState.Behit:
                EnterBehit();
                break;
            case RoleState.Stun:
                EnterStun();
                break;
            case RoleState.Invincible:
                EnterInvincible();
                break;
            case RoleState.Dead:
                EnterDead();
                break;
        }
    }

    public void Apply_Tick(float dt)
    {
        switch (state)
        {
            case RoleState.Normal:
                _Tick_Normal(dt);
                break;
            case RoleState.Behit:
                _Tick_Behit(dt);
                break;
            case RoleState.Stun:
                _Tick_Stun(dt);
                break;
            case RoleState.Invincible:
                _Tick_Invincible(dt);
                break;
            case RoleState.Dead:
                _Tick_Dead(dt);
                break;
        }
    }

    #region RoleAction
    public void Jump()
    {
        if (jumpTime > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpForce);
            jumpTime--;
        }
    }

    public void Move(Vector2 dir)
    {
        if (CheckPos(out var fixPos))
        {
            rb.velocity = dir * moveSpeed + new Vector2(0, rb.velocity.y);
        }
    }

    bool CheckPos(out Vector2 fixPos)
    {
        var pos = transform.position;
        var scale = size.x / 2;
        if (pos.x - scale < moveXRange.x)
        {
            fixPos = new Vector2(moveXRange.x + scale, pos.y);
            return false;
        }

        if (pos.x + scale > moveXRange.y)
        {
            fixPos = new Vector2(moveXRange.y - scale, pos.y);
            return false;
        }

        fixPos = Vector2.zero;
        return true;
    }

    public bool Behit(int damage, Vector3 behitDir)
    {
        if (isInvincible) return false;
        //shield
        if (shield > 0)
        {
            if (shield >= damage)
            {
                shield -= damage;
                if (shield < 0)
                {
                    hp += shield;
                    shield = 0;
                }
                UpdateTxt();
                return false;
            }
            else
            {
                damage -= shield;
                shield = 0;
            }
        }

        //hp
        hp -= damage;
        rb.velocity = behitDir * 5;

        if (hp <= 0)
        {
            hp = 0;
            ApplyState(RoleState.Dead);
            return true;
        }

        if (state != RoleState.Stun)
        {
            ApplyState(RoleState.Behit);
        }
        return true;
    }
    #endregion

    #region RoleFSM
    public void EnterNormal()
    {
        if (state == RoleState.Normal) return;
        state = RoleState.Normal;
        UpdateTxt();
        UpdateColor();
        isEnter = true;
    }

    public void EnterInvincible()
    {
        if (state == RoleState.Invincible) return;
        state = RoleState.Invincible;
        roleTxt.text = "无敌中";
        sr.color = Color.cyan;
        isEnter = true;
    }

    public void EnterBehit()
    {
        if (state == RoleState.Behit) return;
        state = RoleState.Behit;
        UpdateTxt();
        UpdateColor();
        isEnter = true;
    }

    public void EnterStun()
    {
        if (state == RoleState.Stun) return;
        state = RoleState.Stun;
        roleTxt.text = "晕眩中";
        sr.color = Color.grey;
        isEnter = true;
    }

    public void EnterDead()
    {
        if (state == RoleState.Dead) return;
        UpdateTxt();
        state = RoleState.Dead;
        isEnter = true;
    }

    void _Tick_Normal(float dt)
    {
        if (isEnter)
        {
            isEnter = false;
        }
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

    void _Tick_Behit(float dt)
    {
        if (isEnter)
        {
            time = 0;
            durationTime = 0.5F;
            isEnter = false;
        }
        if (time < durationTime)
        {
            time += dt;
            return;
        }
        else
        {
            ApplyState(RoleState.Normal);
        }
    }

    void _Tick_Stun(float dt)
    {
        if (isEnter)
        {
            time = 0;
            durationTime = 1F;
            isEnter = false;
        }

        if (time < durationTime)
        {
            time += dt;
            return;
        }
        else
        {
            ApplyState(RoleState.Normal);
        }
    }

    void _Tick_Invincible(float dt)
    {
        if (isEnter)
        {
            isInvincible = true;
            isEnter = false;
        }

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

        if (InvincibleDurationTime > 0)
        {
            InvincibleDurationTime -= dt;
            return;
        }
        else
        {
            isInvincible = false;
            InvincibleDurationTime = 0;
            ApplyState(RoleState.Normal);
        }
    }

    void _Tick_Dead(float dt)
    {
        if (isEnter)
        {
            isEnter = false;
            GameController.Instance.GameOver();
        }

    }
    #endregion
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
        Apply_Tick(Time.deltaTime);
        if (!CheckPos(out Vector2 fixPos))
        {
            rb.velocity = -rb.velocity;
            transform.position = fixPos;
        }
    }

    public void UpdateColor()
    {
        if (shield > 0)
        {
            sr.color = Color.blue;
            return;
        }

        var lifePercent = (float)hp / (float)maxHp;
        if (lifePercent > 0.8F)
        {
            sr.color = Color.green;
        }
        else if (lifePercent > 0.3F)
        {
            sr.color = Color.yellow;
        }
        else
        {
            sr.color = Color.red;
        }
    }

    public void UpdateTxt()
    {
        if (state == RoleState.Stun || state == RoleState.Invincible) return;

        if (shield > 0)
        {
            roleTxt.text = shield.ToString();
        }
        else
        {
            roleTxt.text = hp.ToString();
        }

    }
}
