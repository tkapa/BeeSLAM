using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beer : MonoBehaviour {

    //Know how hard the player wants to throw and which player wants to throw it
    public Vector3 throwingVector;

    public Rigidbody2D rb;

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

        invulCount = Time.time + invulTime;
    }

    // Update is called once per frame
    void Update () {
                
        if(!isActive && Time.time > invulCount)
        {
            isActive = true;
            GetComponent<CircleCollider2D>().isTrigger = false;
        }
	}
}
