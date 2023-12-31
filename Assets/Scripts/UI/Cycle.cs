using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CycleState
{
    Pause,
    Teach,
    Easy,
    Normal,
    Hard,
    Hell,
    Finish,
    Over,
}

public class Cycle : MonoBehaviour
{
    public CycleState state;
    public Image timeBar;
    public Text stateText;

    public Text teachText;
    public Text tipText;
    bool isEnter;
    float time;
    float durationTime;
    float spawnCD;

    //FSM
    // * Teach
    bool isMove;
    bool isJump;

    public void Awake()
    {
        Apply_State(state);
    }

    public void Apply_State(CycleState state)
    {
        switch (state)
        {
            case CycleState.Teach:
                _Enter_Teach();
                break;
            case CycleState.Easy:
                _Enter_Easy();
                break;
            case CycleState.Normal:
                _Enter_Normal();
                break;
            case CycleState.Hard:
                _Enter_Hard();
                break;
            case CycleState.Hell:
                _Enter_Hell();
                break;
            case CycleState.Over:
                _Enter_Over();
                break;
            case CycleState.Finish:
                _Enter_Finish();
                break;
        }
    }

    void _Enter_Teach()
    {
        state = CycleState.Teach;
        isEnter = true;
    }

    void _Enter_Easy()
    {
        state = CycleState.Easy;
        isEnter = true;
    }

    void _Enter_Normal()
    {
        state = CycleState.Normal;
        isEnter = true;
    }

    void _Enter_Hard()
    {
        state = CycleState.Hard;
        isEnter = true;
    }

    void _Enter_Hell()
    {
        state = CycleState.Hell;
        isEnter = true;
    }

    void _Enter_Over()
    {
        state = CycleState.Over;
        isEnter = true;
    }

    void _Enter_Finish()
    {
        state = CycleState.Finish;
        isEnter = true;
    }

    public void Tick_State(float dt)
    {
        switch (state)
        {
            case CycleState.Teach:
                _Tick_Teach(dt);
                break;
            case CycleState.Easy:
                _Tick_Easy(dt);
                break;
            case CycleState.Normal:
                _Tick_Normal(dt);
                break;
            case CycleState.Hard:
                _Tick_Hard(dt);
                break;
            case CycleState.Hell:
                _Tick_Hell(dt);
                break;
            case CycleState.Over:
                GameOver();
                break;
            case CycleState.Finish:
                Victory();
                break;
        }
    }

    void GameOver()
    {
        tipText.gameObject.SetActive(true);
        tipText.text = "Game Over!";
        Time.timeScale = 0;
    }

    void Victory()
    {
        tipText.gameObject.SetActive(true);
        tipText.text = "Victory!";
        Time.timeScale = 0;
    }

    void _Tick_Teach(float dt)
    {
        if (isEnter)
        {
            isEnter = false;
            teachText.gameObject.SetActive(true);
            time = 0;
            durationTime = 10;
            stateText.text = "教学阶段";
        }

        if (!isMove)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                isMove = true;
                timeBar.fillAmount += 0.25f;
            }
        }

        if (!isJump)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isJump = true;
                timeBar.fillAmount += 0.25f;
            }
        }

        if (timeBar.fillAmount == 0.5f)
        {
            if (time < durationTime)
            {
                time += dt;
            }
            else
            {
                time = 0;
                durationTime = 5;
                timeBar.fillAmount += 0.25f;
            }
            teachText.text = "数值：<b>生命值或护盾值</b>，当<b>Hp</b>为0时，游戏结束。\n不同的角色颜色代表不同信息\n<b><color=green>绿色</color></b>：健康，<b><color=yellow>黄色</color></b>：正常。\n<b><color=cyan>青色</color></b>：无敌，<b><color=red>红色</color></b>：生命垂危。\n<b><color=blue>蓝色</color></b>：护盾，霸体,挡伤。";
        }

        if (timeBar.fillAmount == 0.75f)
        {
            if (time < durationTime)
            {
                time += dt;
            }
            else
            {
                timeBar.fillAmount += 0.25f;
            }
            teachText.text = "躲避障碍达到特定条件还有补给包供应！\n总之尽可能的躲避所有障碍，努力活下去吧！";
        }

        if (timeBar.fillAmount >= 1)
        {
            teachText.gameObject.SetActive(false);
            Apply_State(CycleState.Easy);
        }
    }

    void _Tick_Easy(float dt)
    {
        if (isEnter)
        {
            isEnter = false;
            time = 0;
            durationTime = 15;
            stateText.text = "简单阶段";
        }

        if (spawnCD <= 0)
        {
            spawnCD = 1;
            GameController.Instance.Apply_Easy_Single();
        }
        else
        {
            spawnCD -= dt;
        }

        if (time < durationTime)
        {
            time += dt;
            timeBar.fillAmount = time / durationTime;
        }
        else
        {
            Apply_State(CycleState.Normal);
        }
    }

    void _Tick_Normal(float dt)
    {
        if (isEnter)
        {
            isEnter = false;
            time = 0;
            durationTime = 20;
            stateText.text = "普通阶段";
        }

        if (spawnCD <= 0)
        {
            spawnCD = 3;
            GameController.Instance.Apply_Normal_Group();
        }
        else
        {
            spawnCD -= dt;
        }

        if (time < durationTime)
        {
            time += dt;
            timeBar.fillAmount = time / durationTime;
        }
        else
        {
            Apply_State(CycleState.Hard);
        }
    }

    void _Tick_Hard(float dt)
    {
        if (isEnter)
        {
            isEnter = false;
            time = 0;
            durationTime = 25;
            stateText.text = "困难阶段";
        }

        if (spawnCD <= 0)
        {
            spawnCD = 3;
            GameController.Instance.Apply_Hard_Group();
        }
        else
        {
            spawnCD -= dt;
        }

        if (time < durationTime)
        {
            time += dt;
            timeBar.fillAmount = time / durationTime;
        }
        else
        {
            Apply_State(CycleState.Hell);
        }
    }

    void _Tick_Hell(float dt)
    {
        if (isEnter)
        {
            isEnter = false;
            time = 0;
            durationTime = 30;
            stateText.text = "地狱阶段";
        }

        if (spawnCD <= 0)
        {
            spawnCD = 2;
            GameController.Instance.Apply_Hell();
        }
        else
        {
            spawnCD -= dt;
        }


        if (time < durationTime)
        {
            time += dt;
            timeBar.fillAmount = time / durationTime;
        }
        else
        {
            Apply_State(CycleState.Finish);
        }
    }

    public void Update()
    {
        Tick_State(Time.deltaTime);
    }
}
