using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RandomSupplyState
{
    Activate,
    Normal,
    Dead
}

public class RandomSupplyEntity : MonoBehaviour
{
    //Component
    Rigidbody2D rb;
    SpriteRenderer sr;

    //Attri
    public int level;
    public RandomSupplyState state;
    Vector2 size;
    public float ctorTime;
    public float lifeTime;


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

    public void Awake()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        size = transform.localScale;
        player = GameObject.FindGameObjectWithTag("Player");
        ApplyState(RandomSupplyState.Activate);
    }

    public void ApplyState(RandomSupplyState state)
    {
        this.state = state;
        switch (state)
        {
            case RandomSupplyState.Activate:
                EnterActivate();
                break;
            case RandomSupplyState.Normal:
                EnterNoraml();
                break;
            case RandomSupplyState.Dead:
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
            case RandomSupplyState.Activate:
                _Tick_Activate(dt);
                break;
            case RandomSupplyState.Normal:
                _Tick_Normal(dt);
                break;
            case RandomSupplyState.Dead:
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
            return;
        }
        else
        {
            ApplyState(RandomSupplyState.Normal);
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
            ApplyState(RandomSupplyState.Dead);
        }
    }

    void _Tick_Dead(float dt)
    {
        if (isEnter)
        {
            time = 0;
            isEnter = false;
            //Set Spd
            Destroy(this.gameObject);
            return;
        }
    }

    public void Update()
    {
        Apply_Tick(Time.deltaTime);
    }

    // Trigger
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (state == RandomSupplyState.Activate) return;
        if (other.collider.CompareTag("Player"))
        {
            GetRandomReward(other.gameObject);
            ApplyState(RandomSupplyState.Dead);
        }
    }

    public void GetRandomReward(GameObject player)
    {
        var playerCom = player.GetComponent<RoleEntity>();
        if (level < 3)
        {
            int seed = Random.Range(0, 100);
            if (seed > 20)
            {
                GetHpReward(10, playerCom);
            }
            else
            {
                GetShieldReward(10, playerCom);
            }
        }
        else if (level < 6)
        {
            int seed = Random.Range(0, 100);
            if (seed > 50)
            {
                GetHpReward(50, playerCom);
            }
            else if (seed > 20)
            {
                GetShieldReward(50, playerCom);
            }
            else
            {
                UpgradeJump(playerCom);
            }
        }
        else if (level < 10)
        {

        }
    }

    public void GetHpReward(int count, RoleEntity player)
    {
        if (player.hp + count > player.maxHp)
        {
            player.hp = player.maxHp;
            return;
        }
        else
        {
            player.hp += count;
        }
        player.UpdateTxt();
        player.UpdateColor();
    }

    public void GetShieldReward(int count, RoleEntity player)
    {
        if (player.shield + count > player.maxShield)
        {
            player.shield = player.maxShield;
            return;
        }
        else
        {
            player.shield += count;
        }
        player.UpdateTxt();
        player.UpdateColor();
    }

    public void UpgradeJump(RoleEntity player)
    {
        if (player.maxJumpTime < 3)
        {
            player.maxJumpTime += 1;
        }
        else
        {
            GetHpReward(player.maxHp, player);
        }
    }

}
