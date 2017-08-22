using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //Used to define which number player this is
    public enum Player_Number{
        EPN_1 = 0,
        EPN_2
    }

    //Tracks the state of the player
    public enum Player_State
    {
        EPS_Standing,
        EPS_Jumping,
        EPS_Dodging,
        EPS_Parrying
    }

    public Player_Number playerNumber;

    [HideInInspector]
    public Player_State playerState = Player_State.EPS_Standing;

    [HideInInspector]
    public Rigidbody2D rb;

    //Inputs for this player
    public string throwInput, dodgeInput;

    //Am I meant to be able to move
    //(is true for now, make false when testing)
    bool takingInput = true;


    // Use this for initialization
    void Start () {
        //Check for setup errors
        if (GetComponent<Rigidbody2D>())
            Debug.LogError(gameObject.name + " does not contain a rigidbody2D!");

        if (playerNumber == 0)
            Debug.LogError(gameObject.name + " has not been assigned a player number!");

        //Listen for round events
        EventManager.instance.OnBeginRound.AddListener(() => {
            takingInput = true;
        });

        EventManager.instance.OnEndRound.AddListener((b) => {
            takingInput = false;
        });
    }
	
	// Update is called once per frame
	void Update () {
        //If input is being accepted
        if (takingInput)
            PollInput(Time.deltaTime);
	}

    //Called to take input from the player
    void PollInput(float timeDelta)
    {

    }

    //Called when the player is hit
    public void OnDeath()
    {
        if (playerNumber == Player_Number.EPN_1)
            EventManager.instance.OnEndRound.Invoke(1);
        else
            EventManager.instance.OnEndRound.Invoke(0);
    }
}
