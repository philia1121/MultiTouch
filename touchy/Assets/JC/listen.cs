using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class listen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        callout.Singleton.onCustomEvent.AddListener(PrintSomething);
        callout.Singleton.onCustomEvent.Invoke();
    }

    // Update is called once per frame
    public void PrintSomething()
    {
        Debug.Log("heard!");
    }
}
