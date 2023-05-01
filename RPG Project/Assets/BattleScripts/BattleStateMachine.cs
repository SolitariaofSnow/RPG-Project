using System;
using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

[System.Serializable]
public class BattleStateMachine : MonoBehaviour
{

    internal class EnemyButtonSelect
{
    public GameObject EnemyPrefab;
    public GameObject BattleManager;
    void start()
    {
     this.BattleManager = GameObject.Find("BattleManager");
    }
    
    public void SelectEnemy(){
        this.BattleManager.GetComponent<BattleStateMachine> ();
         }
}

       void EnemyButton()
    {
        foreach(GameObject enemy in EnemiesInBattle)
        {
            GameObject newButton = Instantiate(EnemyButtons) as GameObject;
            EnemyButtonSelect button = newButton.GetComponent<EnemyButtonSelect> ();

            EnemyStateMachine cur_enemy = enemy.GetComponent<EnemyStateMachine> ();

            Text buttonText = newButton.transform.Find("Text").gameObject.GetComponent<Text>();
            buttonText.text = cur_enemy.enemy.name;

            button.EnemyPrefab = enemy;
            newButton.transform.SetParent (Spacer);

            

        }
    } 
     void Start()
    {
        BattleStates = PerformAction.WAIT;
        EnemyButton();
        EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        HeroesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));

    }




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
  

    public enum HeroGUI
    {
        ACTIVATE, //activates UI
        WAITING, // waiting for input
        INPUT1, //basic attack
        INPUT2, //selected enemy
        DONE


    }

    public HeroGUI HeroInput; 
    public List<GameObject> HerosToManage = new List<GameObject> ();
    private HandleTurn HeroChoice;
    public GameObject EnemyButtons;
    public Transform Spacer;
    
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

