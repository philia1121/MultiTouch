using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using TouchScript;
using TouchScript.Pointers;
using UnityEngine.Events;

public class touch3 : MonoBehaviour
{
    public Dictionary<int, Vector2> touchy = new Dictionary<int, Vector2>();

    public List<Vector2> G, newG = new List<Vector2>();

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
            //print(pointer.Id + ":pressed" + pointer.Position);
            touchy.Add(pointer.Id, pointer.Position); 
        }

        G.Clear();
        GetDic();
        
        if(G.Count >= 3)
        {
            sorting();
        }
        
    }

    private void pointersReleaseHandler(object sender, PointerEventArgs e)//手離開時最後一個接觸點的位置，如果手按下去有移動的話位置會不一樣
    {
        foreach (var pointer in e.Pointers)
        {
            //print(pointer.Id + ":released"+ pointer.Position);
            touchy.Remove(pointer.Id);
        }

        G.Clear();
    }
    
 

    void printDic()
    {
        print(touchy.Count);
        foreach(KeyValuePair<int, Vector2> kvp in touchy)
        {
            print("POINTER" + kvp.Key + " " + kvp.Value);
        }
    }
    
    void GetDic()
    {
        foreach(KeyValuePair<int, Vector2> kvp in touchy)
        {
            G.Add(kvp.Value);
        }
    }

    public GameObject prefab;
    void sorting()
    {
        newG.Add(G[0]);
        for(int k = 1; k < G.Count; k++)
        {
            if(Vector2.Distance(G[0], G[k]) < Screen.width/4)
            {
                newG.Add(G[k]);
            }
        }

        if(newG.Count >= 3)
        {
            Vector3 center = new Vector3(0,0,0);
            foreach(var pos in newG)
            {
                center = center +  Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 8.0f));
            }
            
            Instantiate(prefab, center/newG.Count, Quaternion.identity);
        }

        foreach(var value in newG)
        {
            G.Remove(value);
        }
        newG.Clear();

        if(G.Count >= 3)
        {
            sorting();
        }
        
    }
}
