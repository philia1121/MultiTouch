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
    public Vector2 point1,point2,corner;
    public int side;

    public points_side(Vector2 p1, Vector2 p2, Vector2 p3, int s)
    {
        point1 = p1;
        point2 = p2;
        corner = p3;
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
    [HideInInspector]public Vector2 centercoords; //三角形重心座標((A+B+C)/3)
    public List<Vector2> ABC = new List<Vector2>(); //接touch端得到的三點
    public bool camOn;
    [HideInInspector]public touch7 t7;

    [SerializeField]private string tritype;
    private Vector2 dir;
    private List<points_side> sides = new List<points_side>(); //轉換計算得三點與對應邊長
    private LineRenderer linerenderer;
    [SerializeField]private float line_trans;
    

    public GameObject dir_obj, cam_obj;

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
                linerenderer.SetPosition(i, Camera.main.ScreenToWorldPoint(new Vector3(ABC[i].x, ABC[i].y, this.transform.position.z)));
            }
            else//3
            {
                linerenderer.SetPosition(i, Camera.main.ScreenToWorldPoint(new Vector3(ABC[0].x, ABC[0].y, this.transform.position.z)));
            }
        }

        //找最長邊面朝方向
        LongSideDir(sides[2].point1, sides[2].point2, sides[2].corner);
        showDir();

        CreateCam();

        
    }

    //轉換求點與對應邊長
    void Getsides(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        sides.Add(new points_side(p1,p2,p3,Mathf.RoundToInt(Vector2.Distance(p1,p2))));
        sides.Add(new points_side(p2,p3,p1,Mathf.RoundToInt(Vector2.Distance(p2,p3))));
        sides.Add(new points_side(p1,p3,p2,Mathf.RoundToInt(Vector2.Distance(p1,p3))));

        sides.Sort();
    }

    //分辨三角形種類+根據種類分配line顏色
    string TriangleType(int a, int b, int c)
    {
        if(Mathf.Abs((a+b+c)/3-a)<10 && Mathf.Abs((a+b+c)/3-b)<10 && Mathf.Abs((a+b+c)/3-c)<10)
        {
            linerenderer.startColor = new Color(0, 0.3f, 1, line_trans);
            linerenderer.endColor = linerenderer.startColor;
            return "正三角形";
        }
        else
        {
            //角度小於90度則cos值為正，大於90度為負，0度時cos值為1，90度時cos值為0

            float BIGcos = CosC(a,b,c); 
            if(Mathf.Abs(BIGcos) < 0.15f)
            {
                linerenderer.startColor = new Color(1, 0.6f, 0, line_trans);
                linerenderer.endColor = linerenderer.startColor;
                return "直角三角形";
            }
            else if(BIGcos > 0.15f)
            {
                linerenderer.startColor = new Color(0.3f, 0.7f, 0, line_trans);
                linerenderer.endColor = linerenderer.startColor;
                return "銳角三角形";
            }
            else
            {
                linerenderer.startColor = new Color(0.4f, 0, 0.9f, line_trans);
                linerenderer.endColor = linerenderer.startColor;
                return "鈍角三角形";
            }
            
        }
    }
    
    //餘弦定理 求最大角cos值
    float CosC(int a, int b, int c)
    {
        return (Mathf.Pow(a,2)+Mathf.Pow(b,2)-Mathf.Pow(c,2))/(2*a*b);
    }

    //找最長邊面朝方向向量dir
    void LongSideDir(Vector2 A, Vector2 B, Vector2 C)
    {
        var CM = getV(C, (A+B)/2).normalized; //C點到AB中點M的向量
        var AB = getV(A, B);
        dir = new Vector2(AB.y, -AB.x); //原始值的AB法向量
        var dir_n = dir.normalized;  

        if((CM+dir_n).magnitude > (CM-dir_n).magnitude) //判斷法向量方向是否指向三角形外
        {
            print(dir);
        }
        else
        {
            dir = -dir;
            print(dir);
        }


    }

    //速求p1p2方向向量
    Vector2 getV(Vector2 p1, Vector2 p2)
    {
        return (p2-p1);
    }

    //顯示最長邊面朝方向向量dir
    void showDir()
    {
        var r = Mathf.RoundToInt(Vector2.Distance(new Vector2(0,0),dir));
        var cosine = dir.x/r;
        var theta = Mathf.Acos(cosine)*(180/ Math.PI); 
        if(theta<90 & dir.y<0)
        {
            theta = 360-theta;
        }
        else if(theta >90 & dir.y<0)
        {
            theta = 360-theta;
        }
        //print(theta);

        var M = (sides[2].point1+ sides[2].point2)/2;
        var v = new Vector3(0,0,(float)theta);
        var showdir = Instantiate(dir_obj, Camera.main.ScreenToWorldPoint(new Vector3(M.x, M.y, this.transform.position.z)), Quaternion.Euler(v));
        showdir.transform.parent = this.transform;
        
    }

    //產生camera capture材質的物件
    void CreateCam()
    {
        if(camOn && tritype == "鈍角三角形")
        {
            var N = (sides[2].point1+ sides[2].point2)/2 + dir.normalized*150;
            var temp = Instantiate(cam_obj, Camera.main.ScreenToWorldPoint(new Vector3(N.x, N.y, 8)), Quaternion.Euler(0, 0, 180));
            temp.transform.parent = this.transform;
            //t7.cam = false; //是否允許場上出現多個使用webcamtexture的物件
        }
    }
}
