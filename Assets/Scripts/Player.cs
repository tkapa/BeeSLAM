using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public string throwInput;
    public string dodgeInput;

    public GameObject beerCan;

    // Use this for initialization
    void Start () {
		
	}

	// Update is called once per frame
	void Update () {
        print("Throw: " + throwInput + " Dodge: " + dodgeInput);
	}
}
