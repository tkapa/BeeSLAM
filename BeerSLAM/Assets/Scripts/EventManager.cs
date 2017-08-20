using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Called then both players are ready to begin
public class StartGame : UnityEvent { }

//Begin Round is called when the round should begin
public class BeginRound : UnityEvent { }

//End round iss called when the round ends, passing a bool to know which player wins
public class EndRound : UnityEvent<bool> { }

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
