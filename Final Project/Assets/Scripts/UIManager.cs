using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
    [SerializeField] GameObject storeBox;
    [SerializeField] UnityEngine.UI.Button yesButton;
    [SerializeField] UnityEngine.UI.Button noButton;
    [SerializeField] AudioClip buttonSound;

    EventManager eventManager;
    public UnityEvent onCloseMenu;
    BackgroundController backgroundController;

    //For resetting environment on game over
    [SerializeField] GameObject environment;

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
        food.text = ""+ amount;
    }

    public void updateMoney(int amount)
    {
        money.text = "" + amount;
    }


    public void updateShipHP(int amount)
    {
        shipHP.text = "" + amount + "%";
    }

    public void updateMorale(int amount)
    {
        crewMorale.text = "" + amount + "%";
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

    public void displayTwoChoiceEvent(string title, string message) {
        // Clear any previous listeners
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        //Add back sound onClick
        yesButton.onClick.AddListener(PlayButtonSound);
        noButton.onClick.AddListener(PlayButtonSound);

        //Add the new listeners
        yesButton.onClick.AddListener(() => eventManager.ChoiceMade(true));
        noButton.onClick.AddListener(() => eventManager.ChoiceMade(false));

        displayEvent(title, message, true);

    }

    public void DisplayStore() {
        eventBox.SetActive(false);
        storeBox.SetActive(true);
    }
    public void CloseStore() { 
        storeBox.SetActive(false);
        onCloseMenu.Invoke();
    }

    public void OnEventTriggered(EventData eventData)
    {
        if (eventData.isTwoChoices) {
            Debug.Log("This event has two choices, gross");
            displayTwoChoiceEvent(eventData.eventTitle, eventData.description);
        }
        else {
            displayEvent(eventData.eventTitle, eventData.description, eventData.isTwoChoices);
        }
    }

    public void CloseEventBox()
    {
        Debug.Log("I am being clicked");
        eventBox.SetActive(false);
        onCloseMenu.Invoke();
    }

    public void displayGameOver(string title, string message) {
        displayEvent(title, message, false);


        //Set the button to restart the game
        eventOneButton.GetComponentInChildren<TextMeshProUGUI>().text = "Restart";
        eventOneButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(RestartGame);
    }

    public void RestartGame()
    {
       
        backgroundController = FindObjectOfType<BackgroundController>();
        backgroundController.ResetBackgrounds();
        SceneManager.LoadScene(1);

    }

    public void PlayButtonSound()
    {
        AudioSource.PlayClipAtPoint(buttonSound, Camera.main.transform.position);
    }



}
