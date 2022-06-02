using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class button : MonoBehaviour
{
    public void quit()
    {
        Application.Quit();
    }

    public GameObject[] triangles;
    public void clearall()
    {
        triangles = GameObject.FindGameObjectsWithTag("triangle");
        foreach(var tri in triangles)
        {
            Destroy(tri);
        }
        var t7 = GameObject.Find("touchcontroller").GetComponent<touch7>();
        t7.cam = true;
    }

    public GameObject CursorController;
    [SerializeField]private bool on;
    public int counting = 0;
    public void CursorOn()
    {
        counting++;
        if(counting%2 != 0)
        {
            CursorController.SetActive(on);
            on = !on;
        }
        
    }

    
}
