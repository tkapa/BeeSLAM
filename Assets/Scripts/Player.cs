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

    [Tooltip("Number for player")]
    public Player_Number playerNumber;

    [HideInInspector]
    public Player_State playerState = Player_State.EPS_Standing;

    [HideInInspector]
    public Rigidbody2D rb;

    Transform arm;

    //Inputs for this player
    public string throwInput, dodgeInput;

    public float jumpingForce = 1500.0f;

    //Am I meant to be able to move
    //(is true for now, make false when testing)
    bool takingInput = true;

    //Manages the player's throwing and when the strength increases 
    public Vector2 throwThresholds = new Vector2(0.4f, 0.8f);
    private float throwHoldTime = 0.0f;

    //Manages the player's count to jump or dodge
    public float doubleTapTime = 0.1f;
    private float dodgeTime;

    //Beer can gameObject
    public GameObject beerCan;

    // Use this for initialization
    void Start () {
        //Check for setup errors
        if (!GetComponent<Rigidbody2D>())
            Debug.LogError(gameObject.name + " does not contain a rigidbody2D!");
        else rb = GetComponent<Rigidbody2D>();

        //Listen for round events
        EventManager.instance.OnBeginRound.AddListener(() => {
            takingInput = true;
        });

        EventManager.instance.OnEndRound.AddListener((b) => {
            takingInput = false;
        });

        arm = GetComponentInChildren<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        //If input is being accepted
        //if (takingInput)
            PollInput(Time.deltaTime);
	}

    //Called to take input from the player
    void PollInput(float timeDelta)
    {
        //While holding down the throw button
        if (Input.GetKey(throwInput))
        {
            throwHoldTime += timeDelta;

            //Ensure to provide feedback to the player
        }
        //When the player releases the throw button
        else if (Input.GetKeyUp(throwInput))
        {
            print(throwHoldTime);

            Throw();

            //Reset the hold time
            throwHoldTime = 0.0f;
        }

        //When the dodge button is presssed
        if (Input.GetKeyDown(dodgeInput))
        {
            //For now just jump
            if(playerState == Player_State.EPS_Standing)
                Jump();
        }
    }

    //Called when the player is hit
    public void OnDeath()
    {
        if (playerNumber == Player_Number.EPN_1)
            EventManager.instance.OnEndRound.Invoke(1);
        else
            EventManager.instance.OnEndRound.Invoke(0);
    }

    void Throw()
    {
        //Switch case for throwing
        switch (playerState)
        {
            //Throw for standing
            case Player_State.EPS_Standing:
                if (throwHoldTime > throwThresholds.y)
                {

                }
                else if (throwHoldTime > throwThresholds.x && throwHoldTime < throwThresholds.y)
                {

                }
                else if(throwHoldTime < throwThresholds.x)
                {

                }

                Instantiate(beerCan, arm.position, arm.rotation);
                break;

            //Throw for jumping
            case Player_State.EPS_Jumping:
                if (throwHoldTime > throwThresholds.y)
                {

                }
                else if (throwHoldTime > throwThresholds.x && throwHoldTime < throwThresholds.y)
                {

                }
                else if (throwHoldTime < throwThresholds.x)
                {

                }
                Instantiate(beerCan, arm.position, arm.rotation);
                break;
        }
    }

    //Called when the player wants to jump
    void Jump()
    {
        //Set to jumping and start to jump
        playerState = Player_State.EPS_Jumping;

        rb.AddForce(new Vector2(0, jumpingForce));
    }

    //Coroutine begins wheen the player dodges
    IEnumerator Dodge()
    {
        //Player jumps backwards
        //Player is set to dodging state while jumping backwards
        //Player jumps forwasrds again
        //Player is set to Parry state during this time
        //Player returns to origin position and is set to standing again
        yield return null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground" && playerState == Player_State.EPS_Jumping)
            playerState = Player_State.EPS_Standing;
    }
}
