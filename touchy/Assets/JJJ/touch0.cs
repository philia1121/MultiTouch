using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using TouchScript;
using TouchScript.Pointers;
using UnityEngine.Events;

public class touch0 : MonoBehaviour
{
    Dictionary<Pointer, GameObject> BallArray = new Dictionary<Pointer, GameObject>();
   

    private void OnEnable()
    {
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.PointersPressed += pointersPressedHandler;
            TouchManager.Instance.PointersReleased += pointersReleaseHandler;
        }

    }

    private void OnDisable()
    {
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.PointersPressed -= pointersPressedHandler;
            TouchManager.Instance.PointersReleased -= pointersReleaseHandler;
        }
    }

    private void pointersPressedHandler(object sender, PointerEventArgs e)
    {
        foreach (var pointer in e.Pointers)
        {
            print(pointer.Id + ":pressed" + " " + pointer.Position);
            
        }
        
        
    }

    private void pointersReleaseHandler(object sender, PointerEventArgs e)
    {
        foreach (var pointer in e.Pointers)
        {
            BallArray.Remove(pointer);
            print(pointer.Id + ":released" + " " + pointer.Position);
        }
    }
    
    
    
}
