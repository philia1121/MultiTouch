using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class button : MonoBehaviour
{
    public void quit()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false; //unity editor用的停止play mode方法
        #else
        Application.Quit(); //build成專案檔用的停止play mode方法
        #endif
    }

    public GameObject[] triangles;
    public void clearall()
    {
        triangles = GameObject.FindGameObjectsWithTag("triangle");
        foreach(var tri in triangles)
        {
            Destroy(tri); //清除所有已生成的三角形代表物件
        }
        var t7 = GameObject.Find("touchcontroller").GetComponent<touch7>();
        if(t7)
        {
            t7.cam = true;
        }
        
    }

    public GameObject CursorController;
    [SerializeField]private bool on;
    public int counting = 0;
    public void CursorOn()
    {
        counting++;
        if(counting%2 != 0) //看上去是點一下但程式端會算兩下 不信可以拆掉自己試試
        {
            CursorController.SetActive(on);
            on = !on;
        }
    }

    
}
