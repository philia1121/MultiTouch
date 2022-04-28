using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using TouchScript;
using TouchScript.Pointers;
using UnityEngine.Events;

public class prefabBehaviour : MonoBehaviour
{
    public int id;
    public string tritype;
    public Vector2 centercords; //三角形重心座標((A+B+C)/3)
    public List<Vector2> ABC = new List<Vector2>(); //ABC三點組成三角形
    private LineRenderer linerenderer;
    
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
        /*
        foreach(var pointer in e.Pointers) 
        {
            if(Vector2.Distance(pointer.Position, centercords) <= Screen.width/6)
            {
                print(pointer.Position + "in");
            }
            else
            {
                print("not in");
            }
        }*/

    }
    private void pointersReleaseHandler(object sender, PointerEventArgs e)
    {

    }

    private void Start()
    {
        linerenderer = this.GetComponent<LineRenderer>();
        SetLineColor();
        for(int i = 0; i<4 ; i++)
        {
            if(i<3)
            {
                linerenderer.SetPosition(i, Camera.main.ScreenToWorldPoint(new Vector3(ABC[i].x, ABC[i].y, 8.0f)));
            }
            else
            {
                linerenderer.SetPosition(i, Camera.main.ScreenToWorldPoint(new Vector3(ABC[0].x, ABC[0].y, 8.0f)));
            }
        }
    }
    void SetLineColor()
    {
        switch(tritype)
        {
            case "正三角形":
                linerenderer.startColor = new Color(0, 0.3f, 1, 1);
                break;

            case "直角三角形":
                linerenderer.startColor = new Color(1, 0.6f, 0, 1);
                break;

            case "銳角三角形":
                linerenderer.startColor = new Color(0.3f, 0.7f, 0, 1);
                break;

            case "鈍角三角形":
                linerenderer.startColor = new Color(0.4f, 0, 0.9f, 1);
                break;

            default:
                break;
        }
        linerenderer.endColor = linerenderer.startColor;
    }
}
