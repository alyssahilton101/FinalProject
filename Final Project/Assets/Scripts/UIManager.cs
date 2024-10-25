using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //Handles the logic for showing and hiding UI panels, updating HUD elements, and managing user inputs.
    //Should listen to a bunch of stuff and update accordingly given an event (probably)

    [SerializeField] TextMeshProUGUI food;
    [SerializeField] TextMeshProUGUI money;
   // [SerializeField] TextMeshProUGUI crewMorale;
    //[SerializeField] TextMeshProUGUI crewHP;
    [SerializeField] TextMeshProUGUI shipHP;
    [SerializeField] TextMeshProUGUI date;
    [SerializeField] TextMeshProUGUI pace;
    [SerializeField] TextMeshProUGUI distance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateFood(int amount) {
        food.text = "Food: " + amount;
    }

    public void updateMoney(int amount)
    {
        money.text = "Money: " + amount;
    }


    public void updateShipHP(int amount)
    {
        shipHP.text = "Ship HP: " + amount;
    }

    public void updateDate(int amount)
    {
        //TODO: Make this a date
        date.text = "Days Past: " + amount;
    }

    public void updatePace(string speed)
    {
        pace.text = "Pace: " + speed;
    }

    public void updateDistance(int amount)
    {
        distance.text = "Distance: " + amount;
    }

}
