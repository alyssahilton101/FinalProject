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
    int foodConsumedPerDay = 10;
    int distanceTraveledPerDay = 100;
    private float currentTimer = 3;

    //Enums for game state and pace
    public enum Pace { Normal, Slow, Fast };
    enum GameState { Travel, Stopped, Event, GameOver };

    //UI Events for updating key values
    public UnityEvent<int> onDaysPassedUpdate;
    public UnityEvent<int> onDistanceRemainingUpdate;
    public UnityEvent<Pace> onPaceUpdate;
    public UnityEvent<int> onFoodUpdate;
    public UnityEvent<string> onGameOver;

    //Might not be needed but left them here for now
    public UnityEvent<int> onShipHPUpdate;
    public UnityEvent<int> onMoneyUpdate;
    public UnityEvent<int> onCrewMoraleUpdate;
    public UnityEvent<int> onCrewHPUpdate;


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
     

        if (gameState == GameState.Travel) {
            if (currentTimer > 0)
            {
                currentTimer -= Time.deltaTime;
                return;
            }
            travel();
            currentTimer = 3;
        }
    }

    //Contains the logic for traveling
    public void travel()
    {
        if (distanceRemaining != 0) { 
            setDistanceRemaining(distanceTraveledPerDay);
            onFoodUpdate.Invoke(-foodConsumedPerDay);
            setDaysPassed(1);
            //other logic will go here
            //Mostly, it'll be checking to see if there should be an event
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
        if (isGameWon)
        {
            onGameOver.Invoke("You win!");
        }
        else { onGameOver.Invoke("Game Over"); }
       
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
            gameOver();
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
