using System;
using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public List<HandleTurn> PerformList = new List<HandleTurn> ();
    public List<GameObject> HeroesInBattle = new List<GameObject> (); 
    public List<GameObject> EnemiesInBattle = new List<GameObject> ();


    void Start()
    {
        BattleStates = PerformAction.WAIT;
        EnemiesInBattle.AddRange (GameObject.FindGameObjectsWithTag("Enemy"));
        HeroesInBattle.AddRange (GameObject.FindGameObjectsWithTag("Hero"));
    }

    
    void Update()
    {
        switch (BattleStates)
        {
            
           case(PerformAction.WAIT):

           break;

           case(PerformAction.TAKEACTION):

           break;

           case(PerformAction.PERFORMACTION):

           break;
        }
  }
     public void CollectActions(HandleTurn input)
        {
            PerformList.Add(input); 
        }
}


