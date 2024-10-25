using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    //Singleton
    //Should manage game state + coordinates transitions between different parts of the game (travel, events, rest, etc.).

    //Important values
    private int daysPassed;
    private int distanceRemaining;
    private Pace currentPace;
    private GameState gameState;
    bool isGameWon = false;

    //Enums for game state and pace
    public enum Pace { Normal, Slow, Fast };
    enum GameState { Travel, Stopped, Event, GameOver };

    //UI Events for updating key values
    public UnityEvent<int> onDaysPassedUpdate;
    public UnityEvent<int> onDistanceRemainingUpdate;
    public UnityEvent<Pace> onPaceUpdate;


    // Start is called before the first frame update
    void Start()
    {
        //Temp: Giving starting values
        daysPassed = 0;
        distanceRemaining = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        //Testing UI
        if (Input.GetKeyDown(KeyCode.Space))
        {
            setDaysPassed(1);
            setDistanceRemaining(100);
        }
    }

    //Contains the logic for traveling
    public void travel()
    {
        if (gameState == GameState.Travel) { 
            //logic
        }
    }

    //Contains the logic for stopping
    public void stopped()
    {
        if (gameState == GameState.Stopped)
        {
            //logic
        }
    }
    //Contains the logic for events
    public void eventPresent()
    {
        if (gameState == GameState.Event)
        {
            //logic
        }
    }

    //Contains the logic for game over
    public void gameOver()
    {
        if (gameState == GameState.GameOver)
        {
            //logic
        }
    }

    //Sets the pace and updates the UI
    public void setPace(Pace pace) {
        currentPace = pace;
        onPaceUpdate.Invoke(currentPace);
    }

    //Sets the distance remaining and updates the UI
    public void setDistanceRemaining(int amount)
    {
        int temp = distanceRemaining;

        //Check to see if subtracting the distance will make distance negtive
        if ((temp - amount) >= 0)
        {
            //If distance stays positive, add the distance
            distanceRemaining -= amount;
        }
        else
        {
            //Set distance to 0 and declare the game over
            distanceRemaining = 0;
            isGameWon = true;
            //TODO: Call game over function
        }

        onDistanceRemainingUpdate.Invoke(distanceRemaining);
    }

    //Sets the days passed and updates the UI
    public void setDaysPassed(int amount)
    {
        daysPassed += amount;
        onDaysPassedUpdate.Invoke(daysPassed);
    }

}
