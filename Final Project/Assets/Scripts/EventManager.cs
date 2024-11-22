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
    public EventData seaMonsterEvent;
    public EventData desertedIslandEvent;
    public EventData rivalPiratesEvent;
    public EventData merfolkEvent;
    public EventData ghostShipEvent;
    public EventData hiddenCoveEvent;
    public EventData tradeShipEvent;
    public EventData abandonedShipEvent;
    public EventData sirenSongEvent;
    public EventData portBlockadeEvent;
    public EventData treasureMapEvent;
    public EventData stowawayEvent;
    public EventData floatingBarrelEvent;
    public EventData whirlpoolEvent;
    public EventData ghostlyFogEvent;
    public EventData giantSquidEvent;
    public EventData pirateCoveEvent;
    public EventData sunkenTreasureEvent;
    public EventData plagueEvent;
    public EventData seaTraderEvent;
    public EventData crewArgumentEvent;
    public EventData seaBattleEvent;
    public EventData midnightMutinyEvent;
    public EventData shortageEvent;
    public EventData stormySeasEvent;
    public EventData doldrumsEvent;
    public EventData mysteriousFogEvent;
    public EventData whaleSightingEvent;
    public EventData shipwreckSurvivorsEvent;
    public EventData mermaidsWarningEvent;
    public EventData strangeCatchEvent;
    public EventData floatingCargoEvent;
    public EventData pirateChallengeEvent;
    public EventData sharkFrenzyEvent;
    public EventData navyEvent;
    public EventData floatingBottleEvent;
    public EventData mysteriousLightsEvent;
    public EventData starvingCastawayEvent;
    public EventData ratInfestationEvent;







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
        stormEvent.description = "The skies darken and the waves rise. The crew braces for the storm. (-30 ship HP, -10 morale)";
        stormEvent.isTwoChoices = false;
        stormEvent.effects = new List<Tuple<string, int>>();
        stormEvent.effects.Add(new Tuple<string, int>("shipHP", -30));
        stormEvent.effects.Add(new Tuple<string, int>("crewMorale", -10));
        stormEvent.rarity = 1;
        stormEvent.sound = concern;
        possibleEvents.Add(stormEvent);

        //Shipwreck Event Success
        EventData shipwreckSuccess = ScriptableObject.CreateInstance<EventData>();
        shipwreckSuccess.eventTitle = "Success!";
        shipwreckSuccess.description = "Ye find a chest of gold and a few supplies. A good haul! (+50 money, +20 food, and +10 morale)" ;
        shipwreckSuccess.isTwoChoices = false;
        shipwreckSuccess.effects = new List<Tuple<string, int>>();
        shipwreckSuccess.effects.Add(new Tuple<string, int>("money", 50));
        shipwreckSuccess.effects.Add(new Tuple<string, int>("food", 20));
        shipwreckSuccess.effects.Add(new Tuple<string, int>("crewMorale", 10));
        shipwreckSuccess.rarity = 2;
        shipwreckSuccess.sound = good;

        //Shipwreck Event Fail
        EventData shipwreckFail = ScriptableObject.CreateInstance<EventData>();
        shipwreckFail.eventTitle = "Fail!";
        shipwreckFail.description = "Ye find nothing but a few splinters and a bad omen. (-10 morale)";
        shipwreckFail.isTwoChoices = false;
        shipwreckFail.effects = new List<Tuple<string, int>>();
        shipwreckFail.effects.Add(new Tuple<string, int>("crewMorale", -10));
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

        //Sea Monster Event
        seaMonsterEvent = ScriptableObject.CreateInstance<EventData>();
        seaMonsterEvent.eventTitle = "Kraken Attack!";
        seaMonsterEvent.description = "The waters churn, and tentacles rise from the depths! The crew screams in terror. Prepare for a fight! (-30 ship HP, -20 morale)";
        seaMonsterEvent.isTwoChoices = false;
        seaMonsterEvent.effects = new List<Tuple<string, int>>();
        seaMonsterEvent.effects.Add(new Tuple<string, int>("shipHP", -30));
        seaMonsterEvent.effects.Add(new Tuple<string, int>("crewMorale", -20));
        seaMonsterEvent.rarity = 3;
        seaMonsterEvent.sound = concern;
        possibleEvents.Add(seaMonsterEvent);

        //Deserted Island Success
        EventData desertedIslandSuccess = ScriptableObject.CreateInstance<EventData>();
        desertedIslandSuccess.eventTitle = "Success!";
        desertedIslandSuccess.description = "Ye head for shore and are able to find enough fruit to make the trip worth ye time. (+30 food)";
        desertedIslandSuccess.isTwoChoices = false;
        desertedIslandSuccess.effects = new List<Tuple<string, int>>();
        desertedIslandSuccess.effects.Add(new Tuple<string, int>("food", 30));
        desertedIslandSuccess.rarity = 2;
        desertedIslandSuccess.sound = good;

        //Deserted Island Fail
        EventData desertedIslandFail = ScriptableObject.CreateInstance<EventData>();
        desertedIslandFail.eventTitle = "Fail!";
        desertedIslandFail.description = "Ye head to shore, but upon a closer look, seems there was no food to be found. (-15 morale)";
        desertedIslandFail.isTwoChoices = false;
        desertedIslandFail.effects = new List<Tuple<string, int>>();
        desertedIslandFail.effects.Add(new Tuple<string, int>("crewMorale", -15));
        desertedIslandFail.rarity = 2;
        desertedIslandFail.sound = fail;

        //Deserted Island Event
        desertedIslandEvent = ScriptableObject.CreateInstance<EventData>();
        desertedIslandEvent.eventTitle = "Island";
        desertedIslandEvent.description = "Ye spot a tiny island, deserted but ripe with fruit. A chance to restock or a waste of time?";
        desertedIslandEvent.isTwoChoices = true;
        desertedIslandEvent.effects = new List<Tuple<string, int>>();
        desertedIslandEvent.rarity = 2;
        desertedIslandEvent.sound = concern;
        desertedIslandEvent.ifYes = () => { TriggerEvent(desertedIslandFail, desertedIslandSuccess);  };
        desertedIslandEvent.ifNo = () => { onCloseWindow.Invoke(); };
        possibleEvents.Add(desertedIslandEvent);

        //Rival Pirates Flee
        EventData rivalPiratesFleeEvent = ScriptableObject.CreateInstance<EventData>();
        rivalPiratesFleeEvent.eventTitle = "Flee!";
        rivalPiratesFleeEvent.description = "Ye make a run for it, but the other ship is faster. They catch up and board ye, taking some of yer supplies. (-100 gold, -50 food, -30 morale)";
        rivalPiratesFleeEvent.isTwoChoices = false;
        rivalPiratesFleeEvent.effects = new List<Tuple<string, int>>();
        rivalPiratesFleeEvent.effects.Add(new Tuple<string, int>("money", -100));
        rivalPiratesFleeEvent.effects.Add(new Tuple<string, int>("crewMorale", -30));
        rivalPiratesFleeEvent.effects.Add(new Tuple<string, int>("food", -50));
        rivalPiratesFleeEvent.rarity = 3;
        rivalPiratesFleeEvent.sound = fail;

        //Rival Pirates Flee 2
        EventData rivalPiratesFlee2Event = ScriptableObject.CreateInstance<EventData>();
        rivalPiratesFlee2Event.eventTitle = "Flee!";
        rivalPiratesFlee2Event.description = "Ye make a run for it. While ye manage to escape, ye ship is damaged in the process. (-15 ship HP, -10 morale)";
        rivalPiratesFlee2Event.isTwoChoices = false;
        rivalPiratesFlee2Event.effects = new List<Tuple<string, int>>();
        rivalPiratesFlee2Event.effects.Add(new Tuple<string, int>("shipHP", -15));
        rivalPiratesFlee2Event.effects.Add(new Tuple<string, int>("crewMorale", -10));
        rivalPiratesFlee2Event.rarity = 3;
        rivalPiratesFlee2Event.sound = fail;

        //Rival Pirates Fight
        EventData rivalPiratesFightEvent = ScriptableObject.CreateInstance<EventData>();
        rivalPiratesFightEvent.eventTitle = "Fight!";
        rivalPiratesFightEvent.description = "Yer crew fights well and ye manage to come out on top. In the process, ye gain some spoils as a reward. (+100 gold, +25 morale, +50 food)";
        rivalPiratesFightEvent.isTwoChoices = false;
        rivalPiratesFightEvent.effects = new List<Tuple<string, int>>();
        rivalPiratesFightEvent.effects.Add(new Tuple<string, int>("money", 100));
        rivalPiratesFightEvent.effects.Add(new Tuple<string, int>("crewMorale", 25));
        rivalPiratesFightEvent.effects.Add(new Tuple<string, int>("food", 50));
        rivalPiratesFightEvent.rarity = 3;
        rivalPiratesFightEvent.sound = good;


        //Rival Pirates Event
        rivalPiratesEvent = ScriptableObject.CreateInstance<EventData>();
        rivalPiratesEvent.eventTitle = "Pirates!";
        rivalPiratesEvent.description = "Another pirate ship looms on the horizon. They signal a challenge—do ye rise to it?";
        rivalPiratesEvent.isTwoChoices = true;
        rivalPiratesEvent.effects = new List<Tuple<string, int>>();
        rivalPiratesEvent.rarity = 3;
        rivalPiratesEvent.sound = concern;
        rivalPiratesEvent.ifYes = () => { TriggerEvent(rivalPiratesFightEvent); };
        rivalPiratesEvent.ifNo = () => { TriggerEvent(rivalPiratesFlee2Event, rivalPiratesFleeEvent); };
        possibleEvents.Add(rivalPiratesEvent);

        //Merfolk Event

        //Hidden Cover enter
        EventData hiddenCoveEnterEvent = ScriptableObject.CreateInstance<EventData>();
        hiddenCoveEnterEvent.eventTitle = "Enter!";
        hiddenCoveEnterEvent.description = "Ye sail into the cove and find a safe place to anchor. The crew sets to work repairing the ship. (+20 ship HP, +10 morale)";
        hiddenCoveEnterEvent.isTwoChoices = false;
        hiddenCoveEnterEvent.effects = new List<Tuple<string, int>>();
        hiddenCoveEnterEvent.effects.Add(new Tuple<string, int>("shipHP", 20));
        hiddenCoveEnterEvent.effects.Add(new Tuple<string, int>("crewMorale", 10));
        hiddenCoveEnterEvent.rarity = 2;
        hiddenCoveEnterEvent.sound = good;

        //Hidden Cove
        hiddenCoveEvent = ScriptableObject.CreateInstance<EventData>();
        hiddenCoveEvent.eventTitle = "Cove";
        hiddenCoveEvent.description = "Yer lookout spots a secluded cove. It could be a perfect place to repair the ship and take a rest. Do ye sail in?";
        hiddenCoveEvent.isTwoChoices = true;
        hiddenCoveEvent.effects = new List<Tuple<string, int>>();
        hiddenCoveEvent.rarity = 2;
        hiddenCoveEvent.sound = good;
        hiddenCoveEvent.ifYes = () => { TriggerEvent(hiddenCoveEnterEvent); };
        hiddenCoveEvent.ifNo = () => {  onCloseWindow.Invoke(); };
        possibleEvents.Add(hiddenCoveEvent);

        //Ghost Ship Enter
        EventData ghostShipEnterEvent = ScriptableObject.CreateInstance<EventData>();
        ghostShipEnterEvent.eventTitle = "Board!";
        ghostShipEnterEvent.description = "Ye board the ghost ship. While ye find the promised riches, yer crew run from the ship in fear. (+100 gold, -15 morale)";
        ghostShipEnterEvent.isTwoChoices = false;
        ghostShipEnterEvent.effects = new List<Tuple<string, int>>();
        ghostShipEnterEvent.effects.Add(new Tuple<string, int>("money", 100));
        ghostShipEnterEvent.effects.Add(new Tuple<string, int>("crewMorale", -15));
        ghostShipEnterEvent.rarity = 4;
        ghostShipEnterEvent.sound = good;

        //Ghost Ship Event
        ghostShipEvent = ScriptableObject.CreateInstance<EventData>();
        ghostShipEvent.eventTitle = "Ghosts!";
        ghostShipEvent.description = "In the moonlight, a spectral ship drifts across the waves. The crew whispers tales of cursed treasure and deathly spirits. Do ye dare board it?";
        ghostShipEvent.isTwoChoices = true;
        ghostShipEvent.effects = new List<Tuple<string, int>>();
        ghostShipEvent.rarity = 4;
        ghostShipEvent.sound = concern;
        ghostShipEvent.ifYes = () => { TriggerEvent(ghostShipEnterEvent); };
        ghostShipEvent.ifNo = () => {  onCloseWindow.Invoke(); };
        possibleEvents.Add(ghostShipEvent);

        //Trade Ship Plunder
        EventData tradeShipPlunderEvent = ScriptableObject.CreateInstance<EventData>();
        tradeShipPlunderEvent.eventTitle = "Plunder!";
        tradeShipPlunderEvent.description = "Ye board the merchant ship and take what ye can. The crew is pleased with the haul. (+50 gold, +20 food, +10 morale)";
        tradeShipPlunderEvent.isTwoChoices = false;
        tradeShipPlunderEvent.effects = new List<Tuple<string, int>>();
        tradeShipPlunderEvent.effects.Add(new Tuple<string, int>("money", 50));
        tradeShipPlunderEvent.effects.Add(new Tuple<string, int>("food", 20));
        tradeShipPlunderEvent.effects.Add(new Tuple<string, int>("crewMorale", 10));
        tradeShipPlunderEvent.rarity = 3;
        tradeShipPlunderEvent.sound = good;

        //Trade Ship Event
        tradeShipEvent = ScriptableObject.CreateInstance<EventData>();
        tradeShipEvent.eventTitle = "Trade Ship";
        tradeShipEvent.description = "Ye spot a merchant ship on the horizon, likely laden with supplies ripe for plunder. Do ye dare take from it?";
        tradeShipEvent.isTwoChoices = true;
        tradeShipEvent.effects = new List<Tuple<string, int>>();
        tradeShipEvent.rarity = 3;
        tradeShipEvent.sound = concern;
        tradeShipEvent.ifYes = () => { TriggerEvent(tradeShipPlunderEvent); };
        tradeShipEvent.ifNo = () => { onCloseWindow.Invoke(); };
        possibleEvents.Add(tradeShipEvent);

        //Abandoned Ship Investigate
        EventData abandonedShipInvestigateEvent = ScriptableObject.CreateInstance<EventData>();
        abandonedShipInvestigateEvent.eventTitle = "Investigate!";
        abandonedShipInvestigateEvent.description = "Ye send a crew to investigate the abandoned ship. They return with a chest of gold and a few supplies. (+50 gold, +20 food, +10 morale)";
        abandonedShipInvestigateEvent.isTwoChoices = false;
        abandonedShipInvestigateEvent.effects = new List<Tuple<string, int>>();
        abandonedShipInvestigateEvent.effects.Add(new Tuple<string, int>("money", 50));
        abandonedShipInvestigateEvent.effects.Add(new Tuple<string, int>("food", 20));
        abandonedShipInvestigateEvent.effects.Add(new Tuple<string, int>("crewMorale", 10));
        abandonedShipInvestigateEvent.rarity = 2;
        abandonedShipInvestigateEvent.sound = good;

        //Abandoned Ship Investigate Fail
        EventData abandonedShipInvestigateFailEvent = ScriptableObject.CreateInstance<EventData>();
        abandonedShipInvestigateFailEvent.eventTitle = "Fail!";
        abandonedShipInvestigateFailEvent.description = "Ye send a crew to investigate the abandoned ship. They return empty-handed and spooked. (-15 morale)";
        abandonedShipInvestigateFailEvent.isTwoChoices = false;
        abandonedShipInvestigateFailEvent.effects = new List<Tuple<string, int>>();
        abandonedShipInvestigateFailEvent.effects.Add(new Tuple<string, int>("crewMorale", -15));
        abandonedShipInvestigateFailEvent.rarity = 2;
        abandonedShipInvestigateFailEvent.sound = fail;

        //Abandoned Ship Enter
        abandonedShipEvent = ScriptableObject.CreateInstance<EventData>();
        abandonedShipEvent.eventTitle = "Abandoned Ship";
        abandonedShipEvent.description = "A derelict ship floats eerily on the horizon. Could be laden with treasure...or a trap. Do ye send a crew to investigate?";
        abandonedShipEvent.isTwoChoices = true;
        abandonedShipEvent.effects = new List<Tuple<string, int>>();
        abandonedShipEvent.rarity = 2;
        abandonedShipEvent.sound = concern;
        abandonedShipEvent.ifYes = () => { TriggerEvent(abandonedShipInvestigateFailEvent, abandonedShipInvestigateEvent); };
        abandonedShipEvent.ifNo = () => { onCloseWindow.Invoke(); };
        possibleEvents.Add(abandonedShipEvent);

        //Sirens Song Resist
        EventData sirenSongResistEvent = ScriptableObject.CreateInstance<EventData>();
        sirenSongResistEvent.eventTitle = "Resist!";
        sirenSongResistEvent.description = "Ye resist the siren song and sail on. While disappointed at first, the crew is grateful fer yer wisdom in the end. (+10 morale)";
        sirenSongResistEvent.isTwoChoices = false;
        sirenSongResistEvent.effects = new List<Tuple<string, int>>();
        sirenSongResistEvent.effects.Add(new Tuple<string, int>("crewMorale", 10));
        sirenSongResistEvent.rarity = 3;
        sirenSongResistEvent.sound = good;

        //Sirens Song Listen
        EventData sirenSongListenEvent = ScriptableObject.CreateInstance<EventData>();
        sirenSongListenEvent.eventTitle = "Listen!";
        sirenSongListenEvent.description = "Ye follow the siren song to its source. The crew is entranced, and ye barely manage to break the spell before yer ship crashes. (-15 ship HP, -20 morale)";
        sirenSongListenEvent.isTwoChoices = false;
        sirenSongListenEvent.effects = new List<Tuple<string, int>>();
        sirenSongListenEvent.effects.Add(new Tuple<string, int>("shipHP", -15));
        sirenSongListenEvent.effects.Add(new Tuple<string, int>("crewMorale", -20));
        sirenSongListenEvent.rarity = 3;
        sirenSongListenEvent.sound = fail;


        //Sirens Event
        sirenSongEvent = ScriptableObject.CreateInstance<EventData>();
        sirenSongEvent.eventTitle = "Siren Song";
        sirenSongEvent.description = "A haunting melody drifts over the waves, entrancing the crew. The source is unknown, but the temptation to follow it grows stronger. Do ye resist or follow the song?";
        sirenSongEvent.isTwoChoices = true;
        sirenSongEvent.effects = new List<Tuple<string, int>>();
        sirenSongEvent.rarity = 3;
        sirenSongEvent.sound = concern;
        sirenSongEvent.ifYes = () => { TriggerEvent(sirenSongListenEvent); };
        sirenSongEvent.ifNo = () => { TriggerEvent(sirenSongResistEvent); };
        possibleEvents.Add(sirenSongEvent);

        //Port Bribed
        EventData portBribedEvent = ScriptableObject.CreateInstance<EventData>();
        portBribedEvent.eventTitle = "Bribe!";
        portBribedEvent.description = "Ye pay the bribe and are allowed to pass. The crew grumbles, but ye make it through. (-50 gold)";
        portBribedEvent.isTwoChoices = false;
        portBribedEvent.effects = new List<Tuple<string, int>>();
        portBribedEvent.effects.Add(new Tuple<string, int>("money", -50));
        portBribedEvent.rarity = 3;
        portBribedEvent.sound = good;

        //Port Escaped
        EventData portEscapedEvent = ScriptableObject.CreateInstance<EventData>();
        portEscapedEvent.eventTitle = "Escape!";
        portEscapedEvent.description = "Ye try to escape the blockade, but the naval ships catch ye. The crew is angry at the wasted effort. (-10 ship HP, -10 crew morale)";
        portEscapedEvent.isTwoChoices = false;
        portEscapedEvent.effects = new List<Tuple<string, int>>();
        portEscapedEvent.effects.Add(new Tuple<string, int>("shipHP", -10));
        portEscapedEvent.effects.Add(new Tuple<string, int>("crewMorale", -10));
        portEscapedEvent.rarity = 3;
        portEscapedEvent.sound = fail;


        //Port Blockade
        portBlockadeEvent = ScriptableObject.CreateInstance<EventData>();
        portBlockadeEvent.eventTitle = "Port Blockade";
        portBlockadeEvent.description = "Yer approach to a busy port is blocked by naval ships! Ye could always sail off... or, ye could give a bribe to get through. Do ye try it?";
        portBlockadeEvent.isTwoChoices = true;
        portBlockadeEvent.effects = new List<Tuple<string, int>>();
        portBlockadeEvent.effects.Add(new Tuple<string, int>("money", -50)); // Bribe cost.
        portBlockadeEvent.effects.Add(new Tuple<string, int>("shipHP", -20)); // Penalty for escape attempt.
        portBlockadeEvent.rarity = 3;
        portBlockadeEvent.sound = concern;
        portBlockadeEvent.ifYes = () => { TriggerEvent(portBribedEvent); };
        portBlockadeEvent.ifNo = () => { TriggerEvent(portEscapedEvent); };
        possibleEvents.Add(portBlockadeEvent);

        //Treasure Map Success
        EventData treasureMapSuccessEvent = ScriptableObject.CreateInstance<EventData>();
        treasureMapSuccessEvent.eventTitle = "Success!";
        treasureMapSuccessEvent.description = "Ye follow the map to a hidden cove and find a chest of gold. (+100 gold)";
        treasureMapSuccessEvent.isTwoChoices = false;
        treasureMapSuccessEvent.effects = new List<Tuple<string, int>>();
        treasureMapSuccessEvent.effects.Add(new Tuple<string, int>("money", 100));
        treasureMapSuccessEvent.rarity = 3;
        treasureMapSuccessEvent.sound = good;

        //Treasure Map Fail
        EventData treasureMapFailEvent = ScriptableObject.CreateInstance<EventData>();
        treasureMapFailEvent.eventTitle = "Fail!";
        treasureMapFailEvent.description = "Ye follow the map to a hidden cove, but find nothing. (-10 morale)";
        treasureMapFailEvent.isTwoChoices = false;
        treasureMapFailEvent.effects = new List<Tuple<string, int>>();
        treasureMapFailEvent.effects.Add(new Tuple<string, int>("crewMorale", -10));
        treasureMapFailEvent.rarity = 3;
        treasureMapFailEvent.sound = fail;

        //Treasure Map
        treasureMapEvent = ScriptableObject.CreateInstance<EventData>();
        treasureMapEvent.eventTitle = "Treasure Map";
        treasureMapEvent.description = "Ye find a map that promises a hidden treasure. Do ye follow it?";
        treasureMapEvent.isTwoChoices = true;
        treasureMapEvent.effects = new List<Tuple<string, int>>();
        treasureMapEvent.rarity = 3;
        treasureMapEvent.sound = concern;
        treasureMapEvent.ifYes = () => { TriggerEvent(treasureMapSuccessEvent, treasureMapFailEvent); };
        treasureMapEvent.ifNo = () => { onCloseWindow.Invoke(); };

        //Stowaway Kept
        EventData stowawayKeptEvent = ScriptableObject.CreateInstance<EventData>();
        stowawayKeptEvent.eventTitle = "Kept!";
        stowawayKeptEvent.description = " Ye decide to keep the stowaway aboard. Their word is good--they prove to be a skilled navigator and boost morale. (+15 morale, -20 gold, -30 food)";
        stowawayKeptEvent.isTwoChoices = false;
        stowawayKeptEvent.effects = new List<Tuple<string, int>>();
        stowawayKeptEvent.effects.Add(new Tuple<string, int>("crewMorale", 15));
        stowawayKeptEvent.effects.Add(new Tuple<string, int>("money", -20));
        stowawayKeptEvent.effects.Add(new Tuple<string, int>("food", -30));
        stowawayKeptEvent.rarity = 2;
        stowawayKeptEvent.sound = good;

        //Stowaway Kept fail
        EventData stowawayFailEvent = ScriptableObject.CreateInstance<EventData>();
        stowawayFailEvent.eventTitle = "Kept!";
        stowawayFailEvent.description = "Ye decide to keep the stowaway aboard. However, it seems their word wasn't good--they steal supplies and flee. (-30 food, -45 gold, -15 morale)";
        stowawayFailEvent.isTwoChoices = false;
        stowawayFailEvent.effects = new List<Tuple<string, int>>();
        stowawayFailEvent.effects.Add(new Tuple<string, int>("crewMorale", -15));
        stowawayFailEvent.effects.Add(new Tuple<string, int>("money", -45));
        stowawayFailEvent.effects.Add(new Tuple<string, int>("food", -30));
        stowawayFailEvent.rarity = 2;
        stowawayFailEvent.sound = fail;


        //Stowaway Event
        stowawayEvent = ScriptableObject.CreateInstance<EventData>();
        stowawayEvent.eventTitle = "Stowaway Discovered!";
        stowawayEvent.description = "The crew discovers a stowaway hiding in the cargo hold. They claim to be a skilled navigator, but ye aren't certain ye can trust 'em. Should ye keep 'em aboard?";
        stowawayEvent.isTwoChoices = true;
        stowawayEvent.effects = new List<Tuple<string, int>>();
        stowawayEvent.effects.Add(new Tuple<string, int>("crewMorale", 15)); // Boost if kept.
        stowawayEvent.effects.Add(new Tuple<string, int>("money", -20)); // Minor cost for supplies.
        stowawayEvent.rarity = 2;
        stowawayEvent.sound = concern;
        stowawayEvent.ifYes = () => { TriggerEvent(stowawayFailEvent, stowawayKeptEvent); };
        stowawayEvent.ifNo = () => { onCloseWindow.Invoke(); };
        possibleEvents.Add(stowawayEvent);

        //Floating Barrel
        floatingBarrelEvent = ScriptableObject.CreateInstance<EventData>();
        floatingBarrelEvent.eventTitle = "Barrel";
        floatingBarrelEvent.description = "Ye spot a floating barrel in the water. Yer crew drags it in--looks like ye've gained some food. (+30 food)";
        floatingBarrelEvent.isTwoChoices = false;
        floatingBarrelEvent.effects = new List<Tuple<string, int>>();
        floatingBarrelEvent.effects.Add(new Tuple<string, int>("food", 30));
        floatingBarrelEvent.rarity = 1;
        floatingBarrelEvent.sound = good;
        possibleEvents.Add(floatingBarrelEvent);

        //Whirlpool
        whirlpoolEvent = ScriptableObject.CreateInstance<EventData>();
        whirlpoolEvent.eventTitle = "Whirlpool!";
        whirlpoolEvent.description = "Ye sail into a whirlpool! The crew fights to keep the ship afloat. (-20 ship HP, -10 morale)";
        whirlpoolEvent.isTwoChoices = false;
        whirlpoolEvent.effects = new List<Tuple<string, int>>();
        whirlpoolEvent.effects.Add(new Tuple<string, int>("shipHP", -20));
        whirlpoolEvent.effects.Add(new Tuple<string, int>("crewMorale", -10));
        whirlpoolEvent.rarity = 2;
        whirlpoolEvent.sound = concern;
        possibleEvents.Add(whirlpoolEvent);

        //Ghostly Fog
        ghostlyFogEvent = ScriptableObject.CreateInstance<EventData>();
        ghostlyFogEvent.eventTitle = "Fog!";
        ghostlyFogEvent.description = "A ghostly fog rolls in, obscuring the ship. The crew is on edge. (-10 morale)";
        ghostlyFogEvent.isTwoChoices = false;
        ghostlyFogEvent.effects = new List<Tuple<string, int>>();
        ghostlyFogEvent.effects.Add(new Tuple<string, int>("crewMorale", -10));
        ghostlyFogEvent.rarity = 1;
        ghostlyFogEvent.sound = concern;
        possibleEvents.Add(ghostlyFogEvent);

        //Giant Squid
        giantSquidEvent = ScriptableObject.CreateInstance<EventData>();
        giantSquidEvent.eventTitle = "Squid!";
        giantSquidEvent.description = "A giant squid attacks the ship! The crew fights it off, but not without cost. (-20 ship HP, -10 morale)";
        giantSquidEvent.isTwoChoices = false;
        giantSquidEvent.effects = new List<Tuple<string, int>>();
        giantSquidEvent.effects.Add(new Tuple<string, int>("shipHP", -20));
        giantSquidEvent.effects.Add(new Tuple<string, int>("crewMorale", -10));
        giantSquidEvent.rarity = 2;
        giantSquidEvent.sound = concern;
        possibleEvents.Add(giantSquidEvent);

        //Pirate Cove
        pirateCoveEvent = ScriptableObject.CreateInstance<EventData>();
        pirateCoveEvent.eventTitle = "Cove!";
        pirateCoveEvent.description = "Ye spot a pirate cove on the horizon. The crew is eager to plunder it. (+50 gold, +20 food, +10 morale)";
        pirateCoveEvent.isTwoChoices = false;
        pirateCoveEvent.effects = new List<Tuple<string, int>>();
        pirateCoveEvent.effects.Add(new Tuple<string, int>("money", 50));
        pirateCoveEvent.effects.Add(new Tuple<string, int>("food", 20));
        pirateCoveEvent.effects.Add(new Tuple<string, int>("crewMorale", 10));
        pirateCoveEvent.rarity = 2;
        pirateCoveEvent.sound = good;
        possibleEvents.Add(pirateCoveEvent);

        //Sunken Treasure
        sunkenTreasureEvent = ScriptableObject.CreateInstance<EventData>();
        sunkenTreasureEvent.eventTitle = "Treasure!";
        sunkenTreasureEvent.description = "Ye spot a sunken ship on the ocean floor. The crew dives in and finds a chest of gold. (+100 gold)";
        sunkenTreasureEvent.isTwoChoices = false;
        sunkenTreasureEvent.effects = new List<Tuple<string, int>>();
        sunkenTreasureEvent.effects.Add(new Tuple<string, int>("money", 100));
        sunkenTreasureEvent.rarity = 2;
        sunkenTreasureEvent.sound = good;
        possibleEvents.Add(sunkenTreasureEvent);

        //Plague
        plagueEvent = ScriptableObject.CreateInstance<EventData>();
        plagueEvent.eventTitle = "Plague!";
        plagueEvent.description = "A sickness spreads through the crew. The ship and it's crew are quarantined until the bulk of ye crew recover. (-30 morale)";
        plagueEvent.isTwoChoices = false;
        plagueEvent.effects = new List<Tuple<string, int>>();
        plagueEvent.effects.Add(new Tuple<string, int>("crewMorale", -20));
        plagueEvent.rarity = 2;
        plagueEvent.sound = concern;
        possibleEvents.Add(plagueEvent);

        //Sea trader bought
       EventData seaTraderBought = ScriptableObject.CreateInstance<EventData>();
        seaTraderBought.eventTitle = "Traitor!";
        seaTraderBought.description = "Ye purchase some supplies from the trader. (+20 food, -20 gold)";
        seaTraderBought.isTwoChoices = false;
        seaTraderBought.effects = new List<Tuple<string, int>>();
        seaTraderBought.effects.Add(new Tuple<string, int>("food", 20));
        seaTraderBought.effects.Add(new Tuple<string, int>("money", -20));
        seaTraderBought.rarity = 1;
        seaTraderBought.sound = good;

        //Sea Trader
        seaTraderEvent = ScriptableObject.CreateInstance<EventData>();
        seaTraderEvent.eventTitle = "Trader!";
        seaTraderEvent.description = "A friendly merchant ship crosses yer path, offering goods at a fair price. Will ye trade?";
        seaTraderEvent.isTwoChoices = true;
        seaTraderEvent.effects = new List<Tuple<string, int>>();
        seaTraderEvent.rarity = 1;
        seaTraderEvent.sound = good;
        seaTraderEvent.ifYes = () => { TriggerEvent(seaTraderBought); };
        seaTraderEvent.ifNo = () => { onCloseWindow.Invoke(); };
        possibleEvents.Add(seaTraderEvent);

        //Crew Arugment Intervene
        EventData crewArgumentIntervene = ScriptableObject.CreateInstance<EventData>();
        crewArgumentIntervene.eventTitle = "Intervene!";
        crewArgumentIntervene.description = "Ye step in and help settle things down. While tense at first, yer crew is grateful in the end. (+15 morale)";
        crewArgumentIntervene.isTwoChoices = false;
        crewArgumentIntervene.effects = new List<Tuple<string, int>>();
        crewArgumentIntervene.effects.Add(new Tuple<string, int>("crewMorale", 15));
        crewArgumentIntervene.rarity = 1;
        crewArgumentIntervene.sound = good;

        //Crew Arugment Ignore
        EventData crewArgumentIgnore = ScriptableObject.CreateInstance<EventData>();
        crewArgumentIgnore.eventTitle = "Ignore!";
        crewArgumentIgnore.description = "Ye let the crew settle things on their own. While they sort it out, the tension lingers. (-10 morale)";
        crewArgumentIgnore.isTwoChoices = false;
        crewArgumentIgnore.effects = new List<Tuple<string, int>>();
        crewArgumentIgnore.effects.Add(new Tuple<string, int>("crewMorale", -10));
        crewArgumentIgnore.rarity = 1;
        crewArgumentIgnore.sound = fail;

        //Crew Argument
        crewArgumentEvent = ScriptableObject.CreateInstance<EventData>();
        crewArgumentEvent.eventTitle = "Crew Dispute!";
        crewArgumentEvent.description = "Two crewmates are at each other’s throats over a stolen bottle of rum. Do ye intervene to calm things down?";
        crewArgumentEvent.isTwoChoices = true;
        crewArgumentEvent.effects = new List<Tuple<string, int>>();
        crewArgumentEvent.rarity = 1;
        crewArgumentEvent.sound = concern;
        crewArgumentEvent.ifYes = () => { TriggerEvent(crewArgumentIntervene); };
        crewArgumentEvent.ifNo = () => { TriggerEvent(crewArgumentIgnore); };
        possibleEvents.Add(crewArgumentEvent);

        //Rat Infestation
        ratInfestationEvent = ScriptableObject.CreateInstance<EventData>();
        ratInfestationEvent.eventTitle = "Rats!";
        ratInfestationEvent.description = "Rats have infested the ship! The crew works to get rid of them, but not without cost. (-10 morale, -30 food)";
        ratInfestationEvent.isTwoChoices = false;
        ratInfestationEvent.effects = new List<Tuple<string, int>>();
        ratInfestationEvent.effects.Add(new Tuple<string, int>("crewMorale", -10));
        ratInfestationEvent.effects.Add(new Tuple<string, int>("food", -30));
        ratInfestationEvent.rarity = 1;
        ratInfestationEvent.sound = concern;
        possibleEvents.Add(ratInfestationEvent);

        //Mutiny Take Action
        EventData mutinyTakeAction = ScriptableObject.CreateInstance<EventData>();
        mutinyTakeAction.eventTitle = "Action!";
        mutinyTakeAction.description = "Ye root out the mutineers and restore order. The crew is grateful for yer leadership. (+20 morale)";
        mutinyTakeAction.isTwoChoices = false;
        mutinyTakeAction.effects = new List<Tuple<string, int>>();
        mutinyTakeAction.effects.Add(new Tuple<string, int>("crewMorale", 20));
        mutinyTakeAction.rarity = 3;
        mutinyTakeAction.sound = good;

        //Mutiny Let Fester
        EventData mutinyLetFester = ScriptableObject.CreateInstance<EventData>();
        mutinyLetFester.eventTitle = "Say nothing";
        mutinyLetFester.description = "Ye say nothing, letting the mutineers fester. Their word spreads and the crew grows restless. (-35 morale)";
        mutinyLetFester.isTwoChoices = false;
        mutinyLetFester.effects = new List<Tuple<string, int>>();
        mutinyLetFester.effects.Add(new Tuple<string, int>("crewMorale", -35));
        mutinyLetFester.rarity = 3;
        mutinyLetFester.sound = fail;


        //Midnight Mutiny
        midnightMutinyEvent = ScriptableObject.CreateInstance<EventData>();
        midnightMutinyEvent.eventTitle = "Mutiny Brews";
        midnightMutinyEvent.description = "Rumors of mutiny echo below deck. Do ye root out the troublemakers and attempt to restore order?";
        midnightMutinyEvent.isTwoChoices = true;
        midnightMutinyEvent.effects = new List<Tuple<string, int>>();
        midnightMutinyEvent.rarity = 3;
        midnightMutinyEvent.sound = concern;
        midnightMutinyEvent.ifYes = () => { TriggerEvent(mutinyTakeAction); };
        midnightMutinyEvent.ifNo = () => { TriggerEvent(mutinyLetFester); };
        possibleEvents.Add(midnightMutinyEvent);







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
