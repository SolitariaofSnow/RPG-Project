using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : Fighter {
    public BaseEnemy enemy;

    public override void Start() {
        base.Start();
        type = "Enemy";
    }
    void ChooseAction()
    {
        HandleTurn myAttack = new HandleTurn(
                this.enemy.name,
                type,
                this.gameObject,
                BSM.HeroesInBattle[Random.Range(0, BSM.HeroesInBattle.Count)]);
        BSM.CollectActions(myAttack);
    }
    // Update is called once per frame
    public override void Update()
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

    
}
