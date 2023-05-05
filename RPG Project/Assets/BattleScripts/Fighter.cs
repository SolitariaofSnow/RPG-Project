using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fighter : MonoBehaviour {

    protected BattleStateMachine BSM;
    public GameObject BattleManager;
    public TurnState CurrentState;
    protected string type = "";
    //for the progress bar 
    protected float cur_cooldown = 0f;
    protected float max_cooldown = 5f;
    public Image ProgressBar;
    //this gameobject
    protected Vector3 startposition;
    protected bool actionStarted = false;
    public GameObject? Target = null;
    protected float animSpeed = 5f;

    public enum TurnState
    {
        PROCESSING,
        CHOOSEACTION,
        ADDTOLIST,
        WAITING,
        SELECTING,
        ACTION,
        DEAD
   };

    public virtual void Start()
    {
        this.CurrentState = TurnState.PROCESSING;
        this.BattleManager = GameObject.Find("BattleManager");
        BSM = this.BattleManager.GetComponent<BattleStateMachine> ();
        startposition = transform.position;
    }

    public virtual void Update() {
        if(BSM.BattleStates == BattleStateMachine.PerformAction.WAIT)
            UpdateProgressBar();
    }

    public virtual void UpdateProgressBar()
    {
        cur_cooldown = cur_cooldown + Time.deltaTime;
        if(cur_cooldown >= max_cooldown)
        {
            CurrentState = TurnState.CHOOSEACTION;
        }
    }

    protected IEnumerator TimeForAction()
    {
        if(actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        //TODO:animations 
        //animate the enemy near the hero to attack
        Vector3 StartPosition = this.gameObject.transform.position;
        float offset = (type == "Hero") ? -.25f : .25f;
        Vector3 TargetPosition = new Vector3(Target.transform.position.x + offset, Target.transform.position.y,0f);
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

}
