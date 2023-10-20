using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GroundSpikeState
{
    Activate,
    Normal,
    Dead
}

public class GroundSpikeEntity : MonoBehaviour
{
    //Component
    Rigidbody2D rb;
    SpriteRenderer sr;

    //Attri
    public GroundSpikeState state;
    Vector2 size;
    public int damage;
    public float ctorTime;
    public float lifeTime;
    Color color;
    float groundY = -4.8f;
    float activateY = -4.35f;
    public float showSpd;
    public float hideSpd;

    //Ability
    // * Move
    public float moveSpeed;
    Vector2 moveDir;
    public bool isTrack;
    public float trackCD;
    

    //FSM
    bool isEnter;

    //Temp
    float time;
    float durationTime;
    GameObject player;
    float damageCD;

    public void Awake()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        color = sr.color;
        size = transform.localScale;
        player = GameObject.FindGameObjectWithTag("Player");
        ApplyState(GroundSpikeState.Activate);
    }

    public void ApplyState(GroundSpikeState state)
    {
        this.state = state;
        switch (state)
        {
            case GroundSpikeState.Activate:
                EnterActivate();
                break;
            case GroundSpikeState.Normal:
                EnterNoraml();
                break;
            case GroundSpikeState.Dead:
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
            case GroundSpikeState.Activate:
                _Tick_Activate(dt);
                break;
            case GroundSpikeState.Normal:
                _Tick_Normal(dt);
                break;
            case GroundSpikeState.Dead:
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
            damageCD = 0;
            transform.position = new Vector2(player.transform.position.x, groundY);
            time = 0;
            durationTime = ctorTime;
            isEnter = false;
            return;
        }
        if (time < durationTime)
        {
            time += dt;
            return;
        }

        if (damageCD > 0)
        {
            damageCD -= dt;
        }
        else
        {
            ApplyState(GroundSpikeState.Normal);
        }
    }

    void _Tick_Normal(float dt)
    {
        if (isEnter)
        {
            isEnter = false;
            time = 0;
            durationTime = lifeTime;
            //Set Spd
            moveDir = Vector2.up;
            rb.velocity = moveDir * showSpd;
            return;
        }
        if (time < durationTime)
        {
            if(transform.position.y > activateY)
            {
                transform.position = new Vector2(transform.position.x, activateY);
            }
            time += dt;
            return;
        }
        else
        {
            ApplyState(GroundSpikeState.Dead);
        }
    }

    void _Tick_Dead(float dt)
    {
        if (isEnter)
        {
            time = 0;
            isEnter = false;
            //Set Spd
            moveDir = Vector2.down;
            rb.velocity = moveDir * hideSpd;
            return;
        }

        if (transform.position.y <= groundY)
        {
            this.gameObject.SetActive(false);
            return;
        }
    }

    public void Update()
    {
        Apply_Tick(Time.deltaTime);
    }

    // Trigger
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (state == GroundSpikeState.Activate) return;
        if (other.CompareTag("Player") && damageCD <= 0)
        {
            var role = other.GetComponent<RoleEntity>();
            role.Behit(damage);
            damageCD = 0.5f;
        }
    }
}
