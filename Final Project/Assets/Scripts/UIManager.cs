using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] GameObject eventBox;
    [SerializeField] TextMeshProUGUI eventText;
    [SerializeField] TextMeshProUGUI eventTitle;


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

    public void updateMorale(int amount)
    {
        shipHP.text = "Morale: " + amount;
    }


    public void updateDistance(int amount)
    {
        distance.text = "Distance: " + amount;
    }

    public void displayEvent(string title, string message) {
        if (eventBox.activeSelf)
        {
            eventBox.SetActive(false);
            eventText.text = "You should not see this.";
        }
        eventBox.SetActive(true);
        eventTitle.text = title;
        eventText.text = message;
    }

    

}
