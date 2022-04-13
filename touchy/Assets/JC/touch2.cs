using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using TouchScript;
using TouchScript.Pointers;
using UnityEngine.Events;

public class touch2 : MonoBehaviour
{
    Dictionary<Pointer, GameObject> BallArray = new Dictionary<Pointer, GameObject>();
    public Dictionary<int, Vector2> touchy = new Dictionary<int, Vector2>();

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


    
    private void pointersPressedHandler(object sender, PointerEventArgs e)//第一個按下接觸點的位置
    {
        foreach (var pointer in e.Pointers)
        {
            print(pointer.Id + ":pressed" + pointer.Position);
            touchy.Add(pointer.Id, pointer.Position); 
            printDic();
            spawn();
        }
        
        //read(e);
        
    }

    private void pointersReleaseHandler(object sender, PointerEventArgs e)//手離開時最後一個接觸點的位置，如果手按下去有移動的話位置會不一樣
    {
        foreach (var pointer in e.Pointers)
        {
            print(pointer.Id + ":released"+ pointer.Position);
            touchy.Remove(pointer.Id);
            printDic();
        }
    }
    
    public GameObject prefab;
    void spawn()
    {
        if(touchy.Count==3)
        {
            Vector3 center = new Vector3(0,0,0);
            foreach(var t in touchy)
            {
                center = center + Camera.main.ScreenToWorldPoint(new Vector3(t.Value.x, t.Value.y, 10));
            }
            Instantiate(prefab, center/3, Quaternion.identity);
            touchy.Clear();
            printDic();
        }
    }

    

    void printDic()
    {
        print(touchy.Count);
        foreach(KeyValuePair<int, Vector2> kvp in touchy)
        {
            print("POINTER" + kvp.Key + " " + kvp.Value);
        }
    }
    
}
