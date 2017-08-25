using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour {

    public GameObject beerCollisionSystem;
    public GameObject playerCollisionSystem;

    bool slowTime = false;

    float slowTimeSeconds = 2;
    float slowTimeCounter;

    Player pl;

	// Use this for initialization
	void Start () {
        EventManager.instance.OnPlayerDeath.AddListener(PlayerCollision);

	}
	
	// Update is called once per frame
	void Update () {
        if (slowTime)
            TimeSlow();

	}

    //Called when Beers collide with each other
    public void BeerCollision(Vector2 position)
    {
        GameObject b = Instantiate(beerCollisionSystem, position, transform.rotation) as GameObject;
        Destroy(b, 0.5f);
    }

    //Called when Beer collides with the player
    public void PlayerCollision(Vector2 position, Player player)
    {
        pl = player;
        GameObject p = Instantiate(beerCollisionSystem, position, transform.rotation) as GameObject;
        Destroy(p, 0.5f);
        slowTimeCounter = slowTimeSeconds;
        Time.timeScale = 0.5f;
        slowTime = true;
    }

    void TimeSlow()
    {
        slowTimeCounter -= Time.deltaTime;

        if(slowTimeCounter < 0)
        {
            Time.timeScale = 1;
            slowTime = false;
            pl.OnDeath();
        }
    }
}
