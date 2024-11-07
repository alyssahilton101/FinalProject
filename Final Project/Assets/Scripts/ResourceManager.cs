using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Events;






public class ResourceManager : MonoBehaviour, IEventObserver
{
    //Responisble for handling all resources (food, money, HP, ect)
    //Should create events for the UI to see and update in response and should NOT update the UI itself (observer pattern)
    //Will start off with some public methods for updating resources, but might swap to an event system 

    //TODO:
    // -Do something about running out of stuff
    // -Update UI/figure out how to code events
    //https://gamedevbeginner.com/events-and-delegates-in-unity/

    //Setting up all resources
    private int food;
    private int money;
    private int shipHP;
    private int crewMorale;

    //Adding some booleans
    bool isOutOfFood = false;

    //UI Events
    public UnityEvent<int> onMoneyUpdate;
    public UnityEvent<int> onShipHPUpdate;
    public UnityEvent<int> onCrewMoraleUpdate;
    public UnityEvent<int> onFoodUpdate;

    //Gameover Events
    public UnityEvent onCrewHPZero;
    public UnityEvent onShipHPZero;
    public UnityEvent onCrewMoraleZero;

    //Event Manager
    EventManager eventManager;



    void Start()
    {
        eventManager = FindObjectOfType<EventManager>();
        eventManager.RegisterObserver(this);

        //Temp: Giving resources starting amounts. Will update later
        food = 500;
        money = 500;
        crewMorale = 100;
        shipHP = 100;
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    //Food
    public void setFood(int amount) {
        int temp = food;

        //Check to see if adding the food will make food negtive 
        if ((temp + amount) >= 0)
        {
           //If amount stays positive, add the food 
            food += amount;
            isOutOfFood = false;
        }
        else
        {
           //Set food to 0 and the isOutOfFood bool to true
            food = 0;
            isOutOfFood = true;
        }
        onFoodUpdate.Invoke(food);

    }

    public int getFood() { 
        return food;
    }

    //Money
    public void setMoney(int amount)
    {
        int temp = money;

        //Check to see if adding the amount will make it negtive 
        if ((temp + amount) >= 0)
        {
            //If amount stays positive, add the food 
            money += amount;
        }
        else
        {
            //Set money to 0 
            money = 0;
        }
        onMoneyUpdate.Invoke(money);

    }

    public int getMoney() { 
        return money;
    }

    //Ship HP

    public void setShipHP(int amount)
    {
        int temp = shipHP;

        //If adding the amount will keep the HP between 0 and 100, add the amount
        if (temp + amount >= 0 && temp + amount <= 100)
        {
            shipHP += amount;
        }
        //If the amount will make the HP go below 0, set the HP to 0
        else if (temp + amount < 0)
        {
            shipHP = 0;
            onShipHPZero.Invoke();
        }
        //If the amount will make the HP go above 100, set the HP to 100
        else if (temp + amount > 100) { shipHP = 100; }

        onShipHPUpdate.Invoke(shipHP);
    }

    public int getShipHP()
    {
        return shipHP;
    }




    // Crew Morale

    //Ship HP

    public void setCrewMorale(int amount)
    {
        int temp = crewMorale;

        //If adding the amount will keep the morale between 0 and 100, add the amount
        if (temp + amount >= 0 && temp + amount <= 100)
        {
            crewMorale += amount;
        }
        //If the amount will make the morale go below 0, set the HP to 0
        else if (temp + amount < 0)
        {
            crewMorale = 0;
            onCrewMoraleZero.Invoke();
        }
        //If the amount will make the HP go above 100, set the HP to 100
        else if (temp + amount > 100) { crewMorale = 100; }

        onCrewMoraleUpdate.Invoke(crewMorale);
    }

    public int getCrewMorale()
    {
        return crewMorale;
    }

   

    public void OnEventTriggered(EventData eventData)
    {
        //Check to see if the event has any effects
        if (eventData.effects.Count > 0)
        {
            //Loop through the effects and apply them
            foreach (Tuple<string, int> effect in eventData.effects)
            {
                switch (effect.Item1)
                {
                    case "food":
                        setFood(effect.Item2);
                        break;
                    case "money":
                        setMoney(effect.Item2);
                        break;
                    case "shipHP":
                        setShipHP(effect.Item2);
                        break;
                    case "crewMorale":
                        setCrewMorale(effect.Item2);
                        break;
                    default:
                        break;
                }
            }
        }
    }

}
