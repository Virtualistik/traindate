﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct Decision
{
    public List<BaseEvent> events;
    public string message;

    public Decision(string mes, List<BaseEvent> newEvents)
    {
        events = newEvents;
        message = mes;
    }
}

// Base event class
public class BaseEvent
{

    // List of potential events to go to next
    public List<string> nextEventList;

    // bool to check if event has finished
    public bool End;


    // constructor/destructor
    public BaseEvent()
    {

    }

    ~BaseEvent()
    {

    }


    // virtual methods for inheritance
    //
    // begin is called when event starts
    public virtual void begin()
    {

    }

    // OnMouse Down is called when left mouse button is pressed
    public virtual void OnInput()
    {

    }

    // returns next event
    public virtual string nextEvent()
    {
        return nextEventList[0];
    }

}

public class SetNextFileEvent : BaseEvent
{

    private string file;

    // constructor/destructor
    public SetNextFileEvent(string newFile)
    {

        file = newFile;

        End = false;

    }

    ~SetNextFileEvent()
    {

    }


    // virtual methods for inheritance
    //
    // begin is called when event starts
    public override void begin()
    {
        /*
        GameObject manager = GameObject.FindGameObjectWithTag("EventManager");//.GetComponent<eventManager>();

        manager.GetComponent<eventManager>().SetNextEvent(file);
        */

        eventManager.manager.SetNextEvent(file);
        End = true;
    }

    // OnMouse Down is called when left mouse button is pressed
    public override void OnInput()
    {

    }

    // returns next event
    public override string nextEvent()
    {
        return nextEventList[0];
    }
}

// Audio event class
public class AudioEvent : BaseEvent
{


    private string file;
    private bool blocking;

    private GameObject Audio;

    // constructor/destructor
    public AudioEvent(string filePath, bool block = false)
    {

        // set attributes

        file = filePath;
        blocking = block;

        

        End = false;
    }

    ~AudioEvent()
    {

    }

    // begin is called when event starts
    public override void begin()
    {

        // play audio from file

        Audio = GameObject.FindGameObjectWithTag("AudioHandler");

        // check if event will block next event
        if (!blocking)
        {

            // if not blocking end the event
            End = true;
        }
    }

    // OnMouse Down is called when left mouse button is pressed
    public override void OnInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // end the event
            End = true;
        }
    }

    // returns next event
    public override string nextEvent()
    {
        return nextEventList[0];
    }

}

public class DecisionEvent : BaseEvent
{

    // attributes
    private int currentSelect;
    private List<Decision> selection;

    GameObject EManager;

    public DecisionEvent(List<Decision> newSelection)
    {
        selection = newSelection;

        //UI = GameObject.FindGameObjectWithTag("Canvas");
        //EManager = GameObject.FindGameObjectWithTag("EventManager");

        End = false;
    }

    ~DecisionEvent()
    {

    }

    public override void begin()
    {
        
        currentSelect = 0;

        Debug.Log("selection count");
        Debug.Log(selection.Count);
        // display the options
        //UI.GetComponent<UIhandler>().displaySelection(selection);
        //UIhandler.UI.displayCursor(currentSelect);
    }

    public override void OnInput()
    {

        if (Input.GetMouseButtonDown(0))//Input.GetKeyDown(KeyCode.DownArrow))
        {

            Debug.Log(currentSelect);
            
            currentSelect++;
            currentSelect %= selection.Count;
            //currentSelect %= selection.Count;

            //UIhandler.UI.displayCursor(currentSelect);
        }


        if (Input.GetMouseButtonDown(1))//Input.GetKeyDown(KeyCode.Return))
        {
            //EManager.GetComponent<eventManager>().InsertEvents( selection[currentSelect].events );
            //Choose();

            eventManager.manager.InsertEvents(selection[currentSelect].events);
            End = true;
        }
    }

    /*
    private void Choose()
    {

        List<BaseEvent> meh = new List<BaseEvent>();

        
        // load events
        // add events to events list
        // end event
        /*
        switch (selection[currentSelect].type)
        {
            case Effect.gotoEvent:
                gotoEvent();
                break;
            case Effect.skipEvent:
                skipEvent();
                break;
            case Effect.addDialog:
                addDialog();
                break;
        }
        */
    //} 

