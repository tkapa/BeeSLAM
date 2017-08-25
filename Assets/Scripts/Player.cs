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
        EPS_Parrying,
        EPS_Ducking
    }

    [Tooltip("Number for player")]
    public Player_Number playerNumber;

    [HideInInspector]
    public Player_State playerState = Player_State.EPS_Standing;

    [HideInInspector]
    public Rigidbody2D rb;

    public Transform arm;

    public GameObject[] anims;

    //Inputs for this player
    public string throwInput, dodgeInput;

    public float jumpingForce = 1500.0f;

    //Am I meant to be able to move
    //(is true for now, make false when testing)
    bool takingInput = false;

    //Manages the player's throwing and when the strength increases 
    public Vector2 throwThresholds = new Vector2(0.4f, 0.8f);
    private float throwHoldTime = 0.0f;

    public Vector2[] lowThrowStrength, medThrowStrength, highThrowStrength;

    //Manages the player's count to jump or dodge
    public Vector2 dodgeThresholds = new Vector2(0.4f, 0.8f);
    private float distToGround;
    private float dodgeTime;
    private float jumpTime = 0.5f;

    //Beer can gameObject
    public GameObject beerCan;
    Vector2 startPos;

    // Use this for initialization
    void Start () {
        startPos = this.transform.position;
        //Check for setup errors
        if (!GetComponent<Rigidbody2D>())
            Debug.LogError(gameObject.name + " does not contain a rigidbody2D!");
        else rb = GetComponent<Rigidbody2D>();

        //Listen for round events
        EventManager.instance.OnBeginRound.AddListener(() => {
            takingInput = true;
        });

        EventManager.instance.OnPlayerDeath.AddListener((v, p) => {
            takingInput = false;
        });

        EventManager.instance.OnEndRound.AddListener((b) => {
            takingInput = false;
        });
    }
	
	// Update is called once per frame
	void Update () {

        //find arm on stand obj
        if (playerState == Player_State.EPS_Standing)
        {
            arm = transform.Find("Stand/arm").transform;
            distToGround = transform.Find("Stand").GetComponent<Collider2D>().bounds.extents.y;
        }
        //find arm on ducking obj
        else if (playerState == Player_State.EPS_Ducking)
        {
            arm = transform.Find("Duck/arm").transform;
            distToGround = transform.Find("Duck").GetComponent<Collider2D>().bounds.extents.y;
        }
           

        //If input is being accepted
        if (takingInput)
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
            Throw();

            //Reset the hold time
            throwHoldTime = 0.0f;
        }

        //When the dodge button is presssed
        if (Input.GetKey(dodgeInput))
        {

            dodgeTime += timeDelta;

            //Player ducks
            if (playerState == Player_State.EPS_Standing)
                Duck();

        }
        else if (Input.GetKeyUp(dodgeInput))
        {
            //Dodge,jump
            if (playerState == Player_State.EPS_Ducking)
                Jump();

            //Reset dodge Time
            dodgeTime = 0;
        }

        //Jump only after this time is up
        if (playerState == Player_State.EPS_Jumping)
            jumpTime -= timeDelta;
    }

    //Called when the player is hit
    public void OnDeath()
    {
        if (playerNumber == Player_Number.EPN_1)
            EventManager.instance.OnEndRound.Invoke(1);
        else
            EventManager.instance.OnEndRound.Invoke(0);
    }

    //Throw logic here
    void Throw()
    {
        if (throwHoldTime > throwThresholds.y)
        {

            switch (playerState)
            {
                //Throw for standing
                case Player_State.EPS_Standing:
                    GameObject b = Instantiate(beerCan, arm.position, arm.rotation) as GameObject;
                    b.GetComponent<Rigidbody2D>().AddForce(highThrowStrength[0]);
                    Destroy(b, 4.0f);
                    break;

                //Throw for jumping
                case Player_State.EPS_Jumping:
                    GameObject g = Instantiate(beerCan, arm.position, arm.rotation) as GameObject;
                    g.GetComponent<Rigidbody2D>().AddForce(lowThrowStrength[1]);
                    Destroy(g, 4.0f);
                    break;
            }
        }
        else if (throwHoldTime > throwThresholds.x && throwHoldTime < throwThresholds.y)
        {
            switch (playerState)
            {
                //Throw for standing
                case Player_State.EPS_Standing:
                    GameObject b = Instantiate(beerCan, arm.position, arm.rotation) as GameObject;
                    b.GetComponent<Rigidbody2D>().AddForce(medThrowStrength[0]);
                    Destroy(b, 4.0f);
                    break;

                //Throw for jumping
                case Player_State.EPS_Jumping:
                    GameObject g = Instantiate(beerCan, arm.position, arm.rotation) as GameObject;
                    g.GetComponent<Rigidbody2D>().AddForce(lowThrowStrength[1]);
                    Destroy(g, 4.0f);
                    break;
            }
        }
        else if (throwHoldTime < throwThresholds.x)
        {
            switch (playerState)
            {
                //Throw for standing
                case Player_State.EPS_Standing:
                    GameObject b = Instantiate(beerCan, arm.position, arm.rotation) as GameObject;
                    b.GetComponent<Rigidbody2D>().AddForce(lowThrowStrength[0]);
                    Destroy(b, 4.0f);
                    break;

                //Throw for jumping
                case Player_State.EPS_Jumping:
                    GameObject g = Instantiate(beerCan, arm.position, arm.rotation) as GameObject;
                    g.GetComponent<Rigidbody2D>().AddForce(lowThrowStrength[1]);
                    Destroy(g, 4.0f);
                    break;
            }
        }
    }

    //Duck logic here
    void Duck()
    {
        playerState = Player_State.EPS_Ducking;

        //Change player Sprites here
        anims[1].SetActive(true);
        anims[0].SetActive(false);
    }

    //Called when the player wants to jump
    void Jump()
    {
        //Change player Sprites here
        anims[1].SetActive(false);
        anims[0].SetActive(true);

        rb.AddForce(new Vector2(0, jumpingForce));
        playerState = Player_State.EPS_Jumping;
  
    }

    //Reset the player's position to the origin
    public void ResetPosition()
    {
        playerState = Player_State.EPS_Standing;

        //Change player Sprites here - Make sure they are standing
        anims[1].SetActive(false);
        anims[0].SetActive(true);
        //Change character height back to normal
        transform.position = startPos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Upon collision with the ground
        if (collision.gameObject.tag == "ground" && playerState == Player_State.EPS_Jumping)
        {
            //Check time
            if (jumpTime <= 0){
                //print("collided with grouund");

                playerState = Player_State.EPS_Standing;
                jumpTime = 0.5f;
            }
        }
           
        //Upon collision with a beer can
        if (collision.gameObject.tag == "beer" && takingInput)
        {
            EventManager.instance.OnPlayerDeath.Invoke(collision.contacts[0].point, this);
        }
    
    }
}
