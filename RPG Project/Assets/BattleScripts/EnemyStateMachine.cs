using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour {

    public BaseEnemy enemy;

     public enum TurnState
    {
        PROCESSING,
        ADDTOLIST,
        WAITING,
        SELECTING,
        ACTION,
        DEAD
    };

    public TurnState CurrentState; 
    //for the progress bar 
    private float cur_cooldown = 0f;
    private float max_cooldown = 5f;

    void Start()
    {
        CurrentState = TurnState.PROCESSING; 
    }

    // Update is called once per frame
    void Update()
    {
         switch(CurrentState)
        {
            case(TurnState.PROCESSING):
                    UpgradeProgressBar ();
            break;

            case(TurnState.ADDTOLIST):

            break;

            case(TurnState.WAITING):

            break;

            case(TurnState.SELECTING):

            break;

            case(TurnState.ACTION):

            break;

            case(TurnState.DEAD):

            break;

            default:
                throw new System.ArgumentException("Bad Hero State");
            break;
        
        };
    }
     void UpgradeProgressBar()
    {
        cur_cooldown = cur_cooldown + Time.deltaTime;
        if(cur_cooldown >= max_cooldown)
        {
            CurrentState = TurnState.ADDTOLIST; 
        }
    }

}
