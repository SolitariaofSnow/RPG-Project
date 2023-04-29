using System.Security.AccessControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BattleStateMachine : MonoBehaviour
{
    public enum PerformAction
    {
        WAITING,
        TAKEACTION,
        PERFORMACTION,
    }
    public PerformAction BattleStates;
    void Start()
    {
        BattleStates = PerformAction.WAITING;
    }

    
    void Update()
    {
        switch (BattleStates)
        {
            
           case(PerformAction.WAITING):

           break;

           case(PerformAction.TAKEACTION):

           break;

           case(PerformAction.PERFORMACTION):

           break;
        }
    }
}
