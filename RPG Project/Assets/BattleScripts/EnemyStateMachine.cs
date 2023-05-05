using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour {
    private BattleStateMachine BSM; 
    public BaseEnemy enemy;

    public enum TurnState
    {
        PROCESSING,
        CHOOSEACTION,
        WAITING,
        ACTION,
        DEAD
    };

    public TurnState CurrentState; 
    //for the progress bar 
    private float cur_cooldown = 0f;
    private float max_cooldown = 5f;
    //this gameobject
    private Vector3 startposition;
    private bool actionStarted = false;
    public GameObject Target;
    private float animSpeed = 5f;
    void Start()
    {
        CurrentState = TurnState.PROCESSING; 
        BSM = GameObject.Find("BattleManager").GetComponent<BattleStateMachine> ();
        startposition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch(CurrentState)
        {
            case(TurnState.PROCESSING):
                if(BSM.BattleStates == BattleStateMachine.PerformAction.WAIT) UpdateProgressBar ();
                break;

            case(TurnState.CHOOSEACTION):
                ChooseAction();
                CurrentState = TurnState.WAITING;
                break;

            case(TurnState.WAITING):

                break;

            case(TurnState.ACTION):
                StartCoroutine(TimeForAction());
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
        if(cur_cooldown >= max_cooldown)
        {
            CurrentState = TurnState.CHOOSEACTION;
        }
    }

    void ChooseAction()
    {
        HandleTurn myAttack = new HandleTurn(
                this.enemy.name,
                "Enemy",
                this.gameObject,
                BSM.HeroesInBattle[Random.Range(0, BSM.HeroesInBattle.Count)]);
        BSM.CollectActions (myAttack);
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
        Vector3 TargetPosition = new Vector3(Target.transform.position.x+.25f, Target.transform.position.y,0f);
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
