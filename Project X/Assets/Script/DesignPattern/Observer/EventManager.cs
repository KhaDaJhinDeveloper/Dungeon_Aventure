using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public static EventManager OP_EventManager;
    private static Dictionary<string, Delegate> eventDictionary = new Dictionary<string, Delegate>();
    protected override void Awake()
    {
        base.Awake();
        OP_EventManager = this;
    }
    public void Subscribe(string eventName, Action listener)
    {
        if(!eventDictionary.ContainsKey(eventName))
            eventDictionary[eventName] = listener;
        else
            eventDictionary[eventName] = (Action)eventDictionary[eventName] + listener;
    }
    public void Subscribe<T>(string eventName, Action<T> listener)
    {
        if (!eventDictionary.ContainsKey(eventName))
            eventDictionary[eventName] = listener;
        else
            eventDictionary[eventName] = (Action<T>)eventDictionary[eventName] + listener;
    }
    public void Subscribe<T1,T2>(string eventName, Action<T1, T2> listener)
    {
        if (!eventDictionary.ContainsKey(eventName))
            eventDictionary[eventName] = listener;
        else
            eventDictionary[eventName] = (Action<T1, T2>)eventDictionary[eventName] + listener;
    }
    //-----------------------------------------------------------
    public void Unsubscribe(string eventName, Action listener)
    {
        if (eventDictionary.ContainsKey(eventName))
            eventDictionary[eventName] = (Action)eventDictionary[eventName] - listener;
    }
    public void Unsubscribe<T>(string eventName, Action<T> listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] = (Action<T>)eventDictionary[eventName] - listener;
            if (eventDictionary[eventName] == null)
                eventDictionary.Remove(eventName);
        }    
    }
    public void Unsubscribe<T1, T2>(string eventName, Action<T1, T2> listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] = (Action<T1, T2>)eventDictionary[eventName] - listener;
            if (eventDictionary[eventName] == null)
                eventDictionary.Remove(eventName);
        }
    }
    //-----------------------------------------------------------
    public void TriggerEvent(string eventName)
    {
        if (eventDictionary.ContainsKey(eventName) && eventDictionary[eventName] is Action action)
            action.Invoke();
    }
    public void TriggerEvent<T>(string eventName, T param)
    {
        if (eventDictionary.ContainsKey(eventName) && eventDictionary[eventName] is Action<T> action)
            action.Invoke(param);
    }
    public void TriggerEvent<T1, T2>(string eventName, T1 param1, T2 param2)
    {
        if (eventDictionary.ContainsKey(eventName) && eventDictionary[eventName] is Action<T1, T2> action)
            action.Invoke(param1, param2);
    }
}
