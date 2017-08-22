using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //Variables for countin rounds
    private int maximumWins = 4;
    private int currentRound = 0;

    //Keeping track of each player's round wins
    int playerOneWins = 0;
    int playerTwoWins = 0;

    //Keep track of the round's time
    public float maximumRoundTime = 20.0f;
    float currentRoundTime;

    //Keep track of times between rounds
    public float roundResetTime = 4.0f;
    float roundResetCounter;

    //Bools used to check whether or not players can move and are actively playing
    bool isActive;
    bool isPlaying = false;
    bool isGameManagerOnline = false;

	// Use this for initialization
	void Start () {
        //Listening for important events
        EventManager.instance.OnStartGame.AddListener(()=>{

            //When the game starts, clear the slate for the game to begin
            currentRound = playerOneWins = playerTwoWins = 0;
            roundResetCounter = roundResetTime * 2;
            currentRoundTime = maximumRoundTime;
            isActive = false;
            isPlaying = false;
            isGameManagerOnline = true;
        });

        EventManager.instance.OnBeginRound.AddListener(()=>{
            //When the round beginss, start counting down the round time
            ++currentRound;
            currentRoundTime = maximumRoundTime;
            isActive = false;
            isPlaying = true;
        });

        EventManager.instance.OnEndRound.AddListener((i)=> {

            //When the round ends run a check and reset for the next round
            InactivityCheck();
            GameEndCheck();
            roundResetCounter = roundResetTime;
            isPlaying = false;
        });

        EventManager.instance.OnEndGame.AddListener(() =>   {
            //Do things when the game ends
            isGameManagerOnline = false;
        });
    }
	
	// Update is called once per frame
	void Update () {

        if (isGameManagerOnline)
        {
            //Check if anyone is playing the game
            if (Input.GetKeyDown("a") || Input.GetKeyDown("l"))
            {
                if (!isActive)
                    isActive = true;
            }

            RoundTimer();
            //print("Round Countdown: " + roundResetCounter + " CurrentRoundTime: " + currentRoundTime + " " + isActive);
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
                EventManager.instance.OnEndRound.Invoke(3);
        }
    }

    //Used to check if no one is playing the game
    void InactivityCheck()
    {
        //If there is still no activity, end the game
        if (!isActive)
            EventManager.instance.OnEndGame.Invoke();
    }

    //Used to check whether or not the game is done
    void GameEndCheck()
    {
        if(playerOneWins == maximumWins || playerTwoWins == maximumWins)
        {
            //If either player has reached the maximum number of wins, finish the game.
            EventManager.instance.OnEndGame.Invoke();
        }
    }
}
