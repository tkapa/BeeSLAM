using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceManager : MonoBehaviour {

    //Objects for UI elements
    public GameObject mainMenu;
    public GameObject gameUI;

    //Text for the game
    public Text roundTimerText;
    public GameObject WinRoundText;
    public GameObject roundCountdownText;
    public Slider[] sliders;
    public Text[] wins;
    public float speed = 5;
    bool mainMenuShowing = true;

    GameManager gm;
    int loser;
    int result;
    bool takeAway = false;
    float healthValue = 100.0f;
    int player1wins = 0, player2wins = 0;
    // Use this for initialization
    void Start()
    {
        //Set up the required things
        gm = GetComponent<GameManager>();
        EventManager.instance.OnStartGame.AddListener(SwitchUI);
        EventManager.instance.OnEndGame.AddListener(SwitchUI);
        EventManager.instance.OnBeginRound.AddListener(()=> {
            roundCountdownText.SetActive(false);
            WinRoundText.SetActive(false);
        });
        EventManager.instance.OnEndRound.AddListener((b) => {
            roundCountdownText.SetActive(true);

            //Reset hp bars
            foreach (Slider s in sliders)
            {
                s.GetComponent<Slider>().value = 100;
            }

            takeAway = false;
            //Displays score for player 1
            if (b == 0)
            {
                player1wins++;
                wins[b].text = player1wins.ToString();
            }
            //Displays score for player 2
            else if(b == 1)
            {
                player2wins++;
                wins[b].text = player2wins.ToString();
            }
            //Displays a draw
            else
            {
                WinRoundText.SetActive(true);
                WinRoundText.GetComponent<Text>().text = "Draw";
            }

        });

        EventManager.instance.OnPlayerDeath.AddListener((v ,p) => {
            healthValue = 100.0f;           

            //Takes away hp
            loser = (int)p.playerNumber;

            //Gets the players number
            if (loser == 0)
                result = 2;
            else
                result = 1;
            takeAway = true;

            //Displays who has won
            WinRoundText.GetComponent<Text>().text = "Player " + result.ToString() + " Wins";
            WinRoundText.SetActive(true);
        });
    }

    // Update is called once per frame
    void Update()
    {
        //Cast timer to an Int and change the text
        int crt = (int)gm.currentRoundTime;
        roundTimerText.text = crt.ToString();

        int rrt = (int)gm.roundResetCounter;
        roundCountdownText.GetComponent<Text>().text = rrt.ToString();

        if (takeAway)
            UpdateHP(loser);

        
        /*if (Input.GetKeyDown("v"))
            SwitchUI();*/
    }

    //Switch the status of the UI
    void SwitchUI()
    {
        mainMenuShowing = !mainMenuShowing;
        mainMenu.SetActive(mainMenuShowing);
        gameUI.SetActive(!mainMenuShowing);
    }

    void UpdateHP(int playerW)
    {
        healthValue -= 10;

        if(playerW != 2)
        sliders[playerW].GetComponent<Slider>().value = healthValue;
    }
}
