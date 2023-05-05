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

                TextGO = new GameObject();
                TextGO.transform.SetParent(spacer);
                TextGO.AddComponent<TextMeshProUGUI>();

                this.buttonText = TextGO.GetComponent<TextMeshProUGUI>();
                this.buttonText.text = cur_enemy.enemy.name;
                // TODO: Set font, size
            }
            public void Highlight() {
                buttonText.outlineWidth = 0.2f;
                buttonText.outlineColor = Color.yellow;
            }
            public void Unhighlight() {
                buttonText.outlineWidth = 0.0f;
            }

            public GameObject EnemyParent;
            public GameObject TextGO;
            public BattleStateMachine BattleManager;
            public TextMeshProUGUI buttonText;
        }
        /* ^^^^^ END OF ENEMY BUTTON SELECT  ^^^^^ */
        public void Activate() {
            if (this.Activity) return;
            index = 0;
            Buttons[index].Button.Highlight();
            this.Activity = true;
            this.CascadeActivity();
        }

        public void Add(GameObject enemy, Transform spacer) {
            GameObject newGO = new GameObject();
            newGO.SetActive(Activity);

            EnemyButtonSelect newEBS = newGO.AddComponent<EnemyButtonSelect>() as EnemyButtonSelect;
            newEBS.init(enemy, BSM, spacer);
            newEBS.TextGO.SetActive(Activity);

            this.Buttons.Add(new EnemyButton(newEBS, newGO));
        }

        public void Awake() {
            // Load the Arial font from the Unity Resources folder.
            //arial = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        }

        private void CascadeActivity() {
            foreach (EnemyButton eb in this.Buttons) {
                eb.gameObject.SetActive(this.Activity);
                eb.Button.TextGO.SetActive(this.Activity);
            }
        }

        public void Deactivate() {
            if (!this.Activity) return;
            Buttons[index].Button.Unhighlight();
            this.Activity = false;
            this.CascadeActivity();
        }

        public void Register(BattleStateMachine bsm) {
            this.BSM = bsm;
        }

        public void Update() {
            if (!this.Activity) return;
            if(Input.GetKeyDown(KeyCode.Keypad2)) {
                Buttons[index].Button.Unhighlight();
                index = (index + 1) % Buttons.Count;
                Buttons[index].Button.Highlight();
            } else if (Input.GetKeyDown(KeyCode.Keypad8)) {
                Buttons[index].Button.Unhighlight();
                index = (Buttons.Count + index - 1) % Buttons.Count;
                Buttons[index].Button.Highlight();
            } else if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
                GameObject target = Buttons[index].Button.EnemyParent;
                BSM.PerformList[0].Defender = target;
                BSM.ActionReady(target);
            }
        }

        private struct EnemyButton {
            public EnemyButton(EnemyButtonSelect b, GameObject g) {
                Button = b;
                gameObject = g;
            }
            public EnemyButtonSelect Button { get; }
            public GameObject gameObject { get; }
        }
        private BattleStateMachine BSM;
        private List<EnemyButton> Buttons = new List<EnemyButton>();
        private bool Activity = false;
        //private Font arial; // TODO: Load font elsewhere
        private int index = 0;
    }

    void CreateEnemyButtons()
    {
        this.EnemyButtons = ScriptableObject.CreateInstance<EnemyTargetButtons>();
        this.EnemyButtons.Register(this);
        foreach(GameObject enemy in EnemiesInBattle)
        {
            EnemyButtons.Add(enemy, Spacer);
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
        EnemyButtons.Update();
        switch (BattleStates)
        {
            case(PerformAction.WAIT):
                break;

            case(PerformAction.TAKEACTION):
                Assert.AreNotEqual(PerformList.Count, 0);
                GameObject performer = PerformList[0].Attacker;
                Assert.IsNotNull(performer);
                if(PerformList[0].Type == "Enemy")
                {
                    EnemyStateMachine ESM = performer.GetComponent<EnemyStateMachine>();
                    ESM.Target = PerformList[0].Defender;
                    ESM.CurrentState = EnemyStateMachine.TurnState.ACTION;
                }
                if(PerformList[0].Type == "Hero")
                {
                    HeroStateMachine HSM = performer.GetComponent<HeroStateMachine>();
                    if(HSM.CurrentState == HeroStateMachine.TurnState.WAITING) {
                        HSM.CurrentState = HeroStateMachine.TurnState.SELECTING;
                        EnemyButtons.Activate();
                    }
                }

                break;

            case(PerformAction.PERFORMACTION):

                break;
        }



    }


    public void CollectActions(HandleTurn input)
    {
        PerformList.Add(input);
        if(PerformList.Count > 0)
        {
            BattleStates = PerformAction.TAKEACTION;
        }
    }
    public void ActionComplete() {
        Assert.AreNotEqual(PerformList.Count, 0);
        PerformList.RemoveAt(0);
        if(PerformList.Count < 1) BattleStates = PerformAction.WAIT;
        else if(PerformList.Count > 0)
        {
            BattleStates = PerformAction.TAKEACTION;
        }
    }
    public void ActionReady(GameObject target) {
        GameObject performer = PerformList[0].Attacker;
        HeroStateMachine HSM = performer.GetComponent<HeroStateMachine>();
        HSM.CurrentState = HeroStateMachine.TurnState.ACTION;
        HSM.Target = target;
        EnemyButtons.Deactivate();
        HSM.CurrentState = HeroStateMachine.TurnState.ACTION;
    }
}

