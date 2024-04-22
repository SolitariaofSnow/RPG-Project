using System.Timers;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStateMachine : Fighter {
    public BaseHero hero;

    public override void Start()
    {
        base.Start();
        type = "Hero";
    }

    public override void UpdateProgressBar()
    {
        cur_cooldown = cur_cooldown + Time.deltaTime;
        float calc_cooldown = cur_cooldown / max_cooldown;
        ProgressBar.transform.localScale = new Vector3(Mathf.Clamp(calc_cooldown, 0, 1), ProgressBar.transform.localScale.y, ProgressBar.transform.localScale.z);
        if (cur_cooldown >= max_cooldown)
        {
            CurrentState = TurnState.ADDTOLIST;
        }
    }
    public override void Update()
    {
        switch(CurrentState)
        {
            case(TurnState.PROCESSING):
                if(BSM.BattleStates == BattleStateMachine.PerformAction.WAIT) UpdateProgressBar();
                break;

            case(TurnState.ADDTOLIST):
                //BSM.HerosToManage.Add(this.gameObject); // TODO: idklol
                BSM.CollectActions(new HandleTurn(gameObject.name, type, gameObject));
                CurrentState = TurnState.WAITING;
                break;

            case(TurnState.WAITING):
                break;

            case(TurnState.SELECTING):
                break;

            case(TurnState.ACTION):
                cur_cooldown = 0f;
                float calc_cooldown = cur_cooldown / max_cooldown;
                ProgressBar.transform.localScale = new Vector3(Mathf.Clamp(calc_cooldown, 0, 1), ProgressBar.transform.localScale.y, ProgressBar.transform.localScale.z);
                StartCoroutine(TimeForAction());
                break;

            case(TurnState.DEAD):
                break;

            default:
                throw new System.ArgumentException("Bad Hero State");
                break;
        };
    }
    
    
    }

