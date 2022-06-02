using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using TouchScript;
using TouchScript.Pointers;
using UnityEngine.Events;
using System.Linq; // for ToList(), Except()

//搭配prefabBehaviour0
public class touch7 : MonoBehaviour
{
    private Dictionary<int, Vector2> touchy = new Dictionary<int, Vector2>();
    
    public int R = 4;
    private List<Vector2> G,Used = new List<Vector2>();
    private List<int> side = new List<int>();
    private int cubeID = 1;
    public bool cam;
    

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
            //print("pressed");
        }

        //printDic();
        G = touchy.Values.ToList(); //讀所有的點
        G = G.Except(Used).ToList(); //去掉已經用過的點

        if(G.Count >= 3) //如果剩下的點超過3個再來看有沒有鄰近可以成組的
        {
            //print("go sort");
            sorting(G,R);
        }
        
    }

    private void pointersReleaseHandler(object sender, PointerEventArgs e)//手離開時最後一個接觸點的位置，如果手按下去有移動的話位置會不一樣
    {
        foreach (var pointer in e.Pointers)
        {
            //print(pointer.Id + ":released"+ pointer.Position);
            if(Used.Contains(touchy[pointer.Id]))
            {
                Used.Remove(touchy[pointer.Id]);
            }
            touchy.Remove(pointer.Id);
        }

    }
    
    public GameObject prefab;
    void sorting(List<Vector2> All,int radius)
    {
        List<Vector2> Near = new List<Vector2>();

        Near.Add(All[0]);
        for(int k = 1; k < All.Count; k++)
        {
            if(Vector2.Distance(All[0], All[k]) < Screen.width/radius)
            {
                Near.Add(All[k]);
            }
        }

        if(Near.Count > 3)
        {
            //print("need to sort again");
        }
        else if(Near.Count == 3)
        {
            Vector3 center = new Vector3(0,0,0);
            foreach(var pos in Near)
            {
                center = center +  Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 0));
                Used.Add(pos);
            }
            
            CreateCube(prefab, new Vector3(center.x/3, center.y/3, 8.0f), cubeID, Near, cam);
            //cam = false; //要有人通知他到底上一次有沒有生webcamcube出來，才知道要不要換

        }
        
        foreach(var value in Near)
        {
            All.Remove(value);
        }
        Near.Clear();

        if(All.Count >= 3)
        {
            sorting(All, radius);
        }
        
    }

    //生成prefab的同時把與其相關的一些資料丟給他
    void CreateCube(GameObject obj, Vector3 prefabcoords, int id, List<Vector2> points, bool tf)
    {
        var prefab = Instantiate(obj, prefabcoords, Quaternion.identity);
        prefab.name = "tri" + id.ToString();
        
        var behaviour = prefab.GetComponent<prefabBehaviour0>();
        behaviour.id = id;
        behaviour.camOn = tf;
        behaviour.t7 = this.GetComponent<touch7>();
        cubeID++;

        foreach(var p in points) //把生成這個prefab的三個點座標傳過去
        {
            behaviour.ABC.Add(p);
            behaviour.centercoords += p;
        }
        behaviour.centercoords = behaviour.centercoords/3;
    }
}
