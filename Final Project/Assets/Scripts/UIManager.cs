using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IEventObserver
{
    //Handles the logic for showing and hiding UI panels, updating HUD elements, and managing user inputs.
    //Should listen to a bunch of stuff and update accordingly given an event (probably)

    [SerializeField] TextMeshProUGUI food;
    [SerializeField] TextMeshProUGUI money;
   [SerializeField] TextMeshProUGUI crewMorale;
    [SerializeField] TextMeshProUGUI shipHP;
    [SerializeField] TextMeshProUGUI distance;
    [SerializeField] GameObject eventBox;
    [SerializeField] TextMeshProUGUI eventText;
    [SerializeField] TextMeshProUGUI eventTitle;
    [SerializeField] GameObject eventOneButton;
    [SerializeField] GameObject eventTwoButton;

    EventManager eventManager;


    // Start is called before the first frame update
    void Start()
    {
        eventManager = FindObjectOfType<EventManager>();
        eventManager.RegisterObserver(this);
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
        shipHP.text = "Ship HP: " + amount + "%";
    }

    public void updateMorale(int amount)
    {
        crewMorale.text = "Morale: " + amount + "%";
    }


    public void updateDistance(int amount)
    {
        distance.text = "Distance: " + amount;
    }

    public void displayEvent(string title, string message, bool isTwoChoices) {
       
        eventBox.SetActive(true);
      
        eventTitle.text = title;
        eventText.text = message;
        if (isTwoChoices)
        {
            eventOneButton.SetActive(false);
            eventTwoButton.SetActive(true);

        }
        else
        {
            eventTwoButton.SetActive(false);
            eventOneButton.SetActive(true);
        }
    }

    public void OnEventTriggered(EventData eventData)
    {
        displayEvent(eventData.eventTitle, eventData.description, eventData.isTwoChoices);
    }

    public void CloseEventBox()
    {
        Debug.Log("I am being clicked");
        eventBox.SetActive(false);
    }



}
