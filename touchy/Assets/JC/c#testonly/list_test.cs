using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/*
public class tri_points : IComparable<tri_points>
{
    public Vector2 point1,point2;
    public float side;

    public tri_points(Vector2 p1, Vector2 p2, float s)
    {
        point1 = p1;
        point2 = p2;
        side = s;
    }

    //照邊長降冪排序
    public int CompareTo(tri_points other)
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
public class list_test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<tri_points> points = new List<tri_points>();

        Vector2 a = new Vector2(0,6);
        Vector2 b = new Vector2(2,4);
        Vector2 c = new Vector2(1,0);

        points.Add(new tri_points(a,b,Vector2.Distance(a,b)));
        points.Add(new tri_points(b,c,Vector2.Distance(b,c)));
        points.Add(new tri_points(a,c,Vector2.Distance(a,c)));

        points.Sort();
        foreach(tri_points tp in points)
        {
            print(tp.point1+","+tp.point2+" side:"+tp.side);
        }
    }

    
}*/
