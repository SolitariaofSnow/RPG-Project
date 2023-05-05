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
    //this gameobject
    private Vector3 startposition;
    private bool actionStarted = false;
    public GameObject? Target = null;
    private float animSpeed = 5f;
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
                //BSM.HerosToManage.Add(this.gameObject); // TODO: idklol
                BSM.CollectActions(new HandleTurn(gameObject.name, "Hero", gameObject));
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
    private IEnumerator TimeForAction()
    {
        if(actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        //TODO:animations 
        //animate the enemy near the hero to attack
        Vector3 StartPosition = this.gameObject.transform.position;
        Vector3 TargetPosition = new Vector3(Target.transform.position.x-.25f, Target.transform.position.y,0f);
        while(MoveTowardsObject(TargetPosition)) {yield return null;}
        //wait
        //do damage
        //animate return to start 
        //remove performer from BSM list as to not attack twice
        while(MoveTowardsObject(StartPosition)) {yield return null;}
        //reset BSM
        BSM.ActionComplete();
        CurrentState = TurnState.PROCESSING;
        cur_cooldown = 0f;
        actionStarted = false;
        //reset this enemy state
        yield return null;

    }
    private bool MoveTowardsObject(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
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
