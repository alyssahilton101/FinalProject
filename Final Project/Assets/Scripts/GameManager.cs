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
    bool isGameOver = false;
    int foodConsumedPerDay = 10;
    int distanceTraveledPerDay = 1;
    private float currentTimer = 3;

    //Enums for game state and pace
    public enum Pace { Normal, Slow, Fast };
    enum GameState { Travel, Stopped };

    //UI Events for updating key values
    public UnityEvent<int> onDaysPassedUpdate;
    public UnityEvent<int> onDistanceRemainingUpdate;
    public UnityEvent<Pace> onPaceUpdate;
    public UnityEvent<int> onFoodUpdate;
    public UnityEvent<string> onGameOver;
    public UnityEvent<string, string> onEvent;

    //Might not be needed but left them here for now
    public UnityEvent<int> onShipHPUpdate;
    public UnityEvent<int> onMoneyUpdate;
    public UnityEvent<int> onCrewMoraleUpdate;
    public UnityEvent<int> onCrewHPUpdate;

    //EventManager
    EventManager eventManager;

    //Ship sprite and key values
    [SerializeField] GameObject ship;
    [SerializeField] GameObject shipStartPosition;
    Vector3 shipEndPosition;
    Vector3 island1Position;
    Vector3 island2Position;
    Vector3 island3Position;

    

    // Start is called before the first frame update
    void Start()
    {
        //Temp: Giving starting values
        daysPassed = 0;
        distanceRemaining = 75;

        //Setting up event manager (possibly temporary)
        eventManager = FindObjectOfType<EventManager>();

        //Setting up the ship
        ship.transform.position = shipStartPosition.transform.position;

        

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
            currentTimer = 1;
            
        }
        if (gameState == GameState.Stopped) {
            //Stuff here someday
        }


    }

    //Contains the logic for traveling
    public void travel()
    {
        if (distanceRemaining != 0) { 
            setDistanceRemaining(distanceTraveledPerDay);
            onFoodUpdate.Invoke(-foodConsumedPerDay);
            setDaysPassed(1);
            ship.transform.position = new Vector3(ship.transform.position.x + 0.1f, ship.transform.position.y, 0);
            //other logic will go here
            //Mostly, it'll be checking to see if there should be an event

        }
        if (distanceRemaining == 16 || distanceRemaining == 37 || distanceRemaining == 58)
        {
            //trigger island event
            eventManager.TriggerIslandEvent();

        }
        if(distanceRemaining % 10 == 0)
        {
            //trigger random event
            eventManager.TriggerRandomEvent();
        }
        if (distanceRemaining == 0)
        {
            isGameWon = true;
            gameOver("Against all odds, ye’ve carved yer own legend on the high seas. Riches and renown be yers, Captain—may this be the first of many victories!");
        }

    }

    //Contains the logic for stopping
    public void stopped()
    {
        if (gameState == GameState.Stopped)
        {
            
        }
    }
 

    //Contains the logic for game over
    public void gameOver(string message)
    {
        isGameOver = true;
        if (isGameWon)
        {
            gameState = GameState.Stopped;
            onEvent.Invoke("You won!", message);
        }
        else {
         
            gameState = GameState.Stopped;
            onEvent.Invoke("Game Over", message); 
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
            gameOver("Congraulations, you did it! Yipee!");
        }

        onDistanceRemainingUpdate.Invoke(distanceRemaining);
    }

    //Sets the days passed and updates the UI
    public void setDaysPassed(int amount)
    {
        daysPassed += amount;
        onDaysPassedUpdate.Invoke(daysPassed);
    }

    public void swapState() {

        if (gameState == GameState.Travel)
        {
            gameState = GameState.Stopped;
        }
        else if(!isGameOver)
        {
            gameState = GameState.Travel;
        }

    }

   
}
