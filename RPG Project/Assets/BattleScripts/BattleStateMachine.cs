using System;
using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class BattleStateMachine : MonoBehaviour
{


    public class EnemyTargetButtons : ScriptableObject {
        private class EnemyButtonSelect : MonoBehaviour {
            public void init(GameObject enemy, BattleStateMachine battleManager, Transform spacer) {
                this.EnemyParent = enemy;
                this.BattleManager = battleManager;

                EnemyStateMachine cur_enemy = enemy.GetComponent<EnemyStateMachine> ();

                GameObject textGO = new GameObject();
                textGO.transform.SetParent(spacer);
                textGO.AddComponent<TextMeshProUGUI>();

                this.buttonText = textGO.GetComponent<TextMeshProUGUI>();
                this.buttonText.text = cur_enemy.enemy.name;
                // TODO: Set font, size
            }

            public void SelectEnemy() { /* todo */ }
            public GameObject EnemyParent;
            public BattleStateMachine BattleManager;
            public TextMeshProUGUI buttonText;
        }
        /* ^^^^^ END OF ENEMY BUTTON SELECT  ^^^^^ */
        public void Awake() {
            // Load the Arial font from the Unity Resources folder.
            //arial = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        }

        public void Add(GameObject enemy, BattleStateMachine battleManager, Transform spacer) {
            GameObject newGO = new GameObject();
            EnemyButtonSelect newEBS = newGO.AddComponent<EnemyButtonSelect>() as EnemyButtonSelect;
            newGO.transform.Translate(Vector3.down * 50 * this.Buttons.Count);
            newEBS.init(enemy, battleManager, spacer);
            this.Buttons.Add(new EnemyButton(newEBS, newGO));
        }

        public void Update() { /* TODO: logic for actually navigating/selecting */ }

        private struct EnemyButton {
            public EnemyButton(EnemyButtonSelect b, GameObject g) {
                Button = b;
                gameObject = g;
            }
            public EnemyButtonSelect Button { get; }
            public GameObject gameObject { get; }
        }
        private List<EnemyButton> Buttons = new List<EnemyButton>();
        //private Font arial; // TODO: Load font elsewhere
    }

    void CreateEnemyButtons()
    {
        this.EnemyButtons = ScriptableObject.CreateInstance<EnemyTargetButtons>();
        foreach(GameObject enemy in EnemiesInBattle)
        {
            EnemyButtons.Add(enemy, this, Spacer);
        }
    }
    void Start()
    {
        BattleStates = PerformAction.WAIT;
        EnemiesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        HeroesInBattle.AddRange(GameObject.FindGameObjectsWithTag("Hero"));

        CreateEnemyButtons();
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
    public EnemyTargetButtons EnemyButtons;
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

