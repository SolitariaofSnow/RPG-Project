using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class HandleTurn{
    public HandleTurn(string Attacker, string Type, GameObject AttackerGameObject, GameObject? DefenderGameObject = null)
    {
        Name = Attacker;
        this.Type = Type;
        this.Attacker = AttackerGameObject;
        this.Defender = DefenderGameObject;
    }
    public string Name; //name of attacker
    public string Type;
    public GameObject Attacker;//who attacks
    public GameObject? Defender;//who defends
    //which attack is performed
}
