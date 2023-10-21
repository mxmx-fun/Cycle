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

    public void Ctor(EvadeLevel level)
    {
        int lv = (int)level * 2;
        this.level = lv;
        this.transform.localScale = size + new Vector2(lv * 0.1f, lv * 0.1f);
        var x = Random.Range(-5, 5);
        float y = 4.75f;
        this.transform.position = new Vector3(x, y, 0);
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
        if (level < 2)
        {
            int seed = Random.Range(0, 100);
            if (seed > 20)
            {
                GetHpReward(10, playerCom);
                GameController.Instance.Tip("获得10点生命");
            }
            else
            {
                GetShieldReward(10, playerCom);
                GameController.Instance.Tip("获得10点护盾");
            }
        }
        else if (level < 4)
        {
            int seed = Random.Range(0, 100);
            if (seed > 50)
            {
                GetHpReward(50, playerCom);
                GameController.Instance.Tip("获得50点生命");
            }
            else if (seed > 20)
            {
                GetShieldReward(50, playerCom);
                GameController.Instance.Tip("获得50点护盾");
            }
            else
            {
                UpgradeJump(playerCom);
                GameController.Instance.Tip("跳跃最大次数+1");
            }
        }
        else if (level < 6)
        {
            int seed = Random.Range(0, 100);
            if (seed > 75)
            {
                if (playerCom.hp == playerCom.maxHp)
                {
                    GetShieldReward(playerCom.maxShield, playerCom);
                    GameController.Instance.Tip("增加满护盾！");
                }
                else
                {
                    GetHpReward(playerCom.maxHp, playerCom);
                    GameController.Instance.Tip("恢复全部生命！");
                }
            }
            else if (seed > 50)
            {
                GetShieldReward(playerCom.maxShield, playerCom);
                GameController.Instance.Tip("增加满护盾！");
            }
            else if (seed > 15)
            {
                playerCom.InvincibleDurationTime += 1;
                playerCom.ApplyState(RoleState.Invincible);
                GameController.Instance.Tip("获得无敌时间1s！");
            }
            else
            {
                playerCom.InvincibleDurationTime += 3;
                playerCom.ApplyState(RoleState.Invincible);
                GameController.Instance.Tip("获得无敌时间3s！");
            }
        }
        else if (level < 8)
        {
            int seed = Random.Range(0, 100);
            if (seed > 75)
            {
                ExpandMaxHp(25, playerCom);
                GameController.Instance.Tip("最大生命+25！");
            }
            else if (seed > 50)
            {
                ExpandMaxHp(50, playerCom);
                GameController.Instance.Tip("最大生命+50！");
            }
            else if (seed > 15)
            {
                playerCom.InvincibleDurationTime += 1;
                playerCom.ApplyState(RoleState.Invincible);
                GameController.Instance.Tip("获得无敌时间1s！");
            }
            else
            {
                playerCom.InvincibleDurationTime += 3;
                playerCom.ApplyState(RoleState.Invincible);
                GameController.Instance.Tip("获得无敌时间3s！");
            }
        }
        else if (level < 10)
        {
            int seed = Random.Range(0, 100);
            if (seed > 65)
            {
                playerCom.InvincibleDurationTime += 1;
                playerCom.ApplyState(RoleState.Invincible);
                GameController.Instance.Tip("获得无敌时间1s！");
            }
            else if (seed > 40)
            {
                playerCom.InvincibleDurationTime += 2;
                playerCom.ApplyState(RoleState.Invincible);
                GameController.Instance.Tip("获得无敌时间2s！");
            }
            else if (seed > 15)
            {
                playerCom.InvincibleDurationTime += 3;
                playerCom.ApplyState(RoleState.Invincible);
                GameController.Instance.Tip("获得无敌时间3s！");
            }
            else
            {
                playerCom.InvincibleDurationTime += 5;
                playerCom.ApplyState(RoleState.Invincible);
                GameController.Instance.Tip("获得无敌时间5s！");
            }
        }
    }

    public void GetHpReward(int count, RoleEntity player)
    {
        if (player.hp + count > player.maxHp)
        {
            player.hp = player.maxHp;
            player.UpdateTxt();
            player.UpdateColor();
            return;
        }
        else
        {
            player.hp += count;
        }
        player.UpdateTxt();
        player.UpdateColor();
    }

    public void ExpandMaxHp(int count, RoleEntity player)
    {
        player.maxHp += count;
        player.UpdateTxt();
        player.UpdateColor();
    }

    public void GetShieldReward(int count, RoleEntity player)
    {
        if (player.shield + count > player.maxShield)
        {
            player.shield = player.maxShield;
            player.UpdateTxt();
            player.UpdateColor();
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
            player.jumpTime += 1;
            player.maxJumpTime += 1;
        }
        else
        {
            GetHpReward(player.maxHp, player);
        }
    }

}
