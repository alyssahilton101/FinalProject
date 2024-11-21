using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.Events;


//Referenced the following in creating this:
//https://docs.unity3d.com/2022.3/Documentation/Manual/class-ScriptableObject.html
//https://youtu.be/DWiMI92Lzx0?si=Aa2eU93fDVtDEn1e

public interface IEventObserver
{
    void OnEventTriggered(EventData eventData);
}



[CreateAssetMenu(fileName = "NewEventData", menuName = "Event System/EventData")]
public class EventData : ScriptableObject
{
    //Title of the Event for the UI
    public string eventTitle;

    //Description of the event for the UI
    public string description;

    //Type of event so UI can display the correct number of choices. (Either 1 or 2)
    public bool isTwoChoices;

    //List of effects of the event for ResoruceManager
    //Each element is a tuple of the resoruce name and the amount of change
    public List<Tuple<string, int>> effects;

    //Rarity of the event. Higher rarity means less likely to occur
    public int rarity;

    //Method for yes and no choices
    public Action ifYes;
    public Action ifNo;

    //Sound to play when the event is triggered
    public AudioClip sound;





}
[System.Serializable]


public class EventManager : MonoBehaviour
{
    //Handles all game events, both random and scripted, that occur during gameplay.

    public List<EventData> possibleEvents;
    private EventData currentEvent;
    private List<IEventObserver> observers = new List<IEventObserver>();

    //Events
    public EventData startEvent;
    public EventData islandEvent;
    public EventData stormEvent;
    public EventData treasureEvent;
    public EventData shipwreckEvent;
    public UnityEvent onEventTriggered;
    public UnityEvent onIslandEventYes;
    public UnityEvent onCloseWindow;

    //Sounds to play when the event is triggered
    [SerializeField] AudioClip concern;
    [SerializeField] AudioClip fail;
    [SerializeField] AudioClip good;
    [SerializeField] AudioClip veryGood;



