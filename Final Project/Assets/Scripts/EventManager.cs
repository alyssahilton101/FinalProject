using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


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


}
[System.Serializable]


public class EventManager : MonoBehaviour
{
    //Handles all game events, both random and scripted, that occur during gameplay.

    public List<EventData> possibleEvents;
    private EventData currentEvent;
    private List<IEventObserver> observers = new List<IEventObserver>();
    EventData testEvent;

    GameManager gameManager;



    // Start is called before the first frame update
    void Start()
    {
        //For testing, setting up event list with 1 test event
        possibleEvents = new List<EventData>();
        testEvent = ScriptableObject.CreateInstance<EventData>();
        testEvent.eventTitle = "Test Event";
        testEvent.description = "This is a test event";
        testEvent.isTwoChoices = false;
        testEvent.effects = new List<Tuple<string, int>>();
        testEvent.effects.Add(new Tuple<string, int>("shipHP", -10));
        testEvent.effects.Add(new Tuple<string, int>("crewMorale", -80));
        testEvent.rarity = 1;

        possibleEvents.Add(testEvent);
        currentEvent = testEvent;


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
        // int randomIndex = UnityEngine.Random.Range(0, possibleEvents.Count);
        //currentEvent = possibleEvents[randomIndex];

        currentEvent = testEvent;

        NotifyObservers(currentEvent);
    }

    private void NotifyObservers(EventData eventData)
    {
        foreach (var observer in observers)
        {
            observer.OnEventTriggered(eventData);
        }
    }


}
