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
                    UpdateProgressBar ();
            break;

            case(TurnState.CHOOSEACTION):
                ChooseAction ();
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
        HandleTurn myAttack = new HandleTurn ();
        myAttack.Name = this.enemy.name;
        myAttack.Attacker = this.gameObject;
        myAttack.Defender = BSM.HeroesInBattle[Random.Range(0, BSM.HeroesInBattle.Count)];
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
        //wait
        //do damage
        //animate return to start 
        //remove performer from BSM list as to not attack twice
        //reset BSM
        actionStarted = false;
        //reset this enemy state 
        cur_cooldown = 0f;
        CurrentState = TurnState.PROCESSING;
    }
}
