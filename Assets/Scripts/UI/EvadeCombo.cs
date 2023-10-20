using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EvadeLevel
{
    Beginner,   //新手
    Novice,     //初学者
    Intermediate,   //中级
    Expert,     //专家
    Master      //宗师
}

public class EvadeCombo : MonoBehaviour
{
    EvadeLevel state;
    public Image progressBar;
    public Text stateText;

    bool isEnter;
    int evadeCount;
    int evadeComboCount;
    int rewardComboCount;
    int levelCount;

    public void Awake()
    {
        Apply_State(EvadeLevel.Beginner);
    }

    public void Apply_State(EvadeLevel state)
    {
        switch (state)
        {
            case EvadeLevel.Beginner:
                _Enter_Beginner();
                break;
            case EvadeLevel.Novice:
                _Enter_Novice();
                break;
            case EvadeLevel.Intermediate:
                _Enter_Intermediate();
                break;
            case EvadeLevel.Expert:
                _Enter_Expert();
                break;
            case EvadeLevel.Master:
                _Enter_Master();
                break;
        }
    }

    void _Enter_Beginner()
    {
        state = EvadeLevel.Beginner;
        isEnter = true;
    }

    void _Enter_Novice()
    {
        state = EvadeLevel.Novice;
        isEnter = true;
    }

    void _Enter_Intermediate()
    {
        state = EvadeLevel.Intermediate;
        isEnter = true;
    }

    void _Enter_Expert()
    {
        state = EvadeLevel.Expert;
        isEnter = true;
    }

    void _Enter_Master()
    {
        state = EvadeLevel.Master;
        isEnter = true;
    }

    public void Tick_State(float dt)
    {
        switch (state)
        {
            case EvadeLevel.Beginner:
                _Tick_Teach(dt);
                break;
            case EvadeLevel.Novice:
                _Tick_Novice(dt);
                break;
            case EvadeLevel.Intermediate:
                _Tick_Intermediate(dt);
                break;
            case EvadeLevel.Expert:
                _Tick_Expert(dt);
                break;
            case EvadeLevel.Master:
                _Tick_Master(dt);
                break;
        }
    }

    void _Tick_Teach(float dt)
    {
        if (isEnter)
        {
            isEnter = false;
            levelCount = 9;
            rewardComboCount = 3;
            stateText.text = $"破烂补给包({evadeCount}/{rewardComboCount})";
        }

        if (evadeCount < rewardComboCount)
        {
            progressBar.fillAmount = evadeCount / rewardComboCount;
        }
        else
        {
            evadeCount -= rewardComboCount;
            //生成奖励包
        }

        if (evadeComboCount >= levelCount)
        {
            Apply_State(EvadeLevel.Novice);
        }

    }

    void _Tick_Novice(float dt)
    {
        if (isEnter)
        {
            isEnter = false;
            levelCount = 30;
            rewardComboCount = 5;
            stateText.text = $"小补给包({evadeCount}/{rewardComboCount})";
        }

        if (evadeCount < rewardComboCount)
        {
            progressBar.fillAmount = evadeCount / rewardComboCount;
        }
        else
        {
            evadeCount -= rewardComboCount;
            //生成奖励包
        }

        if (evadeComboCount >= levelCount)
        {
            Apply_State(EvadeLevel.Intermediate);
        }

    }

    void _Tick_Intermediate(float dt)
    {
        if (isEnter)
        {
            isEnter = false;
            levelCount = 100;
            rewardComboCount = 8;
            stateText.text = $"中补给包({evadeCount}/{rewardComboCount})";
        }

        if (evadeCount < rewardComboCount)
        {
            progressBar.fillAmount = evadeCount / rewardComboCount;
        }
        else
        {
            evadeCount -= rewardComboCount;
            //生成奖励包
        }

        if (evadeComboCount >= levelCount)
        {
            Apply_State(EvadeLevel.Expert);
        }
    }

    void _Tick_Expert(float dt)
    {
        if (isEnter)
        {
            isEnter = false;
            levelCount = 300;
            rewardComboCount = 10;
            stateText.text = $"大补给包({evadeCount}/{rewardComboCount})";
        }

        if (evadeCount < rewardComboCount)
        {
            progressBar.fillAmount = evadeCount / rewardComboCount;
        }
        else
        {
            evadeCount -= rewardComboCount;
            //生成奖励包
        }

        if (evadeComboCount >= levelCount)
        {
            Apply_State(EvadeLevel.Master);
        }
    }

    void _Tick_Master(float dt)
    {
        if (isEnter)
        {
            isEnter = false;
            rewardComboCount = 10;
            stateText.text = $"豪华补给包({evadeCount}/{rewardComboCount})";
        }

        if (evadeCount < rewardComboCount)
        {
            progressBar.fillAmount = evadeCount / rewardComboCount;
        }
        else
        {
            evadeCount -= rewardComboCount;
            //生成奖励包
        }
    }

    public void Update()
    {
        Tick_State(Time.deltaTime);
    }
}
