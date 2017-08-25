using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //Variables for countin rounds
    private int maximumWins = 4;
    private int currentRound = 0;

    //Keeping track of each player's round wins
    public int playerOneWins = 0;
    public int playerTwoWins = 0;

    //Keep track of the round's time
    public float maximumRoundTime = 20.0f;
    public float currentRoundTime;

    //Keep track of times between rounds
    public float roundResetTime = 4.0f;
    public float roundResetCounter;

    //Bools used to check whether or not players can move and are actively playing
    bool isActive;
    bool isPlaying = false;
    bool gameManagerActive = false;
    float timer;

	// Use this for initialization
	void Start () {
        //Listening for important events
        EventManager.instance.OnStartGame.AddListener(()=>{

            //When the game starts, clear the slate for the game to begin
            currentRound = playerOneWins = playerTwoWins = 0;
            roundResetCounter = roundResetTime;
            currentRoundTime = maximumRoundTime;
            isActive = false;
            isPlaying = false;
            gameManagerActive = true;
        });

        EventManager.instance.OnBeginRound.AddListener(()=>{
            //When the round beginss, start counting down the round time
            ++currentRound;
            currentRoundTime = maximumRoundTime;
            isActive = false;
            isPlaying = true;
            timer = Time.time + 10;
        });

        EventManager.instance.OnEndRound.AddListener((i)=> {

            //When the round ends run a check and reset for the next round
            CheckWins(i);
            roundResetCounter = roundResetTime;
            isPlaying = false;
        });
        EventManager.instance.OnEndGame.AddListener(() => {
            gameManagerActive = false;
        });
    }
	
	// Update is called once per frame
	void Update () {
        if (gameManagerActive) {
            RoundTimer();

            if (Input.anyKeyDown)
                isActive = true;
        }        
    }

    //Used to count time during and between rounds
    void RoundTimer()
    {
        //If in between rounds, then count down to start the round
        if (!isPlaying)
        {
            roundResetCounter -= Time.deltaTime;

            if (roundResetCounter < 0)
                EventManager.instance.OnBeginRound.Invoke();
        }
        //If the game is in a round
        else if (isPlaying)
        {
            currentRoundTime -= Time.deltaTime;

            if (currentRoundTime < 0)
                EventManager.instance.OnEndRound.Invoke(2);

            //Check whether or not people are playing
            if (Time.time > timer && !isActive)
                EventManager.instance.OnEndGame.Invoke();
        }
    }

    //Called to add to a player's wins
    void CheckWins(int i)
    {
        //Add to a player's wins
        switch (i) {
            //Player one wins
            case 0:
                ++playerOneWins;
                break;

            //Player two wins
            case 1:
                ++playerTwoWins;
                break;

            case 2:
                ++playerOneWins;
                ++playerTwoWins;
                break;
        }

        if (playerOneWins >= maximumWins || playerTwoWins >= maximumWins)
        {
            //If either player has reached the maximum number of wins, finish the game.
            EventManager.instance.OnEndGame.Invoke();
        }
    }
}