    public override string nextEvent()
    {

        return "";
    }

}


// Character Change event class
public class CharacterChangeEvent : BaseEvent
{

    // attributes
    private string file;
    private string charNum;
    private bool blocking;

    // constructor/destructor
    public CharacterChangeEvent(string filePath, string characterNum, bool block = false)
    {

        // set attributes

        file = filePath;
        charNum = characterNum;
        blocking = block;

        

        End = false;
    }

    ~CharacterChangeEvent()
    {

    }


    // begin is called when event starts
    public override void begin()
    {

        // display character
        UIhandler.UI.ChangeImageSprite(charNum, file);


        // check if event is blocking
        if (!blocking)
        {

            // end the event
            End = true;
        }

    }

    // OnMouse Down is called when left mouse button is pressed
    public override void OnInput()
    {
        if (Input.GetMouseButtonDown(0))
        {

            // end the event
            End = true;
        }
    }

    // returns next event
    public override string nextEvent()
    {
        return nextEventList[0];
    }

}


// dialog event class
public class DialogEvent : BaseEvent
{

    // List of dialog used
    public List<Dialog> dialog;

    // index of current dialog
    private int currentDialog;

    // pointers to UI/AUDIO objects
    private GameObject Audio;



    // constructor/destructor
    public DialogEvent(List<Dialog> newDialog)//, string nextEvent, GameObject setUI, GameObject setAudio )
    {

        // set current dialog to start
        currentDialog = 0;

        // set attributes
        dialog = newDialog;

        //nextEventList.Add(nextEvent);

        

        //UI = setUI;
        //Audio = setAudio;

        End = false;

    }


    // called when event starts
    public override void begin()
    {

        Audio = GameObject.FindGameObjectWithTag("Canvas");

        //Debug.Log("begin is happening");

        UIhandler.UI.DisplayText(dialog[currentDialog]);

        Debug.Log(dialog[currentDialog].name);
        Debug.Log(dialog[currentDialog].message);
        // UI display first dialog

    }

    // when mouse button down
    public override void OnInput()
    {

        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("displaying next text");
            Debug.Log(dialog[currentDialog].message);
            Debug.Log(dialog[currentDialog].name);

            // goto next dialog 
            currentDialog++;

            // check if at end of dialog
            if (currentDialog < dialog.Count)
            {

                // if there is dialog display it

                //Debug.Log("mouse down displaying new poop hehe got 'em");

                UIhandler.UI.DisplayText(dialog[currentDialog]);

                //Debug.Log(dialog[currentDialog].name);
                // UI display dialog[currentDialog]
            }
            else
            {

                // else end the event
                End = true;
            }
        }
    }

    // returns location of next event
    public override string nextEvent()
    {

        return nextEventList[0];
    }

}


public class GotoNextEvent : BaseEvent
{

    //eventManager manager;

    // constructor/destructor
    public GotoNextEvent()
    {

        End = true;

    }

    ~GotoNextEvent()
    {

    }


    // virtual methods for inheritance
    //
    // begin is called when event starts
    public override void begin()
    {
        //manager = GameObject.FindGameObjectWithTag("EventManager").GetComponent<eventManager>();

        eventManager.manager.SkipEvents();

        
    }

    // OnMouse Down is called when left mouse button is pressed
    public override void OnInput()
    {

        if (Input.GetMouseButtonDown(0))
        {
            End = true;
        }
    }

}


public class GameEvent : BaseEvent
{

    string gameScene;


    public GameEvent(string SceneName)
    {
        gameScene = SceneName;

        End = false;
    }

    public override void begin()
    {

        UIhandler.UI.Hide();

        SceneManager.LoadScene(gameScene);

    }


    public override void OnInput()
    {
        
    }

    public void ReturnToMain()
    {

        SceneManager.LoadScene("ui-test");

        

        UIhandler.UI.Show();

        End = true;


    }

}
