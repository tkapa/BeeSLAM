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
    public GameObject roundCountdownText;

    bool mainMenuShowing = true;

    GameManager gm;

    // Use this for initialization
    void Start()
    {
        //Set up the required things
        gm = GetComponent<GameManager>();
        EventManager.instance.OnStartGame.AddListener(SwitchUI);
        EventManager.instance.OnEndGame.AddListener(SwitchUI);
        EventManager.instance.OnBeginRound.AddListener(()=> {
            roundCountdownText.SetActive(false);
        });
        EventManager.instance.OnEndRound.AddListener((b) => {
            roundCountdownText.SetActive(true);
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

        /*if (Input.GetKeyDown("v"))
            SwitchUI();*/
    }

    //Switch the status of the UI
    void SwitchUI()
    {
        mainMenuShowing = !mainMenuShowing;
        mainMenu.SetActive(mainMenuShowing);
        gameUI.SetActive(!mainMenuShowing);
        print("Cuck'd");
    }
}
