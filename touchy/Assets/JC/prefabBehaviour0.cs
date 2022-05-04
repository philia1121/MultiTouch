using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using TouchScript;
using TouchScript.Pointers;
using UnityEngine.Events;
using System;

//登記三角形組成點座標、邊長等資料的多項list
public class points_side : IComparable<points_side>
{
    public Vector2 point1,point2;
    public int side;

    public points_side(Vector2 p1, Vector2 p2, int s)
    {
        point1 = p1;
        point2 = p2;
        side = s;
    }

    //照邊長side降冪排序
    public int CompareTo(points_side other)
    {
        if (this.side > other.side)
        {
            return 1;
        }
        else if (this.side < other.side)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

}

//搭配touch7
public class prefabBehaviour0 : MonoBehaviour
{
    public int id;
    public string tritype;
    public Vector2 centercords; //三角形重心座標((A+B+C)/3)
    public List<Vector2> ABC = new List<Vector2>(); //接touch端得到的三點
    private List<points_side> sides = new List<points_side>(); //轉換計算得三點與對應邊長
    private LineRenderer linerenderer;

    public GameObject dir;

    private void Start()
    {
        linerenderer = this.GetComponent<LineRenderer>();
        Getsides(ABC[0],ABC[1],ABC[2]);
        tritype = TriangleType(sides[0].side, sides[1].side, sides[2].side);

        //畫出三角形
        for(int i = 0; i<4 ; i++)//0123
        {
            if(i<3)//012
            {
                linerenderer.SetPosition(i, Camera.main.ScreenToWorldPoint(new Vector3(ABC[i].x, ABC[i].y, 8.0f)));
            }
            else//3
            {
                linerenderer.SetPosition(i, Camera.main.ScreenToWorldPoint(new Vector3(ABC[0].x, ABC[0].y, 8.0f)));
            }
        }
        //顯示長邊面朝方向
        //facing();
    }

    //轉換求點與對應邊長
    void Getsides(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        sides.Add(new points_side(p1,p2,Mathf.RoundToInt(Vector2.Distance(p1,p2))));
        sides.Add(new points_side(p2,p3,Mathf.RoundToInt(Vector2.Distance(p2,p3))));
        sides.Add(new points_side(p1,p3,Mathf.RoundToInt(Vector2.Distance(p1,p3))));

        sides.Sort();
    }

    //分辨三角形種類+根據種類分配line顏色
    string TriangleType(int a, int b, int c)
    {
        if(Mathf.Abs((a+b+c)/3-a)<15 && Mathf.Abs((a+b+c)/3-b)<15 && Mathf.Abs((a+b+c)/3-c)<15)
        {
            linerenderer.startColor = new Color(0, 0.3f, 1, 1);
            linerenderer.endColor = linerenderer.startColor;
            return "正三角形";
        }
        else
        {
            float BIGcos = CosC(a,b,c); 
            if(Mathf.Abs(BIGcos) < 0.15f)
            {
                linerenderer.startColor = new Color(1, 0.6f, 0, 1);
                linerenderer.endColor = linerenderer.startColor;
                return "直角三角形";
            }
            else if(BIGcos > 0.15f)
            {
                linerenderer.startColor = new Color(0.3f, 0.7f, 0, 1);
                linerenderer.endColor = linerenderer.startColor;
                return "銳角三角形";
            }
            else
            {
                linerenderer.startColor = new Color(0.4f, 0, 0.9f, 1);
                linerenderer.endColor = linerenderer.startColor;
                return "鈍角三角形";
            }
            
        }
    }
    
    //求最大角cos值
    float CosC(int a, int b, int c)
    {
        return (Mathf.Pow(a,2)+Mathf.Pow(b,2)-Mathf.Pow(c,2))/(2*a*b);
    }

    void facing()
    {
        if(tritype != "正三角形")
        {
            var midcords = Camera.main.ScreenToWorldPoint(new Vector3((sides[2].point1.x+sides[2].point2.x)/2, (sides[2].point1.y+sides[2].point2.y)/2, 8.0f));
            var facing = new Vector3(-midcords.y, 0, midcords.x);
            Instantiate(dir, midcords, Quaternion.Euler(facing));
        }
    }

}
