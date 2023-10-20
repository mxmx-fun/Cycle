using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SpikeState
{
    Activate,
    Normal,
    Dead
}

public class SpikeEntity : MonoBehaviour
{
    //Component
    Rigidbody2D rb;
    SpriteRenderer sr;

    //Attri
    public SpikeState state;
    Vector2 size;
    public int damage;
    public float ctorTime;
    public float lifeTime;
    Color color;

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
    bool isHit;

    public void Awake()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        color = sr.color;
        size = transform.localScale;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Ctor(CycleState state)
    {
        switch (state)
        {
            case CycleState.Teach:
                moveSpeed = 2;
                isTrack = false;
                ctorTime = 3f;
                damage = 5;
                lifeTime = 10;
                break;
            case CycleState.Easy:
                moveSpeed = 2;
                isTrack = false;
                ctorTime = 3f;
                damage = 5;
                lifeTime = 10;
                break;
            case CycleState.Normal:
                moveSpeed = 3;
                isTrack = false;
                ctorTime = 2f;
                damage = 8;
                lifeTime = 5;
                break;
            case CycleState.Hard:
                moveSpeed = 4;
                isTrack = true;
                ctorTime = 1.5f;
                damage = 10;
                lifeTime = 5;
                break;
            case CycleState.Hell:
                moveSpeed = 5;
                isTrack = true;
                ctorTime = 1f;
                damage = 10;
                lifeTime = 5;
                break;
            default:
                break;
        }
        ApplyState(SpikeState.Activate);
    }

    public void ApplyState(SpikeState state)
    {
        this.state = state;
        switch (state)
        {
            case SpikeState.Activate:
                EnterActivate();
                break;
            case SpikeState.Normal:
                EnterNoraml();
                break;
            case SpikeState.Dead:
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
            case SpikeState.Activate:
                _Tick_Activate(dt);
                break;
            case SpikeState.Normal:
                _Tick_Normal(dt);
                break;
            case SpikeState.Dead:
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
            sr.color = new Color(color.r, color.g, color.b, a);
            return;
        }
        else
        {
            sr.color = new Color(color.r, color.g, color.b, 1);
            ApplyState(SpikeState.Normal);
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
            moveDir = Vector2.down;
            rb.velocity = moveDir * moveSpeed;
            return;
        }
        if (time < durationTime)
        {
            if (isTrack)
            {
                var direction = (player.transform.position - transform.position).normalized;
                transform.up = direction;
                //TODO：追踪慢慢变快，慢慢旋转
                moveDir = (player.transform.position - transform.position).normalized;
                rb.velocity = moveDir * moveSpeed;
            }
            time += dt;
            return;
        }
        else
        {
            ApplyState(SpikeState.Dead);
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
            if (!isHit) GameController.Instance.AddEvadeCount();
            Destroy(this.gameObject);
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
        if (state != SpikeState.Normal || !other.CompareTag("Player")) return;
        if (other.CompareTag("Player"))
        {
            var hitDir = (other.transform.position - transform.position).normalized;
            var role = other.GetComponent<RoleEntity>();
            role.Behit(damage, hitDir);
        }
        isHit = true;
        ApplyState(SpikeState.Dead);
    }
}
