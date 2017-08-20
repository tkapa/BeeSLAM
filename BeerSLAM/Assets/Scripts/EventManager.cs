using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Called then both players are ready to begin
public class StartGame : UnityEvent { }

//Called when the game ends, either forcibly or when a player wins
public class EndGame : UnityEvent { }

//Begin Round is called when the round should begin
public class BeginRound : UnityEvent { }

//End round iss called when the round ends, passing an int for a switch case
//Player One win = 0 Player two = 1, Draw = 2
public class EndRound : UnityEvent<int> { }

public class EventManager : MonoBehaviour {

    /// <summary>
    /// Event manager singleton that can help manage the core game state
    /// </summary>
    private static EventManager inst;
    public static EventManager instance
    {
        get
        {
            if (inst == null)
            {
                var newEventManager = new GameObject("EventManager");
                inst = newEventManager.AddComponent<EventManager>();
            }

            return inst;
        }
    }

    //Events
    public StartGame OnStartGame = new StartGame();
    public EndGame OnEndGame = new EndGame();
    public BeginRound OnBeginRound = new BeginRound();
    public EndRound OnEndRound = new EndRound();

    private void Awake()
    {
        if (inst != null)
        {
            DestroyImmediate(this);
            return;
        }

        inst = this;
    }
}
