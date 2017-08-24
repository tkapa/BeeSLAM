using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    
    //Button and Text for each Player
    public Button[] playerOneButtons = new Button[2];
    public Text playerOneReadyText;

    public Button[] playerTwoButtons = new Button[2];
    public Text playerTwoReadyText;

    //used to tell whether or not both players are ready
    private bool isPlayerOneReady = false;
    private bool isPlayerTwoReady = false;

    public GameObject image;

	// Use this for initialization
	void Start () {
        EventManager.instance.OnEndGame.AddListener(()=>{
            //Listening for the end game thingo
            image.SetActive(true);
        });
	}
	
	// Update is called once per frame
	void Update () {
        TakeInput();
        StartGame();
	}

    //Does something with the input for both players
    void TakeInput()
    {
        //If player one is holding down their buttons
        if (Input.GetKey("a") && Input.GetKey("s"))
        {
            playerOneReadyText.text = "Ready!";
            isPlayerOneReady = true;
        } else if(Input.GetKeyUp("a") || Input.GetKeyUp("s"))
        {
            playerOneReadyText.text = "Not Ready!";
            isPlayerOneReady = false;
        }

        //If player two is holding down their buttons
        if (Input.GetKey("k") && Input.GetKey("l"))
        {
            playerTwoReadyText.text = "Ready!";
            isPlayerTwoReady = true;
        }
        else if (Input.GetKeyUp("k") || Input.GetKeyUp("l"))
        {
            playerTwoReadyText.text = "Not Ready!";
            isPlayerTwoReady = false;
        }

        //Allow for muting of Music (if any)
        if (Input.GetKey("m"))
        {

        }
    }

    //Should the game begin?
    void StartGame()
    {
        //If both players are ready start the game
        if(isPlayerOneReady && isPlayerTwoReady)
        {
            image.SetActive(false);
            EventManager.instance.OnStartGame.Invoke();
        }
    }
}
