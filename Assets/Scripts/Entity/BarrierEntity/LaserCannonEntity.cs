using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LaserCannonState
{
    Activate,
    Normal,
    Dead
}

public class LaserCannonEntity : MonoBehaviour
{
    //Component
    Rigidbody2D rb;
    BoxCollider2D collider;
    SpriteRenderer sr;

    //Attri
    public LaserCannonState state;
    Vector2 originalSize;
    public int damage;
    public float ctorTime;
    public float lifeTime;
    Color color;
    Transform imgTF;
    float activateY;

    //Ability
    // * Move
    public bool isMove;
    public float moveSpeed;
    Vector2 moveDir;
    public bool isTrack;
    public float trackCD;
    float damageCD;


    //FSM
    bool isEnter;

    //Temp
    float time;
    float durationTime;
    GameObject player;

    public void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        rb = GetComponentInChildren<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        color = sr.color;
        player = GameObject.FindGameObjectWithTag("Player");
        imgTF = transform.Find("Img");
        activateY = 20;
        originalSize = imgTF.localScale;
        ApplyState(LaserCannonState.Activate);
    }

    public void ApplyState(LaserCannonState state)
    {
        this.state = state;
        switch (state)
        {
            case LaserCannonState.Activate:
                EnterActivate();
                break;
            case LaserCannonState.Normal:
                EnterNoraml();
                break;
            case LaserCannonState.Dead:
                EnterDead();
                break;
            default:
                break;
        }
    }

    void EnterActivate()
    {
        isEnter = true;
    }

    void EnterNoraml()
    {
        isEnter = true;
    }

    void EnterDead()
    {
        isEnter = true;
    }

    public void Apply_Tick(float dt)
    {
        switch (state)
        {
            case LaserCannonState.Activate:
                _Tick_Activate(dt);
                break;
            case LaserCannonState.Normal:
                _Tick_Normal(dt);
                break;
            case LaserCannonState.Dead:
                _Tick_Dead(dt);
                break;
            default:
                break;
        }
    }

    void _Tick_Activate(float dt)
    {
        if (isEnter)
        {
            time = 0;
            durationTime = ctorTime;
            isEnter = false;
            return;
        }
        if (time < durationTime)
        {
            time += dt;
            float a = time / durationTime;
            float y = a * activateY;
            sr.color = new Color(color.r, color.g, color.b, a);
            imgTF.localScale = new Vector3(imgTF.localScale.x, y, imgTF.localScale.z);
            return;
        }
        else
        {
            sr.color = new Color(color.r, color.g, color.b, 1);
            imgTF.localScale = new Vector3(imgTF.localScale.x, activateY, imgTF.localScale.z);
            ApplyState(LaserCannonState.Normal);
        }
    }

    void _Tick_Normal(float dt)
    {
        if (isEnter)
        {
            collider.size = imgTF.localScale;
            isEnter = false;
            time = 0;
            durationTime = lifeTime;
            //Set Spd
            moveDir = Vector2.down;
            rb.velocity = moveDir * moveSpeed;
            return;
        }
        if (time < durationTime)
        {
            if (isMove)
            {
                if (isTrack)
                {
                    var targetPos = new Vector3(player.transform.position.x,transform.position.y,transform.position.z);
                    //TODO：追踪慢慢变快，慢慢旋转
                    moveDir = (targetPos - transform.position).normalized;
                    rb.velocity = moveDir * moveSpeed;
                }
            }

            if (damageCD > 0)
            {
                damageCD -= dt;
            }

            time += dt;
            return;
        }
        else
        {
            ApplyState(LaserCannonState.Dead);
        }
    }

    void _Tick_Dead(float dt)
    {
        if (isEnter)
        {
            time = 0;
            isEnter = false;
            //Set Spd
            rb.velocity = Vector2.zero;
            this.gameObject.SetActive(false);
            return;
        }
    }

    public void Update()
    {
        Apply_Tick(Time.deltaTime);
    }

    // Trigger
    public void OnTriggerStay2D(Collider2D other)
    {
        if (state != LaserCannonState.Normal) return;
        if (other.CompareTag("Player") && damageCD <= 0)
        {
            var role = other.GetComponent<RoleEntity>();
            role.Behit(damage);
            damageCD = 1;
        }
    }
}
