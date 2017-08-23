using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beer : MonoBehaviour {

    //Know how hard the player wants to throw and which player wants to throw it
    public Vector3 throwingVector;

    Rigidbody2D rb;

    bool isActive = false;

    //Time this object is invulnerable
    public float invulTime = 0.1f;
    float invulCount;

	// Use this for initialization
	void Start () {
        //If there's no Rigidbody
        if (!GetComponent<Rigidbody2D>())
            Debug.LogError("Beer prefab does not contain a RigidBody!");
        else
            rb = GetComponent<Rigidbody2D>();
	}

    //Called at a fixed rate
    private void FixedUpdate()
    {
        //Stay invulnerable until the timer is up 
        if (!isActive)
        {
            invulCount -= Time.deltaTime;

            if(invulCount < 0)
            {
                isActive = true;
                GetComponent<CircleCollider2D>().isTrigger = false;
            }
        }
    }

    // Update is called once per frame
    void Update () {
	}

    //When the beer hits a player
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            other.gameObject.GetComponent<Player>().OnDeath();
        }
    }
}
