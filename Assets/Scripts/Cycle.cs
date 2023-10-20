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
    Finish
}

public class Cycle : MonoBehaviour
{
    CycleState state;
    public Image timeBar;
    public Text stateText;

    bool isEnter;
    float time;
    float durationTime;

    public void Awake()
    {
        Apply_State(CycleState.Teach);
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
        }
    }

    void _Tick_Teach(float dt) {
        if(isEnter) {
            isEnter = false;
            time = 0;
            durationTime = 10;
            stateText.text = "教学阶段";
        }

        if(time < durationTime) {
            time += dt;
            timeBar.fillAmount = time / durationTime;
        } else {
            Apply_State(CycleState.Easy);
        }
    }

    void _Tick_Easy(float dt) {
        if(isEnter) {
            isEnter = false;
            time = 0;
            durationTime = 15;
            stateText.text = "简单阶段";
        }

        if(time < durationTime) {
            time += dt;
            timeBar.fillAmount = time / durationTime;
        } else {
            Apply_State(CycleState.Normal);
        }
    }

    void _Tick_Normal(float dt) {
        if(isEnter) {
            isEnter = false;
            time = 0;
            durationTime = 20;
            stateText.text = "普通阶段";
        }

        if(time < durationTime) {
            time += dt;
            timeBar.fillAmount = time / durationTime;
        } else {
            Apply_State(CycleState.Hard);
        }
    }

    void _Tick_Hard(float dt) {
        if(isEnter) {
            isEnter = false;
            time = 0;
            durationTime = 25;
            stateText.text = "困难阶段";
        }

        if(time < durationTime) {
            time += dt;
            timeBar.fillAmount = time / durationTime;
        } else {
            Apply_State(CycleState.Hell);
        }
    }

    void _Tick_Hell(float dt) {
        if(isEnter) {
            isEnter = false;
            time = 0;
            durationTime = 30;
            stateText.text = "地狱阶段";
        }

        if(time < durationTime) {
            time += dt;
            timeBar.fillAmount = time / durationTime;
        } else {
            Apply_State(CycleState.Finish);
        }
    }

    public void Update()
    {
        Tick_State(Time.deltaTime);
    }
}
