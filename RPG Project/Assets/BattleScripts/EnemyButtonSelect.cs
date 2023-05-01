using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyButtonSelect : MonoBehaviour
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