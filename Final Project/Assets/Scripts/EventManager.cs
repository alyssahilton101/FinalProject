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




}
[System.Serializable]


public class EventManager : MonoBehaviour
{
    //Handles all game events, both random and scripted, that occur during gameplay.

    public List<EventData> possibleEvents;
    private EventData currentEvent;
    private List<IEventObserver> observers = new List<IEventObserver>();

    //Events
    public EventData islandEvent;
    public EventData stormEvent;
    public UnityEvent onEventTriggered;
    public UnityEvent onIslandEventYes;
    public UnityEvent onCloseWindow;

    //Variable version of a method for passing to choice
    public delegate void ToDo(bool choice);




    // Start is called before the first frame update
    void Start()
    {
        //Setting up all the events. Is there a better way to do this? Probably lol
        possibleEvents = new List<EventData>();

        //Island Event
        islandEvent = ScriptableObject.CreateInstance<EventData>();
        islandEvent.eventTitle = "Land Ho!";
        islandEvent.description = "Through the spyglass, ye see a shore lined with buildings. Seems this could be a good chance to rest or snag supplies. Do ye make for shore?";
        islandEvent.isTwoChoices = true; 
        islandEvent.effects = new List<Tuple<string, int>>();
        islandEvent.rarity = 1;
        islandEvent.ifYes = () => { Debug.Log("Yes"); onIslandEventYes.Invoke(); };
        islandEvent.ifNo = () => { Debug.Log("No"); onCloseWindow.Invoke(); };

        //Storm Event
        stormEvent = ScriptableObject.CreateInstance<EventData>();
        stormEvent.eventTitle = "Storm approaches!";
        stormEvent.description = "This is an unfinished event";
        stormEvent.isTwoChoices = false;
        stormEvent.effects = new List<Tuple<string, int>>();
        stormEvent.effects.Add(new Tuple<string, int>("shipHP", -10));
        stormEvent.effects.Add(new Tuple<string, int>("crewMorale", -10));
        stormEvent.rarity = 1;


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

       // currentEvent = testEvent;

       // NotifyObservers(currentEvent);
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

}
