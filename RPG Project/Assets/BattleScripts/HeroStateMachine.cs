using System.Timers;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroStateMachine : MonoBehaviour {

   public BaseHero hero;
   private BattleStateMachine BSM;
   public GameObject BattleManager;
   public enum TurnState
    {
        PROCESSING,
        ADDTOLIST,
        WAITING,
        SELECTING,
        ACTION,
        DEAD
    };

    public TurnState CurrentState;
    //for the progress bar 
    private float cur_cooldown = 0f;
    private float max_cooldown = 5f;
    public Image ProgressBar;
    void Start()
    {
        CurrentState = TurnState.PROCESSING;
        this.BattleManager = GameObject.Find("BattleManager");
        BSM = this.BattleManager.GetComponent<BattleStateMachine> ();
    }
    void Update()
    {
        switch(CurrentState)
        {
            case(TurnState.PROCESSING):
                if(BSM.BattleStates == BattleStateMachine.PerformAction.WAIT) UpdateProgressBar();
            break;

            case(TurnState.ADDTOLIST):
                    BSM.HerosToManage.Add(this.gameObject);
                    CurrentState = TurnState.WAITING;
            break;

            case(TurnState.WAITING):
                BSM.CollectActions(new HandleTurn(gameObject.name, "Hero", gameObject));
            break;

            case(TurnState.SELECTING):
            break;

            case(TurnState.ACTION):
                CurrentState = TurnState.PROCESSING;
            break;

            case(TurnState.DEAD):
            break;

            default:
                throw new System.ArgumentException("Bad Hero State");
            break;
        };
    }
    void UpdateProgressBar()
    {
        cur_cooldown = cur_cooldown + Time.deltaTime;
        float calc_cooldown = cur_cooldown / max_cooldown;
        ProgressBar.transform.localScale = new Vector3(Mathf.Clamp(calc_cooldown, 0, 1), ProgressBar.transform.localScale.y, ProgressBar.transform.localScale.z);
        if(cur_cooldown >= max_cooldown)
        {
            CurrentState = TurnState.ADDTOLIST;
        }
    }
}
