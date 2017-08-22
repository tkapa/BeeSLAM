using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oPlayer : MonoBehaviour {

    public string throwInput;
    public string dodgeInput;

    public GameObject beer;

    private GameObject empty;
    private Transform arm;

    private float downTime, pressButton, beerDeath = 3;
    private float maxCoolDown = 3;

    private bool grounded = true;

    private Vector2 oldpos;
    private float temp;

    private float jumpBackForce = 300, jumpUpForce = 300,dodgeBack = 60, dodgeUp = 100;

    // Use this for initialization
    void Start () {
        //used to swing arm? maybe?
        arm = this.gameObject.transform.Find("arm");

        //Create some empty gameObjects
        empty = new GameObject(this.name + "_Beer");
    }

    // Update is called once per frame
    void Update()
    {

        //
        GetKey(Time.deltaTime);

    }


    //Checks for player input
    private string GetKey(float d)
    {

        if (Input.GetKeyDown(throwInput))
        {
            downTime = Time.time;
            return "Throw";
        }
        else if(Input.GetKeyUp(throwInput))
        {
            pressButton = Time.time;
            float temp = pressButton - downTime;

            //Create Beer Obj
            GameObject beerTemp;
            beerTemp = Instantiate(beer,arm,false);
            beerTemp.transform.parent = empty.transform;
            beerTemp.name = "beer";

            //Quick hack
            if (temp > 1.5)
                temp = 1.5f;

            if (this.gameObject.name == "Player1")
                beerTemp.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(temp * jumpBackForce, temp * jumpUpForce));
            else
                beerTemp.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-(temp * jumpBackForce),( temp * jumpUpForce)));

            //Destroy after a time
            Destroy(beerTemp, beerDeath);

            return "Throw: " + temp.ToString();
        }

        if (Input.GetKeyDown(dodgeInput))
        {
            downTime = Time.time;
            
            //Ground Check
            if (this.transform.position.y > -2.5)
                grounded = false;
            else
                grounded = true;

            return "Dodge";
        }
        else if(Input.GetKeyUp(dodgeInput))
        {
            pressButton = Time.time;
            temp = pressButton - downTime;

            //Dodging
            if (temp > 0.5)
            {
                StartCoroutine(Dodge(maxCoolDown));
                print(this.transform.position.x + " "+ oldpos.x);
                oldpos = this.transform.position;
            }
            //Jumping
            else if (temp < 0.5 && grounded)
                this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpUpForce));

            return "Dodge: " + temp.ToString();
        }

        //nothing being pressed 
        return null;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        //Make this nice one day...
        if(other.gameObject.transform.parent.name == "Player2_Beer" && this.gameObject.name != "Player2")
        {
            print("Player 2 has won");
            EventManager.instance.OnEndRound.Invoke(1);
        }
        else if(other.gameObject.transform.parent.name == "Player1_Beer" && this.gameObject.name != "Player1")
        {
            print("Player 1 has won");
            EventManager.instance.OnEndRound.Invoke(0);
        }
    }

    //Dodge
    private IEnumerator Dodge(float coolDown = 3)
    {
        if (this.gameObject.name == "Player1")
            this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-dodgeBack, dodgeUp));
        else
            this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(dodgeBack, dodgeUp));
        yield return new WaitForSeconds(coolDown);
    }

}
