using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using TouchScript;
using TouchScript.Pointers;
using UnityEngine.Events;
using System.Linq; // for ToList(), Except()

//可以分類三角形類型(正三角、銳角、鈍角、直角三角形)並生成相應顏色的cube

public class touch6 : MonoBehaviour
{
    public Dictionary<int, Vector2> touchy = new Dictionary<int, Vector2>();
    
    public List<Material> mat = new List<Material>(); //用四種顏色的mat去區分四種三角形判斷結果
    [HideInInspector]
    public List<Vector2> G,Used = new List<Vector2>();
    public List<int> side = new List<int>();
    

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
            //print(pointer.Id + ":released"+ pointer.Position);
            touchy.Add(pointer.Id, pointer.Position); //Position是按下時接觸點所在的位置
        }

        //printDic();
        G = touchy.Values.ToList(); //讀所有的點
        G = G.Except(Used).ToList(); //去掉已經用過的點

        if(G.Count >= 3) //如果剩下的點超過三個再來看有沒有鄰近可以成組的
        {
            //print("go sort");
            sorting(G,3);
        }
        
    }

    private void pointersReleaseHandler(object sender, PointerEventArgs e)
    {
        foreach (var pointer in e.Pointers)
        {
            //print(pointer.Id + ":released"+ pointer.Position); //Position是手放開時接觸點所在的位置 可能會跟按下去時的不一樣
            if(Used.Contains(touchy[pointer.Id]))
            {
                Used.Remove(touchy[pointer.Id]);
            }
            touchy.Remove(pointer.Id);
        }

    }
    

    void printDic()
    {
        print(touchy.Count);
        foreach(KeyValuePair<int, Vector2> kvp in touchy)
        {
            print(kvp.Key + " " + kvp.Value);
        }
    }
    

    public GameObject prefab;
    void sorting(List<Vector2> All, int radius)
    {
        List<Vector2> Near = new List<Vector2>();

        Near.Add(All[0]);
        for(int k = 1; k < All.Count; k++)
        {
            if(Vector2.Distance(All[0], All[k]) < Screen.width/radius) //如果兩點相距小於(Screen.width/radius)則算成一組
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
            Vector3 center = Vector3.zero;
            foreach(var pos in Near)
            {
                center = center +  Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 8.0f));
                Used.Add(pos); //把用過(已經組成三角形)的點登記起來
            }
            
            GetSide(Near); //取三角形三邊長並排序
            TriangleType(side[0], side[1], side[2]); //用三角形三邊長求cos值 分辨三角形的種類
            Instantiate(prefab, center/3, Quaternion.identity);
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

    void GetSide(List<Vector2> p)
    {
        if(side.Any())
        {
            side.Clear();
        }
        side.Add(Mathf.RoundToInt(Vector2.Distance(p[0], p[1])));
        side.Add(Mathf.RoundToInt(Vector2.Distance(p[0], p[2])));
        side.Add(Mathf.RoundToInt(Vector2.Distance(p[1], p[2])));

        side.Sort();
        // side[0] < side[1] < side[2] 三角形三邊
        //    a    <    b    <    c
    }

    void TriangleType(int a, int b, int c)
    {
        
        if(Mathf.Abs((a+b+c)/3-a)<20 && Mathf.Abs((a+b+c)/3-b)<20 && Mathf.Abs((a+b+c)/3-c)<20)
        {
            print("正三角形");
            prefab.GetComponent<Renderer>().material = mat[2];
        }
        else
        {
            //print(Mathf.RoundToInt(a/10) +","+ Mathf.RoundToInt(b/10) +","+ Mathf.RoundToInt(c/10));
            //print("Cos C :"+ CosC(Mathf.RoundToInt(a/10),Mathf.RoundToInt(b/10),Mathf.RoundToInt(c/10)));
            float BIGcos = CosC(Mathf.RoundToInt(a/10),Mathf.RoundToInt(b/10),Mathf.RoundToInt(c/10)); 
            if(Mathf.Abs(BIGcos) < 0.15f)
            {
                print("直角三角形");
                prefab.GetComponent<Renderer>().material = mat[1];
            }
            else if(BIGcos > 0.15f)
            {
                print("銳角三角形");
                prefab.GetComponent<Renderer>().material = mat[0];
            }
            else
            {
                print("鈍角三角形");
                prefab.GetComponent<Renderer>().material = mat[3];
            }
            
        }
    }

    float CosC(int a, int b, int c)
    {
        return (Mathf.Pow(a,2)+Mathf.Pow(b,2)-Mathf.Pow(c,2))/(2*a*b);
    }
}
