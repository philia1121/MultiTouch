using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class callout : ScriptableObject
{
    public UnityEvent called = new UnityEvent();
    // Start is called before the first frame update
    
    private static callout singleton;
    public static callout Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = CreateInstance<callout>();
            }
            return singleton;
        }
    }
    public UnityEvent onCustomEvent = new UnityEvent();

}
