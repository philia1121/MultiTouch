using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using TouchScript;
using TouchScript.Pointers;
using UnityEngine.Events;

public class touch1 : MonoBehaviour
{

    //DEMO-0316 (deprecated)

    //public UnityEvent onPressed;
    Dictionary<Pointer, GameObject> BallArray = new Dictionary<Pointer, GameObject>();
    public List<Vector3> mylist = new List<Vector3>();

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
            print(pointer.Id + ":pressed");
            //print(pointer.Position);
            //onPressed?.Invoke();
            mylist.Add(pointer.Position);
            //Debug.Log("oriPointer" + pointer.Position);
            //Debug.Log("convert" + Camera.main.ScreenToWorldPoint(new Vector3(pointer.Position.x, pointer.Position.y, 1.0f)));
            spawn(3);
        }
        
        
    }

    private void pointersReleaseHandler(object sender, PointerEventArgs e)
    {
        foreach (var pointer in e.Pointers)
        {
            BallArray.Remove(pointer);
            print(pointer.Id + ":released");
        }
    }
    
    public GameObject prefab;

    private void spawn(int n)
    {
        if(mylist.Count>=n)
        {
            Vector3 center = new Vector3(0,0,0);
            print("good");
            foreach(Vector3 mylist in mylist) 
            {
                center = center +  Camera.main.ScreenToWorldPoint(new Vector3(mylist.x, mylist.y, 8.0f));
            }
            print(center/n);
            Instantiate(prefab, center/n, Quaternion.identity);
            mylist.Clear();
        }
    }
    
}