using System;
using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public class BattleStateMachine : MonoBehaviour
{
    public enum PerformAction
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION,
    }
    public PerformAction BattleStates;

    public List<HandleTurn> PerformList = new List<HandleTurn>();
    public List<GameObject> HeroesInBattle = new List<GameObject>();
    public List<GameObject> EnemiesInBattle = new List<GameObject>();


    void Start()
    {
        BattleStates = PerformAction.WAIT;
        EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        HeroesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));
    }

    void Update()
    {
        switch (BattleStates)
        {
            case(PerformAction.WAIT):
                if(PerformList.Count > 0)
                {
                    BattleStates = PerformAction.TAKEACTION;
                }
                break;

            case(PerformAction.TAKEACTION):
                Assert.AreNotEqual(PerformList.Count, 0);
                GameObject performer = PerformList[0].Attacker;
                Assert.IsNotNull(performer);
                if(PerformList[0].Type == "Enemy")
                {
                    EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();
                    ESM.HeroToAttack = PerformList[0].Defender;
                    ESM.CurrentState = EnemyStateMachine.TurnState.ACTION;
                }
                if(PerformList[0].Type == "Hero")
                {

                }

                break;

            case(PerformAction.PERFORMACTION):

                break;
        }
    }
    public void CollectActions(HandleTurn input)
    {
        PerformList.Add(input);
    }
    public void ActionComplete() {
        Assert.AreNotEqual(PerformList.Count, 0);
        PerformList.RemoveAt(0);
        if(PerformList.Count < 1) BattleStates = PerformAction.WAIT;
    }
}


