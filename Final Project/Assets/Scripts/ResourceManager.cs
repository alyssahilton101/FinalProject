using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Events;






public class ResourceManager : MonoBehaviour
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
    private int crewHP;

    //Adding some booleans
    bool isOutOfFood = false;

    //UI Events
   
    public UnityEvent onCrewHPUpdate;
    public UnityEvent onMoneyUpdate;
    public UnityEvent onShipHPUpdate;
    public UnityEvent onCrewMoraleUpdate;
    public UnityEvent<int> onFoodUpdate;

    //Gameover Events
    public UnityEvent onCrewHPZero;
    public UnityEvent onShipHPZero;
    public UnityEvent onCrewMoraleZero;

   

    void Start()
    {
        //Temp: Giving resources starting amounts. Will update later
        food = 500;
        money = 500;
        crewMorale = 100;
        crewHP = 100;
        shipHP = 100;
    }

    // Update is called once per frame
    void Update()
    {
        addFood(1);
        Debug.Log("added food");
    }


    //Food
    public void addFood(int amount) {
        food += amount;
        onFoodUpdate.Invoke(food);
    }

    public void removeFood(int amount) {
        int temp = food;
        
        if ((temp - amount) >= 0)
        {
            food -= amount;
        }
        else
        {
            food = 0;
            isOutOfFood = true;
        }
    }

    public int getFood() { 
        return food;
    }

    //Money
    public void addMoney(int amount) {
        money += amount;
    }
    public void removeMoney(int amount) {
       int temp = money;
        
        if ((temp - amount) >= 0)
        {
            money -= amount;
        }
        else { 
            money = 0;
        }
    }
    
    public int getMoney() { 
        return money;
    }

    //Ship HP
    public void addShipHP(int amount)
    {
        int temp = shipHP;

        if ((temp + amount) <= 100)
        {
            shipHP += amount;
        }
        else
        {
            shipHP = 100; // Ship is at full HP
        }
    }

    public void removeShipHP(int amount)
    {
        int temp = shipHP;

        if ((temp - amount) >= 0)
        {
            shipHP -= amount;
        }
        else
        {
            shipHP = 0; // Ship is fully damaged
            onShipHPZero.Invoke();
        }
    }

    public int getShipHP()
    {
        return shipHP;
    }

    // Crew Morale
    public void addCrewMorale(int amount)
    {
        int temp = crewMorale;

        if ((temp + amount) <= 100)
        {
            crewMorale += amount;
        }
        else
        {
            crewMorale = 100; // Crew is at full morale
        }
    }

    public void removeCrewMorale(int amount)
    {
        int temp = crewMorale;

        if ((temp - amount) >= 0)
        {
            crewMorale -= amount;
        }
        else
        {
            crewMorale = 0; // Morale is at its lowest point
            onCrewMoraleZero.Invoke();
        }
    }

    public int getCrewMorale()
    {
        return crewMorale;
    }

    // Crew HP
    public void addCrewHP(int amount)
    {
        int temp = crewHP;

        if ((temp + amount) <= 100)
        {
            crewHP += amount;
        }
        else
        {
            crewHP = 100; 
        }
    }

    public void removeCrewHP(int amount)
    {
        int temp = crewHP;

        if ((temp - amount) >= 0)
        {
            crewHP -= amount;
        }
        else
        {
            crewHP = 0; // Crew has no health left
            onCrewHPZero.Invoke();
        }
    }

    public int getCrewHP()
    {
        return crewHP;
    }

}