    // Start is called before the first frame update
    void Start()
    {
        //Setting up all the events. Is there a better way to do this? Probably lol
        possibleEvents = new List<EventData>();

        //Start Event
        startEvent = ScriptableObject.CreateInstance<EventData>();
        startEvent.eventTitle = "Start Event";
        startEvent.description =
            "You are the captain of a ship. You must navigate the seas and reach your destination. " +
            "You will face many challenges along the way. Good luck!";
        startEvent.isTwoChoices = false;
        startEvent.effects = new List<Tuple<string, int>>();
        startEvent.rarity = 0;
        startEvent.sound = good;


        //Island Event
        islandEvent = ScriptableObject.CreateInstance<EventData>();
        islandEvent.eventTitle = "Land Ho!";
        islandEvent.description = "Through the spyglass, ye see a shore lined with buildings." +
            "Seems this could be a good chance to rest or snag supplies. Do ye make for shore?";
        islandEvent.isTwoChoices = true; 
        islandEvent.effects = new List<Tuple<string, int>>();
        islandEvent.rarity = 1;
        islandEvent.sound = good;
        islandEvent.ifYes = () => { Debug.Log("Yes"); onIslandEventYes.Invoke(); };
        islandEvent.ifNo = () => { Debug.Log("No"); onCloseWindow.Invoke(); };

        //Storm Event
        stormEvent = ScriptableObject.CreateInstance<EventData>();
        stormEvent.eventTitle = "Storm!";
        stormEvent.description = "This is an unfinished event. -10 hp and -10 morale";
        stormEvent.isTwoChoices = false;
        stormEvent.effects = new List<Tuple<string, int>>();
        stormEvent.effects.Add(new Tuple<string, int>("shipHP", -30));
        stormEvent.effects.Add(new Tuple<string, int>("crewMorale", -10));
        stormEvent.rarity = 1;
        stormEvent.sound = concern;
        possibleEvents.Add(stormEvent);

        //Treasure Event
        treasureEvent = ScriptableObject.CreateInstance<EventData>();
        treasureEvent.eventTitle = "Treasure!";
        treasureEvent.description = "This is an unfinished event. +100 gold";
        treasureEvent.isTwoChoices = false;
        treasureEvent.effects = new List<Tuple<string, int>>();
        treasureEvent.effects.Add(new Tuple<string, int>("money", 100));
        treasureEvent.rarity = 1;
        treasureEvent.sound = good;
        possibleEvents.Add(treasureEvent);

        //Shipwreck Event Success
        EventData shipwreckSuccess = ScriptableObject.CreateInstance<EventData>();
        shipwreckSuccess.eventTitle = "Success!";
        shipwreckSuccess.description = "Ye find a chest of gold and a few supplies. A good haul! (+50 money, +20 food, and +10 morale)" ;
        shipwreckSuccess.isTwoChoices = false;
        shipwreckSuccess.effects = new List<Tuple<string, int>>();
        shipwreckSuccess.effects.Add(new Tuple<string, int>("money", 50));
        shipwreckSuccess.effects.Add(new Tuple<string, int>("food", 20));
        shipwreckSuccess.effects.Add(new Tuple<string, int>("morale", 10));
        shipwreckSuccess.rarity = 2;
        shipwreckSuccess.sound = good;

        //Shipwreck Event Fail
        EventData shipwreckFail = ScriptableObject.CreateInstance<EventData>();
        shipwreckFail.eventTitle = "Fail!";
        shipwreckFail.description = "Ye find nothing but a few splinters and a bad omen. (-10 morale)";
        shipwreckFail.isTwoChoices = false;
        shipwreckFail.effects = new List<Tuple<string, int>>();
        shipwreckFail.effects.Add(new Tuple<string, int>("morale", -10));
        shipwreckFail.rarity = 2;
        shipwreckFail.sound = fail;


        //Shipwreck Event
        shipwreckEvent = ScriptableObject.CreateInstance<EventData>();
        shipwreckEvent.eventTitle = "Wreck Ahead!";
        shipwreckEvent.description = "Yer spyglass spots the remnants of a shipwreck drifting on the waves." +
            "Could be salvageable loot...or trouble. Do ye risk investigating?";
        shipwreckEvent.isTwoChoices = true;
        shipwreckEvent.effects = new List<Tuple<string, int>>();
        shipwreckEvent.rarity = 2;
        shipwreckEvent.sound = concern;
        shipwreckEvent.ifYes = () => {  TriggerEvent(shipwreckSuccess, shipwreckFail); /* Add logic for a random reward or penalty */ };
        shipwreckEvent.ifNo = () => { onCloseWindow.Invoke(); };
        possibleEvents.Add(shipwreckEvent);

       



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RegisterObserver(IEventObserver observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }
    }

    public void UnregisterObserver(IEventObserver observer)
    {
        if (observers.Contains(observer))
        {
            observers.Remove(observer);
        }
    }

    public void TriggerRandomEvent()
    {
        int randomIndex = UnityEngine.Random.Range(0, possibleEvents.Count);
        currentEvent = possibleEvents[randomIndex];
        NotifyObservers(currentEvent);
        onEventTriggered.Invoke();
    }
    public void TriggerIslandEvent() {
        currentEvent = islandEvent;
        NotifyObservers(currentEvent);
        onEventTriggered.Invoke();

    }

    private void NotifyObservers(EventData eventData)
    {
        foreach (var observer in observers)
        {
            observer.OnEventTriggered(eventData);
        }
    }

    public void ChoiceMade(bool choice)
    {
        Debug.Log("Choice made: " + choice);
        if (choice)
        {
            currentEvent.ifYes();
        }
        else
        {
            currentEvent.ifNo();
        }
    }

    //Generilized method for triggering events, useful in the future for more complex events
    public void TriggerEvent(EventData eventData) {
        currentEvent = eventData;
        NotifyObservers(currentEvent);
       // onEventTriggered.Invoke();
    }

    public void TriggerEvent(EventData eventData1, EventData eventData2)
    {
        List<EventData> possibleOutcomes;
        possibleOutcomes = new List<EventData>();
        possibleOutcomes.Add(eventData1);
        possibleOutcomes.Add(eventData2);
        //To do: add rarity logic
        int randomIndex = UnityEngine.Random.Range(0, possibleOutcomes.Count);
        currentEvent = possibleOutcomes[randomIndex];
        NotifyObservers(currentEvent);
        //onEventTriggered.Invoke();
    }

}
